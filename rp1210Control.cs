using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

using log4net;
using zonar_dotnet;

namespace rp1210
{
    public partial class rp1210Control : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(rp1210Control));

        private RP121032 J1939inst;
        private RP121032 J1587inst;
        private RP1210BDriverInfo driverInfo;

        private Stream RxLogFileStream;
        private Stream TxLogFileStream;
        private StreamWriter RxLogger;
        private StreamReader TxLogger;
        private bool bConnected = false;

        private J1939Message nextJ1939Message;
        private J1587Message nextJ1587Message;

        private long nextMessageTimeMs;
        private long TxLogTimeOffsetMs;
        private bool TimeOffsetsCalculated;
        private Stopwatch timeKeeper;

        public event DataRecievedHandler DataRecieved;
        protected virtual void OnDataRecieved(DataRecievedArgs e)
        {
            DataRecieved(this, e);
        }

        public rp1210Control()
        {
            log.Debug("New Control Instance.");
            InitializeComponent();
        }

        private void rp1210Control_Load(object sender, EventArgs e)
        {
            List<string> devicelist = RP121032.ScanForDrivers();
            cmbDriverList.DataSource = devicelist;
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
            //    Manufacturer Code       =  11 = Dearborn Group Technology
            //    Manufacturer Identity   =   0 = Dearborn Group Sample Source Code

            byte[] J1939Name = { 0, 0, 0x60, 1, 0, 0x81, 0, 0 };

            byte[] TxBuffer = new byte[J1939Name.Length + 2];

            //TxBuffer[0] = 129;  // Source Address of the Service Tool\
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

        private void cmdConnect_Click(object sender, EventArgs e)
        {
            bool failed = false;
            if (bConnected)
            {
                if (J1587inst != null) J1587inst = null;
                if (J1939inst != null) J1939inst = null;

                if (RxLogger != null) RxLogger.Close();
                if (RxLogFileStream != null) RxLogFileStream.Close();

                chkJ1587Enable.Enabled = true;
                chkJ1939Enable.Enabled = true;
                chkLogToFile.Enabled = true;
                cmdConnect.Text = "Connect";
                bConnected = false;
                tmrJ1939.Enabled = false;
            }
            else
            {
                if (chkLogToFile.Checked)
                {
                    SaveFileDialog newLogFile = new SaveFileDialog();

                    newLogFile.Filter = "dgd files (*.dgd)|*.dgd|All files (*.*)|*.*";
                    newLogFile.FilterIndex = 2;
                    newLogFile.RestoreDirectory = true;

                    if (newLogFile.ShowDialog() == DialogResult.OK)
                    {
                        if ((RxLogFileStream = newLogFile.OpenFile()) != null)
                        {
                            RxLogger = new StreamWriter(RxLogFileStream);
                        }
                    }
                }

                if (chkJ1939Enable.Checked)
                {
                    J1939inst = new RP121032(cmbDriverList.SelectedItem.ToString());
                    try
                    {
                        //J1939inst.RP1210_ClientConnect((short)150, new StringBuilder("J1939"), 0, 0, 0);
                        DeviceInfo selectedDevice = (DeviceInfo)cmbDeviceList.SelectedValue;
                        J1939inst.RP1210_ClientConnect(selectedDevice.DeviceId, new StringBuilder("J1939"), 0, 0, 0);
                        txtStatus.Text = "SUCCESS - UserDevice= " + J1939inst.nClientID;
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
                        txtStatus.Text = "FAILURE - " + err.Message;
                    }
                }
                if (chkJ1587Enable.Checked)
                {
                    J1587inst = new RP121032(cmbDriverList.SelectedItem.ToString());
                    try
                    {
                        J1587inst.RP1210_ClientConnect((short)cmbDeviceList.SelectedItem, new StringBuilder("J1708"), 0, 0, 0);
                        txtStatus.Text = "SUCCESS - UserDevice= " + J1587inst.nClientID;
                    }
                    catch (Exception err)
                    {
                        failed = true;
                        txtStatus.Text = "FAILURE - " + err.Message;
                    }

                    try
                    {
                        J1587inst.RP1210_SendCommand(RP1210_Commands.RP1210_Set_All_Filters_States_to_Pass, new StringBuilder(""), 0);
                    }
                    catch (Exception err)
                    {
                        failed = true;
                        txtStatus.Text = "FAILURE - " + err.Message;
                    }
                }

                if (!failed)
                {
                    chkJ1587Enable.Enabled = false;
                    chkJ1939Enable.Enabled = false;
                    chkLogToFile.Enabled = false;
                    cmdConnect.Text = "Disconnect";
                    bConnected = true;
                    tmrJ1939.Enabled = true;
                }
            }
        }

        private void cmbDriverList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!bConnected)
            {
                driverInfo = RP121032.LoadDeviceParameters(Environment.GetEnvironmentVariable("SystemRoot") + "\\" + cmbDriverList.SelectedItem.ToString() + ".ini");
                cmbDeviceList.DataSource = driverInfo.RP1210Devices;
                cmbDeviceList.DisplayMember = "DeviceId";
            }

        }

        private void tmrJ1939_Tick(object sender, EventArgs e)
        {
            if (J1939inst != null)
            {
                try
                {
                    while (true)
                    {
                        byte[] response = J1939inst.RP1210_ReadMessage(0);
                        DataRecievedArgs EventArgs = new DataRecievedArgs();
                        EventArgs.J1939 = true;

                        rp1210.J1939Message message = RP121032.DecodeJ1939Message(response);
                        EventArgs.RecievedJ1939Message = message;
                        string datastring = zcrc.ByteArrayToHexString(message.data);
                        string displayString = "RX J1939 - " + message.TimeStamp + " PGN: " + message.PGN + " SA: " + message.SourceAddress;
                        displayString += " DA: " + message.DestinationAddress + " Pri: " + message.Priority;
                        displayString += " Data: " + datastring + Environment.NewLine;
                        txtRX.AppendText(displayString);

                        datastring = datastring.Remove(datastring.Length - 1, 1);
                        datastring = datastring.Replace(" ", ", ");

                        if (chkLogToFile.Checked)
                        {
                            UInt32 canID = (UInt32)((message.Priority << 26) + (message.PGN << 8) + message.SourceAddress);
                            RxLogger.WriteLine("H RXJ1939, {0:d}, {1:x}, {2}", message.TimeStamp, canID, datastring);
                        }

                        OnDataRecieved(EventArgs);
                    }
                }
                catch (Exception err)
                {
                    txtStatus.Text = err.Message;
                }

                if (TxLogger != null)
                {
                    while (((timeKeeper ==null ) || (timeKeeper.ElapsedMilliseconds > nextMessageTimeMs)) && !TxLogger.EndOfStream)
                    {
                        string txline = TxLogger.ReadLine();
                        txtTX.AppendText(txline + Environment.NewLine);
                        txline = txline.Replace(" ", "");
                        string[] rawdata = txline.Split(new char[] { ',' });

                        if (!TimeOffsetsCalculated)
                        {
                            TxLogTimeOffsetMs = Convert.ToUInt32(rawdata[1]);
                            timeKeeper = Stopwatch.StartNew();
                            TimeOffsetsCalculated = true;
                        }
                        else
                        {
                            nextMessageTimeMs = Convert.ToUInt32(rawdata[1]) - TxLogTimeOffsetMs;
                        }

                        if ((rawdata[0] == "HRXJ1939") && (J1939inst != null))
                        {
                            nextJ1939Message = new J1939Message();
                            nextJ1939Message.TimeStamp = Convert.ToUInt32(rawdata[1]);
                            nextJ1939Message.SourceAddress = (short)(Convert.ToInt32(rawdata[2], 16) & 0x00FF);
                            nextJ1939Message.Priority = (byte)(Convert.ToInt32(rawdata[2], 16) >> 26);
                            nextJ1939Message.PGN = (UInt16)((Convert.ToInt32(rawdata[2], 16) >> 8) & 0xFFFF);
                            nextJ1939Message.DestinationAddress = 0xFF;
                            string[] strArrayTemp = new string[rawdata.Length - 3];
                            Array.Copy(rawdata, 3, strArrayTemp, 0, rawdata.Length - 3);
                            nextJ1939Message.data = Array.ConvertAll(strArrayTemp, x => Convert.ToByte(x, 16));
                            nextJ1939Message.dataLength = (UInt16)(rawdata.Length - 3);

                            if (timeKeeper.ElapsedMilliseconds > nextMessageTimeMs)
                            {
                                byte[] txArray = RP121032.EncodeJ1939Message(nextJ1939Message);
                                RP1210_Returns returnTemp = J1939inst.RP1210_SendMessage(txArray, (short)txArray.Length, 0, 0);
                                txtStatus.Text = returnTemp.ToString();
                            }
                        }
                        else if ((rawdata[0] == "HRXJ1708") && (J1587inst != null))
                        {
                            nextJ1587Message = new J1587Message();
                            nextJ1587Message.TimeStamp = Convert.ToUInt32(rawdata[1]);
                            nextJ1587Message.Priority = 8;
                            nextJ1587Message.MID = Convert.ToByte(rawdata[2],16);
                            nextJ1587Message.PID = Convert.ToByte(rawdata[3],16);
                            string[] strArrayTemp = new string[rawdata.Length - 4];
                            Array.Copy(rawdata, 3, strArrayTemp, 0, rawdata.Length - 4);
                            nextJ1587Message.data = Array.ConvertAll(strArrayTemp, x => Convert.ToByte(x, 16));
                            nextJ1587Message.dataLength = (UInt16)(rawdata.Length - 4);

                            if (timeKeeper.ElapsedMilliseconds > nextMessageTimeMs)
                            {
                                byte[] txArray = nextJ1587Message.ToArray();
                                RP1210_Returns returnTemp = J1587inst.RP1210_SendMessage(txArray, (short)(txArray.Length - 1), 0, 0);
                                txtStatus.Text = returnTemp.ToString();
                            }
                        }
                    }
                }
            }
            if (J1587inst != null)
            {
                byte[] response = J1587inst.RP1210_ReadMessage(0);

                rp1210.J1587Message message = RP121032.DecodeJ1587Message(response);
                string datastring = zcrc.ByteArrayToHexString(message.data);
                string displayString = "RX J1587 - " + message.TimeStamp + " MID: " + message.MID + " PID: " + message.PID;
                displayString += " Data: " + zcrc.ByteArrayToHexString(message.data) + Environment.NewLine;
                txtRX.AppendText(displayString);

                datastring = datastring.Remove(datastring.Length - 1, 1);
                datastring = datastring.Replace(" ", ", ");

                if (chkLogToFile.Checked)
                {
                    RxLogger.WriteLine("H RXJ1708, {0:d}, {1:x2}, {2:x2}, {3}", message.TimeStamp, message.MID, message.PID, datastring);
                }
            }

        }

        private void frmJBus_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (RxLogger != null) RxLogger.Close();
            if (RxLogFileStream != null) RxLogFileStream.Close();
        }

        private void cmdSend_Click(object sender, EventArgs e)
        {
            OpenFileDialog txlogfile = new OpenFileDialog();

            txlogfile.Filter = "dgd files (*.dgd)|*.dgd|All files (*.*)|*.*";
            txlogfile.FilterIndex = 1;
            txlogfile.RestoreDirectory = true;

            if (txlogfile.ShowDialog() == DialogResult.OK)
            {
                if ((TxLogFileStream = txlogfile.OpenFile()) != null)
                {
                    TxLogger = new StreamReader(TxLogFileStream);
                }
            }
        }


        public void SendData(J1939Message msgToSend)
        {
            if (J1939inst != null)
            {
                try
                {
                    byte[] txArray = RP121032.EncodeJ1939Message(msgToSend);
                    UInt32 canID = (UInt32)((msgToSend.Priority << 26) + (msgToSend.PGN << 8) + msgToSend.SourceAddress);
                    string txline = "H TXJ1939, " + msgToSend.TimeStamp + ", " + canID.ToString("X") + ", " + zcrc.ByteArrayToHexString(msgToSend.data);
                    txtTX.AppendText(txline + Environment.NewLine);

                    RP1210_Returns returnTemp = J1939inst.RP1210_SendMessage(txArray, (short)txArray.Length, 0, 0);
                    txtStatus.Text = returnTemp.ToString();
                }
                catch(Exception err)
                {
                    txtStatus.Text = err.Message.ToString();
                }
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
