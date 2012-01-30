using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Timers;

using rp1210;
using log4net;
namespace rp1210
{
    class dgdFileReplay
    {
        public RP121032 J1939Instance { get; set; }
        public RP121032 J1587Instance { get; set; }
        public long TimeOffsetMs { get; set; }

        public ConcurrentQueue<J1939Message> TXQueue { get; set; }

        public bool Running { get; set; }

        public void dgdReplay()
        {
            Stopwatch timeKeeper = Stopwatch.StartNew();

            while (Running)
            {
                J1939Message TXData;
                if (TXQueue.TryPeek(out TXData))
                {
                    if (timeKeeper.ElapsedMilliseconds > (TXData.TimeStamp - TimeOffsetMs))
                    {
                        if (TXQueue.TryDequeue(out TXData))
                        {
                            byte[] data2send = RP121032.EncodeJ1939Message(TXData);
                            J1939Instance.RP1210_SendMessage(data2send, (short)data2send.Length, 0, RP121032.BLOCKING_IO);
                        }
                    }
                }
            }
        }

        public dgdFileReplay()
        {
        }
    }

    public partial class rp1210driver
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(rp1210driver));

        private RP121032 J1939inst;
        private RP121032 J1587inst;
        private RP1210BDriverInfo driverInfo;
        private DeviceInfo deviceInfo;
        private Thread DataPoller;

        public bool J1939Connected { get; set; }
        public bool J1587Connected { get; set; }

        private J1939Message nextJ1939Message;
        private J1587Message nextJ1587Message;

        public List<J1939Message> J1939MessageFilter;
        public List<J1587Message> J1587MessageFilter;

        public class PeriodicMessage: IDisposable
        {
            public enum PeriodicMessageType { J1939, J1587 };
            public delegate void txJ1939Data(J1939Message msgToSend);
            public delegate void txJ1587Data(J1587Message msgToSend);

            private bool disposed = false;

            private J1939Message _j1939Message;
            private J1587Message _j1587Message;
            private System.Timers.Timer _timeKeeper;
            private PeriodicMessageType _MessageType;
            private ElapsedEventHandler _EventHandler;

            public J1939Message j1939Message 
            { 
                get
                {
                    return _j1939Message;
                }
            }
            public J1587Message j1587Message
            {
                get
                {
                    return _j1587Message;
                }
            }
            public PeriodicMessageType MessageType
            {
                get
                {
                    return _MessageType;
                }
            }
            public txJ1939Data SendJ1939Data { get; set; }
            public txJ1587Data SendJ1587Data { get; set; }

            public PeriodicMessage(J1939Message msg, double interval, txJ1939Data txDelegate)
            {
                _MessageType = PeriodicMessageType.J1939;
                _timeKeeper = new System.Timers.Timer(interval);
                SendJ1939Data = txDelegate;
                _j1939Message = msg;
                _EventHandler = new ElapsedEventHandler(_timeKeeper_Elapsed);
                _timeKeeper.Elapsed += _EventHandler;
                _timeKeeper.Enabled = true;
            }

            public PeriodicMessage(J1587Message msg, double interval, txJ1587Data txDelegate)
            {
                _MessageType = PeriodicMessageType.J1587;
                _timeKeeper = new System.Timers.Timer(interval);
                SendJ1587Data = txDelegate;
                _j1587Message = msg;
                _EventHandler = new ElapsedEventHandler(_timeKeeper_Elapsed);
                _timeKeeper.Elapsed += _EventHandler;
                _timeKeeper.Enabled = true;
            }

            void _timeKeeper_Elapsed(object sender, ElapsedEventArgs e)
            {
                if (_MessageType == PeriodicMessageType.J1939)
                {
                    if ((_j1939Message != null) && (SendJ1939Data != null))
                    {
                        SendJ1939Data(_j1939Message);
                    }
                }
                else if (_MessageType == PeriodicMessageType.J1587)
                {
                    if ((_j1587Message != null) && (SendJ1587Data != null))
                    {
                        SendJ1587Data(_j1587Message);
                    }
                }
            }

            public void Dispose()
            {
                // Check to see if Dispose has already been called.
                if (!this.disposed)
                {
                    _timeKeeper.Enabled = false;
                    _timeKeeper.Elapsed -= _EventHandler;
                    // Release unmanaged resources.
                    try
                    {

                    
                    }
                    catch
                    {
                    }

                }
                disposed = true;
            }
            ~PeriodicMessage()
            {
                Dispose();
            }

        }
        public List<PeriodicMessage> PeriodicMessages { get; set; }

        private List<string> _DriverList;
        public List<string> DriverList
        {
            get
            {
                return _DriverList;
            }
        }

        private string _SelectedDriver;
        public string SelectedDriver
        {
            get
            {
                return _SelectedDriver;
            }
            set
            {
                _SelectedDriver = value;
                _DeviceList.Clear();
                driverInfo = RP121032.LoadDeviceParameters(Environment.GetEnvironmentVariable("SystemRoot") + "\\" + _SelectedDriver + ".ini");
                foreach (DeviceInfo entry in driverInfo.RP1210Devices)
                {
                    _DeviceList.Add(entry.DeviceName);
                }
            }
        }

        private List<string> _DeviceList;
        public List<string> DeviceList 
        {
            get
            {
                return _DeviceList;
            }
        }

        private string _SelectedDevice;
        public string SelectedDevice 
        {
            get
            {
                return _SelectedDevice;
            }
            set
            {
                _SelectedDevice = value;
                deviceInfo = driverInfo.RP1210Devices.Find(x => x.DeviceName == _SelectedDevice);
            }
        }

        public event DataRecievedHandler J1939DataRecieved;
        public event DataRecievedHandler J1587DataRecieved;

        public rp1210driver()
        {
            log.Debug("New RP1210 Driver Instance.");
            _DeviceList = new List<string>();
            _DriverList = RP121032.ScanForDrivers();
            SelectedDriver = _DriverList[0];
            PeriodicMessages = new List<PeriodicMessage>();

            J1939MessageFilter = new List<J1939Message>();
        }

        private void J1939AddressClaim()
        {
            // J1939 "NAME" for this sample source code application (see J1939/81)
            //    Self Configurable       =   0 = NO
            //    Industry Group          =   0 = GLOBAL
            //    Vehicle System          =   0 = Non-Specific
            //    Vehicle System Instance =   0 = First Diagnostic PC
            //    Reserved                =   0 = Must be zero
            //    Function                = 129 = Offboard Service Tool
            //    Function Instance       =   0 = First Offboard Service Tool
            //    Manufacturer Code       = 297 = Zonar Systems Inc
            //    Manufacturer Identity   =   0 = 

            byte[] J1939Name = { 0, 0, 0x20, 0x25, 0, 0x81, 0, 0 };
            byte[] TxBuffer = new byte[J1939Name.Length + 2];

            TxBuffer[0] = 0;  // Source Address of the Service Tool
            TxBuffer[1] = J1939Name[0];
            TxBuffer[2] = J1939Name[1];
            TxBuffer[3] = J1939Name[2];
            TxBuffer[4] = J1939Name[3];
            TxBuffer[5] = J1939Name[4];
            TxBuffer[6] = J1939Name[5];
            TxBuffer[7] = J1939Name[6];
            TxBuffer[8] = J1939Name[7];
            TxBuffer[9] = 0;                //block until done

            J1939inst.RP1210_SendCommand(RP1210_Commands.RP1210_Protect_J1939_Address, new StringBuilder(Encoding.UTF7.GetString(TxBuffer)), (short)TxBuffer.Length);
        }

        public void J1939Connect()
        {
            bool failed = false;
            if (J1939Connected)
            {
                if (J1939inst != null) J1939inst = null;

                //Need to have a status update event here
                //cmdConnect.Text = "Connect";
                J1939Connected = false;
            }
            else
            {
                J1939inst = new RP121032(_SelectedDriver);
                try
                {
                    J1939inst.RP1210_ClientConnect(deviceInfo.DeviceId, new StringBuilder("J1939"), 0, 0, 0);

                    DataPoller = new Thread(new ThreadStart(PollingDriver));
                    DataPoller.IsBackground = true;
                    DataPoller.Start();

                    // status event here
                    //txtStatus.Text = "SUCCESS - UserDevice= " + J1939inst.nClientID;
                    try
                    {
                        J1939inst.RP1210_SendCommand(RP1210_Commands.RP1210_Set_All_Filters_States_to_Pass, new StringBuilder(""), 0);

                        try
                        {
                            J1939AddressClaim();
                        }
                        catch (Exception err)
                        {
                            failed = true;
                            throw new Exception(err.Message);
                        }
                    }
                    catch (Exception err)
                    {
                        failed = true;
                        throw new Exception(err.Message);
                    }
                }
                catch (Exception err)
                {
                    failed = true;
                    // status event here
                    //txtStatus.Text = "FAILURE - " + err.Message;
                }
                if (!failed)
                    J1939Connected = true;
            }
        }

        public void J1587Connect()
        {
            bool failed = false;
            if (J1939Connected)
            {
                if (J1587inst != null) J1587inst = null;

                //Need to have a status update event here
                //cmdConnect.Text = "Connect";
                J1939Connected = false;
            }
            else
            {
                J1587inst = new RP121032(_SelectedDevice);
                try
                {
                    J1587inst.RP1210_ClientConnect(deviceInfo.DeviceId, new StringBuilder("J1708"), 0, 0, 0);
                    //txtStatus.Text = "SUCCESS - UserDevice= " + J1587inst.nClientID;
                }
                catch (Exception err)
                {
                    failed = true;
                    //txtStatus.Text = "FAILURE - " + err.Message;
                }

                try
                {
                    J1587inst.RP1210_SendCommand(RP1210_Commands.RP1210_Set_All_Filters_States_to_Pass, new StringBuilder(""), 0);
                }
                catch (Exception err)
                {
                    failed = true;
                    //txtStatus.Text = "FAILURE - " + err.Message;
                }
                if (!failed)
                {
                    //Need to throw status event
                    //cmdConnect.Text = "Disconnect";
                    J1587Connected = true;
                }
            }
        }

        public void J1939Disconnect()
        {
            if (J1939inst != null)
            {
                J1939Connected = false;
                J1939inst.RP1210_ClientDisconnect();
                J1939inst.Dispose();
                J1939inst = null;
            }
        }

        public void J1587Disconnect()
        {
            if (J1587inst != null)
            {
                J1587Connected = false;
                J1587inst.RP1210_ClientDisconnect();
                J1587inst.Dispose();
                J1587inst = null;
            }
        }

        public void Close()
        {
            try
            {
                foreach (PeriodicMessage msg in PeriodicMessages)
                {
                    msg.Dispose();
                }
                PeriodicMessages.Clear();
                DataPoller.Abort();
                J1939Disconnect();
                J1587Disconnect();
            }
            catch
            {
            }
        }

        public void SendPeriodicMessage(J1939Message msgToSend, double interval)
        {
            PeriodicMessage newMessage = new PeriodicMessage(msgToSend, interval, SendData);
            PeriodicMessages.Add(newMessage);
        }

        public void SendPeriodicMessage(J1587Message msgToSend, double interval)
        {
            PeriodicMessage newMessage = new PeriodicMessage(msgToSend, interval, SendData);
            PeriodicMessages.Add(newMessage);
        }

        public void SendData(J1939Message msgToSend)
        {
            if (J1939inst != null)
            {
                try
                {
                    byte[] txArray = RP121032.EncodeJ1939Message(msgToSend);
                    UInt32 canID = (UInt32)((msgToSend.Priority << 26) + (msgToSend.PGN << 8) + msgToSend.SourceAddress);
                    //string txline = "H TXJ1939, " + msgToSend.TimeStamp + ", " + canID.ToString("X") + ", " + zcrc.ByteArrayToHexString(msgToSend.data);
                    
                    // Send Data Event
                    //txtTX.AppendText(txline + Environment.NewLine);

                    RP1210_Returns returnTemp = J1939inst.RP1210_SendMessage(txArray, (short)txArray.Length, 0, 0);
                    // Status Event
                    //txtStatus.Text = returnTemp.ToString();
                }
                catch (Exception err)
                {
                    // Status Event here
                    //txtStatus.Text = err.Message.ToString();
                }
            }
        }

        public void SendData(J1587Message msgToSend)
        {
            if (J1587inst != null)
            {
                try
                {
                    byte[] txArray = msgToSend.ToArray();
                    RP1210_Returns returnTemp = J1939inst.RP1210_SendMessage(txArray, (short)txArray.Length, 0, 0);
                    // Status Event
                    //txtStatus.Text = returnTemp.ToString();
                }
                catch (Exception err)
                {
                    // Status Event here
                    //txtStatus.Text = err.Message.ToString();
                }
            }
        }

        /// <summary>
        /// This function is meant to be called as a seperate through to continuously
        /// poll the rp12010 message buffer looking for new messages since the driver
        /// is not event based. This function generates independent J1587 and J1939
        /// data recieved events
        /// </summary>
        private void PollingDriver()
        {
            while (true)
            {
                if (J1939inst != null)
                {
                    byte[] response = J1939inst.RP1210_ReadMessage(0);
                    if (response.Length > 1)
                    {
                        DataRecievedArgs EventArgs = new DataRecievedArgs();
                        EventArgs.J1939 = true;

                        rp1210.J1939Message message = RP121032.DecodeJ1939Message(response);

                        if (J1939MessageFilter.Count != 0)
                        {
                            if (J1939MessageFilter.Find(x => x.PGN == message.PGN) != null)
                            {
                                EventArgs.RecievedJ1939Message = message;
                            }
                        }
                        else
                        {
                            EventArgs.RecievedJ1939Message = message;
                        }

                        if ((J1939DataRecieved != null) && (EventArgs.RecievedJ1939Message != null))
                            J1939DataRecieved(this, EventArgs);
                    }
                }
                else if (J1587inst != null)
                {
                }
                else
                {
                    break;
                }
            }
        }

        public class DataRecievedArgs : EventArgs
        {
            public bool J1939 { get; set; }
            public bool J1587 { get; set; }
            public J1939Message RecievedJ1939Message { get; set; }
            public J1587Message RecievedJ1587Message { get; set; }
        }

        public delegate void DataRecievedHandler(object sender, DataRecievedArgs e);
    }
}
