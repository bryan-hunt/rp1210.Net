using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

using log4net;

namespace rp1210
{
    /// <summary>
    ///  RP1210A  RP1210_SendCommand Defines
    /// </summary>
    public enum RP1210_Commands
    {
        RP1210_Reset_Device                       =   0,
        RP1210_Set_All_Filters_States_to_Pass     =   3,
        RP1210_Set_Message_Filtering_For_J1939    =   4,
        RP1210_Set_Message_Filtering_For_CAN      =   5,
        RP1210_Set_Message_Filtering_For_J1708    =   7,
        RP1210_Set_Message_Filtering_For_J1850    =   8,
        RP1210_Set_Message_Filtering_For_ISO15765 =   9,
        RP1210_Generic_Driver_Command             =  14,
        RP1210_Set_J1708_Mode                     =  15,
        RP1210_Echo_Transmitted_Messages          =  16,
        RP1210_Set_All_Filters_States_to_Discard  =  17,
        RP1210_Set_Message_Receive                =  18,
        RP1210_Protect_J1939_Address              =  19,
        RP1210_Set_Broadcast_For_J1708            =  20,
        RP1210_Set_Broadcast_For_CAN              =  21,
        RP1210_Set_Broadcast_For_J1939            =  22,
        RP1210_Set_Broadcast_For_J1850            =  23,
        RP1210_Set_J1708_Filter_Type              =  24,
        RP1210_Set_J1939_Filter_Type              =  25,
        RP1210_Set_CAN_Filter_Type                =  26,
        RP1210_Set_J1939_Interpacket_Time         =  27,
        RP1210_SetMaxErrorMsgSize                 =  28,
        RP1210_Disallow_Further_Connections       =  29,
        RP1210_Set_J1850_Filter_Type              =  30,
        RP1210_Release_J1939_Address              =  31,
        RP1210_Set_ISO15765_Filter_Type           =  32,
        RP1210_Set_Broadcast_For_ISO15765         =  33,
        RP1210_Set_ISO15765_Flow_Control          =  34,
        RP1210_Clear_ISO15765_Flow_Control        =  35,
        RP1210_Set_ISO15765_Link_Type             =  36,
        RP1210_Set_J1939_Baud                     =  37,
        RP1210_Set_ISO15765_Baud                  =  38,
        RP1210_Set_BlockTimeout                   = 215,
        RP1210_Set_J1708_Baud                     = 305
    }

    /// <summary>
    /// RP1210A Return Definitions
    /// </summary>
    public enum RP1210_Returns
    {
        NO_ERRORS                                =   0,
        ERR_DLL_NOT_INITIALIZED                  = 128,
        ERR_INVALID_CLIENT_ID                    = 129,
        ERR_CLIENT_ALREADY_CONNECTED             = 130,
        ERR_CLIENT_AREA_FULL                     = 131,
        ERR_FREE_MEMORY                          = 132,
        ERR_NOT_ENOUGH_MEMORY                    = 133,
        ERR_INVALID_DEVICE                       = 134,
        ERR_DEVICE_IN_USE                        = 135,
        ERR_INVALID_PROTOCOL                     = 136,
        ERR_TX_QUEUE_FULL                        = 137,
        ERR_TX_QUEUE_CORRUPT                     = 138,
        ERR_RX_QUEUE_FULL                        = 139,
        ERR_RX_QUEUE_CORRUPT                     = 140,
        ERR_MESSAGE_TOO_LONG                     = 141,
        ERR_HARDWARE_NOT_RESPONDING              = 142,
        ERR_COMMAND_NOT_SUPPORTED                = 143,
        ERR_INVALID_COMMAND                      = 144,
        ERR_TXMESSAGE_STATUS                     = 145,
        ERR_ADDRESS_CLAIM_FAILED                 = 146,
        ERR_CANNOT_SET_PRIORITY                  = 147,
        ERR_CLIENT_DISCONNECTED                  = 148,
        ERR_CONNECT_NOT_ALLOWED                  = 149,
        ERR_CHANGE_MODE_FAILED                   = 150,
        ERR_BUS_OFF                              = 151,
        ERR_COULD_NOT_TX_ADDRESS_CLAIMED         = 152,
        ERR_ADDRESS_LOST                         = 153,
        ERR_CODE_NOT_FOUND                       = 154,
        ERR_BLOCK_NOT_ALLOWED                    = 155,
        ERR_MULTIPLE_CLIENTS_CONNECTED           = 156,
        ERR_ADDRESS_NEVER_CLAIMED                = 157,
        ERR_WINDOW_HANDLE_REQUIRED               = 158,
        ERR_MESSAGE_NOT_SENT                     = 159,
        ERR_MAX_NOTIFY_EXCEEDED                  = 160,
        ERR_MAX_FILTERS_EXCEEDED                 = 161,
        ERR_HARDWARE_STATUS_CHANGE               = 162,
        ERR_INI_FILE_NOT_IN_WIN_DIR              = 202,
        ERR_INI_SECTION_NOT_FOUND                = 204,
        ERR_INI_KEY_NOT_FOUND                    = 205,
        ERR_INVALID_KEY_STRING                   = 206,
        ERR_DEVICE_NOT_SUPPORTED                 = 207,
        ERR_INVALID_PORT_PARAM                   = 208,
        ERR_COMMAND_TIMED_OUT                    = 213,
        ERR_OS_NOT_SUPPORTED                     = 220,
        ERR_COMMAND_QUEUE_IS_FULL                = 222,
        ERR_CANNOT_SET_CAN_BAUDRATE              = 224,
        ERR_CANNOT_CLAIM_BROADCAST_ADDRESS       = 225,
        ERR_OUT_OF_ADDRESS_RESOURCES             = 226,
        ERR_ADDRESS_RELEASE_FAILED               = 227,
        ERR_COMM_DEVICE_IN_USE                   = 230,
        DG_DLL_NOT_FOUND                         = 254,
        DG_BUSY_SENDING                          = 255,
        ERR_DATA_LINK_CONFLICT                   = 441,
        ERR_ADAPTER_NOT_RESPONDING               = 453,
        ERR_CAN_BAUD_SET_NONSTANDARD             = 454,
        ERR_MULTIPLE_CONNECTIONS_NOT_ALLOWED_NOW = 455,
        ERR_J1708_BAUD_SET_NONSTANDARD           = 456,
        ERR_J1939_BAUD_SET_NONSTANDARD           = 457,
        ERR_ISO15765_BAUD_SET_NONSTANDARD        = 458
    }

    public class J1939Message
    {
        public UInt32 TimeStamp { get; set; }
        public Int16 SourceAddress { get; set; }
        public Int16 DestinationAddress { get; set; }
        public byte Priority { get; set; }
        public UInt16 PGN { get; set; }
        public UInt16 dataLength { get; set; }
        public byte[] data { get; set; }
        public J1939Message() { }
        public J1939Message(UInt32 pTimeStamp, Int16 pSource, Int16 pDestination, byte pPriority, UInt16 pPGN, UInt16 pDateLength, byte[] pData)
        {
            TimeStamp = pTimeStamp;
            SourceAddress = pSource;
            DestinationAddress = pDestination;
            Priority = pPriority;
            PGN = pPGN;
            dataLength = pDateLength;
            data = pData;
        }

        public static Byte[] SerializeMessage<T>(T msg) where T : struct
        {
            int objsize = Marshal.SizeOf(typeof(T));
            Byte[] ret = new Byte[objsize];

            IntPtr buff = Marshal.AllocHGlobal(objsize);
            Marshal.StructureToPtr(msg, buff, true);
            Marshal.Copy(buff, ret, 0, objsize);
            Marshal.FreeHGlobal(buff);
            return ret;
        }

        public static T DeserializeMsg<T>(Byte[] data) where T : struct
        {
            int objsize = Marshal.SizeOf(typeof(T));
            IntPtr buff = Marshal.AllocHGlobal(objsize);

            Marshal.Copy(data, 0, buff, objsize);
            T retStruct = (T)Marshal.PtrToStructure(buff, typeof(T));
            Marshal.FreeHGlobal(buff);
            return retStruct;
        }

        public byte[] ToArray()
        {
            byte i = 0;
            byte[] returnValue = new byte[dataLength + 6];

            returnValue[i++] = (byte)(PGN & 0xFF);
            returnValue[i++] = (byte)((PGN >> 8) & 0xFF);
            returnValue[i++] = (byte)((PGN >> 16) & 0xFF);
            returnValue[i++] |= (byte)(Priority);
            returnValue[i++] = (byte)(SourceAddress);
            returnValue[i++] = (byte)(DestinationAddress);
            foreach (byte temp in data)
            {
                returnValue[i++] = temp;
            }
            return returnValue;
        }

    }

    public class J1587Message
    {
        public UInt32 TimeStamp { get; set; }
        public byte Priority { get; set; }
        public byte MID { get; set; }
        public byte PID { get; set; }
        public UInt16 dataLength { get; set; }
        public byte[] data { get; set; }
        public J1587Message() { }
        public byte[] ToArray()
        {
            byte i = 0;
            byte[] returnValue = new byte[dataLength + 2];

            returnValue[i++] = Priority;
            returnValue[i++] = MID;
            returnValue[i++] = PID;
            Array.Copy(data, 0, returnValue, i, dataLength);
            return returnValue;
        }
    }

    public class ProtocolInfo
    {
        public string ProtocolString { get; set; }
        public string ProtocolDescription { get; set; }
        public List<string> ProtocolSpeed { get; set; }
        public string ProtocolParams { get; set; }
        public ProtocolInfo()
        {
            ProtocolSpeed = new List<string>();
        }
    }

    public class DeviceInfo
    {
        public short DeviceId { get; set; }
        public string DeviceDescription { get; set; }
        public string DeviceName { get; set; }
        public string DeviceParams { get; set; }
        public List<ProtocolInfo> SupportedProtocols { get; set; }
        public DeviceInfo()
        {
            SupportedProtocols = new List<ProtocolInfo>();
        }
    }

    public class RP1210BDriverInfo
    {
        readonly string[] CANFormats = { "CAN:Baud=X,SampleLocation=Y,SJW=Z,IDSize=S",
                                         "CAN:Baud=X,PROP_SEG=A,PHASE_SEG1=B,PHASE_SEG2=C,SJW=Z,IDSize=SS",
                                         "CAN:Baud=X,TSEG1=D,TSEG2=E,SampleTimes=Y,SJW=Z,IDSize=SS",
                                         "CAN",
                                         "CAN:Baud=x" };

        readonly string[] J1939Formats = { "J1939:Baud=x",
                                           "J1939",
                                           "J1939:Baud=X,SampleLocation=Y,SJW=Z,IDSize=S",
                                           "J1939:Baud=X,PROP_SEG=A,PHASE_SEG1=B,PHASE_SEG2=C,SJW=Z,IDSize=SS",
                                           "J1939:Baud=X,TSEG1=D,TSEG2=E,SampleTimes=Y,SJW=Z,IDSize=SS" };

        readonly string[] J1708Formats = { "J1708:Baud=x", "J1708" };

        readonly string[] ISO15765Formats = { "ISO15765:Baud=x", 
                                              "ISO15765",
                                              "ISO15765:Baud=X,PROP_SEG=A,PHASE_SEG1=B,PHASE_SEG2=C,SJW=Z,IDSize=SS",
                                              "ISO15765:Baud=X,TSEG1=D,TSEG2=E,SampleTimes=Y,SJW=Z,IDSize=SS" };
        public string DriverVersion;
        public string RP1210Version;
        public string VendorName;
        public int TimestampWeight;
        public List<short> CANFormatsSupported;
        public List<short> J1939FormatsSupported;
        public List<short> J1708FormatsSupported;
        public List<short> ISO15765FormatsSupported;

        public List<DeviceInfo> RP1210Devices;
        public RP1210BDriverInfo()
        {
            RP1210Devices = new List<DeviceInfo>();
        }
    }

    public class RP121032 : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RP121032));

        #region RP1210A Constants
        //--------------------------------------------------------------
        // RP1210A Constants
        //--------------------------------------------------------------
        public const string RP1210_INI = "rp121032.ini";    //if any devices are installed this file should be in the system root

        public const byte BLOCKING_IO = 1;          // For Blocking calls to send/read.
        public const byte NON_BLOCKING_IO = 0;          // For Non-Blocking calls to send/read.

        public const byte CONVERTED_MODE = 1;          // RP1210Mode="Converted"
        public const byte RAW_MODE = 0;          // RP1210Mode="Raw"

        public const uint MAX_J1708_MESSAGE_LENGTH = 508;        // Maximum size a J1708 message can be (+1)
        public const uint MAX_J1939_MESSAGE_LENGTH = 1796;       // Maximum size a J1939 message can be (+1)
        public const uint MAX_J1850_MESSAGE_LENGTH = 508;        // Maximum size a J1850 message can be
        public const uint CAN_DATA_SIZE = 8;          // Maximum data size a CAN message can be

        public const byte ECHO_OFF = 0x00;       // EchoMode
        public const byte ECHO_ON = 0x01;       // EchoMode

        public const byte RECEIVE_ON = 0x01;       // Set Message Receive
        public const byte RECEIVE_OFF = 0x00;       // Set Message Receive

        public const uint FILTER_PGN = 0x00000001; // Setting of J1939 filters
        public const uint FILTER_PRIORITY = 0x00000002; // Setting of J1939 filters
        public const uint FILTER_SOURCE = 0x00000004; // Setting of J1939 filters
        public const uint FILTER_DESTINATION = 0x00000008; // Setting of J1939 filters

        public const byte SILENT_J1939_CLAIM = 0x00;       // Claim J1939 Address
        public const byte PASS_J1939_CLAIM_MESSAGES = 0x01;       // Claim J1939 Address

        public const byte STANDARD_CAN = 0x00;       // Filters
        public const byte EXTENDED_CAN = 0x01;       // Filters

        #endregion

        #region RP1210 External References
        //--------------------------------------------------------------
        // TMC RP1210A Defined Function Prototypes
        //--------------------------------------------------------------

        const string RP1210_CLIENT_CONNECT = "RP1210_ClientConnect";
        const string RP1210_CLIENT_DISCONNECT = "RP1210_ClientDisconnect";
        const string RP1210_GET_ERROR_MSG = "RP1210_GetErrorMsg";
        const string RP1210_GET_HARDWARE_STATUS = "RP1210_GetHardwareStatus";
        const string RP1210_READ_DETAILED_VERSION = "RP1210_ReadDetailedVersion";
        const string RP1210_READ_MESSAGE = "RP1210_ReadMessage";
        const string RP1210_READ_VERSION = "RP1210_ReadVersion";
        const string RP1210_SEND_COMMAND = "RP1210_SendCommand";
        const string RP1210_SEND_MESSAGE = "RP1210_SendMessage";

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private delegate short RP1210ClientConnect(IntPtr hwndClient, short nDeviceId, StringBuilder fpchProtocol, int lSendBuffer, int lReceiveBuffer, short nIsAppPacketizingIncomingMsgs);
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private delegate short RP1210ClientDisconnect(short nClientID);
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        //private delegate short RP1210SendMessage(short nClientID, StringBuilder fpchClientMessage, short nMessageSize, short nNotifyStatusOnTx, short nBlockOnSend);
        private delegate short RP1210SendMessage(short nClientID, byte[] fpchClientMessage, short nMessageSize, short nNotifyStatusOnTx, short nBlockOnSend);
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        //private delegate short RP1210ReadMessage(short nClientID, StringBuilder fpchAPIMessage, short nBufferSize, short nBlockOnSend);
        private delegate short RP1210ReadMessage(short nClientID, byte[] fpchAPIMessage, short nBufferSize, short nBlockOnSend);
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private delegate short RP1210SendCommand(short nCommandNumber, short nClientID, StringBuilder fpchClientCommand, short nMessageSize);
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private delegate void RP1210ReadVersion(StringBuilder fpchDLLMajorVersion, StringBuilder fpchDLLMinorVersion, StringBuilder fpchAPIMajorVersion, StringBuilder fpchAPIMinorVersion);
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private delegate short RP1210ReadDetailedVersion(short nClientID, StringBuilder fpchAPIVersionInfo, StringBuilder fpchDLLVersionInfo, StringBuilder fpchFWVersionInfo);
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private delegate short RP1210GetHardwareStatus(short nClientID, StringBuilder fpchClientInfo, short nInfoSize, short nBlockOnRequest);
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private delegate short RP1210GetErrorMsg(short err_code, StringBuilder fpchMessage); //, short nClientID);

        #endregion

        #region Local Variables
        private bool disposed = false; // Track whether Dispose has been called.
        private short _connectedDevice = -1;
        public short nClientID { get { return _connectedDevice; } }

        #region Bunch of DLL Import Junk
        private class Win32API
        {
            [DllImport("kernel32.dll")]
            public static extern IntPtr LoadLibrary(string dllToLoad);

            [DllImport("kernel32.dll")]
            public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

            [DllImport("kernel32.dll")]
            public static extern bool FreeLibrary(IntPtr hModule);
        }

        private IntPtr pDLL;
        private IntPtr fpRP1210_ClientConnect;
        private IntPtr fpRP1210_ClientDisconnect;
        private IntPtr fpRP1210_SendMessage;
        private IntPtr fpRP1210_ReadMessage;
        private IntPtr fpRP1210_SendCommand;
        private IntPtr fpRP1210_ReadVersion;
        private IntPtr fpRP1210_ReadDetailedVersion;
        private IntPtr fpRP1210_GetHardwareStatus;
        private IntPtr fpRP1210_GetErrorMsg;

        private RP1210ClientConnect pRP1210_ClientConnect;
        private RP1210ClientDisconnect pRP1210_ClientDisconnect;
        private RP1210SendMessage pRP1210_SendMessage;
        private RP1210ReadMessage pRP1210_ReadMessage;
        private RP1210SendCommand pRP1210_SendCommand;
        private RP1210ReadVersion pRP1210_ReadVersion;
        private RP1210ReadDetailedVersion pRP1210_ReadDetailedVersion;
        private RP1210GetHardwareStatus pRP1210_GetHardwareStatus;
        private RP1210GetErrorMsg pRP1210_GetErrorMsg;
        #endregion

        private RP1210BDriverInfo _DriverInfo;
        public RP1210BDriverInfo DriverInfo { get { return _DriverInfo; } }

        public short MaxBufferSize { get; set; }

        #endregion

        #region Constructor
        public RP121032(string NameOfRP1210DLL)
        {
            log.Debug("New Object Instance.");

            string PathToRP1210DLL = Environment.GetEnvironmentVariable("SystemRoot") + "\\System32\\" + NameOfRP1210DLL + ".dll";
            string PathToDeviceINI = Environment.GetEnvironmentVariable("SystemRoot") + "\\" + NameOfRP1210DLL + ".ini";
            pDLL = Win32API.LoadLibrary(PathToRP1210DLL);
            if (pDLL == IntPtr.Zero)   // Error Handling
            {
                log.Warn("Load of " + NameOfRP1210DLL + " DLL Failed.");
            }
            else
            {
                #region More DLL Import Junk
                fpRP1210_ClientConnect = Win32API.GetProcAddress(pDLL, RP1210_CLIENT_CONNECT);
                fpRP1210_ClientDisconnect = Win32API.GetProcAddress(pDLL, RP1210_CLIENT_DISCONNECT);
                fpRP1210_SendMessage = Win32API.GetProcAddress(pDLL, RP1210_SEND_MESSAGE);
                fpRP1210_ReadMessage = Win32API.GetProcAddress(pDLL, RP1210_READ_MESSAGE);
                fpRP1210_SendCommand = Win32API.GetProcAddress(pDLL, RP1210_SEND_COMMAND);
                fpRP1210_ReadVersion = Win32API.GetProcAddress(pDLL, RP1210_READ_VERSION);
                fpRP1210_ReadDetailedVersion = Win32API.GetProcAddress(pDLL, RP1210_READ_DETAILED_VERSION);
                fpRP1210_GetHardwareStatus = Win32API.GetProcAddress(pDLL, RP1210_GET_HARDWARE_STATUS);
                fpRP1210_GetErrorMsg = Win32API.GetProcAddress(pDLL, RP1210_GET_ERROR_MSG);

                pRP1210_ClientConnect = (RP1210ClientConnect)Marshal.GetDelegateForFunctionPointer(fpRP1210_ClientConnect, typeof(RP1210ClientConnect));
                pRP1210_ClientDisconnect = (RP1210ClientDisconnect)Marshal.GetDelegateForFunctionPointer(fpRP1210_ClientDisconnect, typeof(RP1210ClientDisconnect));
                pRP1210_SendMessage = (RP1210SendMessage)Marshal.GetDelegateForFunctionPointer(fpRP1210_SendMessage, typeof(RP1210SendMessage));
                pRP1210_ReadMessage = (RP1210ReadMessage)Marshal.GetDelegateForFunctionPointer(fpRP1210_ReadMessage, typeof(RP1210ReadMessage));
                pRP1210_SendCommand = (RP1210SendCommand)Marshal.GetDelegateForFunctionPointer(fpRP1210_SendCommand, typeof(RP1210SendCommand));
                pRP1210_ReadVersion = (RP1210ReadVersion)Marshal.GetDelegateForFunctionPointer(fpRP1210_ReadVersion, typeof(RP1210ReadVersion));
                pRP1210_ReadDetailedVersion = (RP1210ReadDetailedVersion)Marshal.GetDelegateForFunctionPointer(fpRP1210_ReadDetailedVersion, typeof(RP1210ReadDetailedVersion));
                pRP1210_GetHardwareStatus = (RP1210GetHardwareStatus)Marshal.GetDelegateForFunctionPointer(fpRP1210_GetHardwareStatus, typeof(RP1210GetHardwareStatus));
                pRP1210_GetErrorMsg = (RP1210GetErrorMsg)Marshal.GetDelegateForFunctionPointer(fpRP1210_GetErrorMsg, typeof(RP1210GetErrorMsg));
                #endregion

                _DriverInfo = LoadDeviceParameters(PathToDeviceINI);
            }

            //Default Values
            //MaxBufferSize = 127;
            MaxBufferSize = 255;
        }
        #endregion

        #region RP1210 Library Functions

        /// <summary>
        /// Establishes a logical client connection with the TMC RP1210 API DLL
        /// </summary>
        /// <param name="nDeviceId"></param>
        /// <param name="fpchProtocol"></param>
        /// <param name="lSendBuffer"></param>
        /// <param name="lReceiveBuffer"></param>
        /// <param name="nIsAppPacketizingIncomingMsgs"></param>
        public void RP1210_ClientConnect(short nDeviceId, StringBuilder fpchProtocol, int lSendBuffer, int lReceiveBuffer, short nIsAppPacketizingIncomingMsgs)
        {
            short returnValue = pRP1210_ClientConnect(IntPtr.Zero, nDeviceId, fpchProtocol, lSendBuffer, lReceiveBuffer, nIsAppPacketizingIncomingMsgs);
            if ((returnValue >= 0) && (returnValue <= 127))
            {
                log.Debug("RP1210: ClientConnect Successful: " + returnValue);

                _connectedDevice = returnValue;
            }
            else
            {
                string errorReturn = "ClientConnect Failed: " + RP1210_GetErrorMsg((RP1210_Returns)returnValue);
                log.Warn(errorReturn);
                throw new Exception(errorReturn);
            }
        }

        /// <summary>
        /// Disconnects the logical client application from the TMC RP1210 API DLL
        /// </summary>
        /// <returns></returns>
        public RP1210_Returns RP1210_ClientDisconnect()
        {
            return (RP1210_Returns)pRP1210_ClientDisconnect(_connectedDevice);
        }

        /// <summary>
        /// Sends a message to the API DLL
        /// </summary>
        /// <param name="fpchClientMessage"></param>
        /// <param name="nMessageSize"></param>
        /// <param name="nNotifyStatusOnTx"></param>
        /// <param name="nBlockOnSend"></param>
        /// <returns></returns>
        public RP1210_Returns RP1210_SendMessage(byte[] fpchClientMessage, short nMessageSize, short nNotifyStatusOnTx, short nBlockOnSend)
        {
            RP1210_Returns returnValue;
            if (_connectedDevice >= 0)
            {
                returnValue = (RP1210_Returns)pRP1210_SendMessage(_connectedDevice, fpchClientMessage, nMessageSize, nNotifyStatusOnTx, nBlockOnSend);
                if ((short)returnValue > 127)
                {
                    string errorReturn = "SendMessage Failed: " + RP1210_GetErrorMsg(returnValue);
                    log.Warn(errorReturn);
                    throw new Exception(errorReturn);
                }
            }
            else
            {
                returnValue = RP1210_Returns.ERR_CLIENT_DISCONNECTED;
                throw new Exception("Device Not Connected");
            }
            return returnValue;
        }

        /// <summary>
        /// Reads a message from the API DLL.
        /// </summary>
        /// <param name="nClientID"></param>
        /// <param name="fpchAPIMessage"></param>
        /// <param name="nBufferSize"></param>
        /// <param name="nBlockOnSend"></param>
        /// <returns></returns>
        public byte[] RP1210_ReadMessage(short nBlockOnSend)
        {
            byte[] fpchAPIMessage = new byte[MaxBufferSize];
            byte[] returnMessage;
            RP1210_Returns returnValue = (RP1210_Returns)pRP1210_ReadMessage(_connectedDevice, fpchAPIMessage, MaxBufferSize, nBlockOnSend);
            if ((UInt16)returnValue > MaxBufferSize)
            {
                string errorReturn = "ReadMessage Failed: " + RP1210_GetErrorMsg(returnValue);
                log.Warn(errorReturn);
                throw new Exception(errorReturn);
            }
            else
            {
                returnMessage = new byte[(short)returnValue];
                Array.Copy(fpchAPIMessage, returnMessage, (short)returnValue);
            }
            return returnMessage;
        }

        /// <summary>
        /// Sends a command to the API DLL for things like filtering, etc
        /// </summary>
        /// <param name="nCommandNumber"></param>
        /// <param name="nClientID"></param>
        /// <param name="fpchClientCommand"></param>
        /// <param name="nMessageSize"></param>
        /// <returns></returns>
        public void RP1210_SendCommand(RP1210_Commands nCommandNumber, StringBuilder fpchClientCommand, short nMessageSize)
        {
            short returnValue = pRP1210_SendCommand((short)nCommandNumber, _connectedDevice, fpchClientCommand, nMessageSize);
            if (returnValue != 0)
            {
                string errorReturn = "SendCommand Failed: " + RP1210_GetErrorMsg((RP1210_Returns)returnValue);
                log.Warn(errorReturn);
                throw new Exception(errorReturn);
            }
        }

        /// <summary>
        /// Reads version information from the API, about the API.
        /// </summary>
        /// <returns></returns>
        public string[] RP1210_ReadVersion()
        {
            string[] returnValue = new string[2];
            StringBuilder fpchDLLMajorVersion = new StringBuilder();
            StringBuilder fpchDLLMinorVersion = new StringBuilder();
            StringBuilder fpchAPIMajorVersion = new StringBuilder();
            StringBuilder fpchAPIMinorVersion = new StringBuilder();

            pRP1210_ReadVersion(fpchDLLMajorVersion, fpchDLLMinorVersion, fpchAPIMajorVersion, fpchAPIMinorVersion);
            returnValue[0] = fpchDLLMajorVersion.ToString() + "." + fpchDLLMinorVersion.ToString();
            returnValue[1] = fpchAPIMajorVersion.ToString() + "." + fpchAPIMinorVersion.ToString();

            return returnValue;
        }

        /// <summary>
        /// Newer more comprehensive version information command.  This command is now preferred over the RP1210_ReadVersion call
        /// </summary>
        /// <param name="nClientID"></param>
        /// <param name="fpchAPIVersionInfo"></param>
        /// <param name="fpchDLLVersionInfo"></param>
        /// <param name="fpchFWVersionInfo"></param>
        /// <returns></returns>
        public string[] RP1210_ReadDetailedVersion()
        {
            string[] returnValue = new string[3];
            StringBuilder fpchAPIVersionInfo = new StringBuilder();
            StringBuilder fpchDLLVersionInfo = new StringBuilder();
            StringBuilder fpchFWVersionInfo = new StringBuilder();

            RP1210_Returns functionReturn = (RP1210_Returns)pRP1210_ReadDetailedVersion(_connectedDevice, fpchAPIVersionInfo, fpchDLLVersionInfo, fpchFWVersionInfo);

            if ((short)functionReturn > 127)
            {
                string errorReturn = "ReadDetailedVersion Failed: " + RP1210_GetErrorMsg((RP1210_Returns)functionReturn);
                log.Warn(errorReturn);
                throw new Exception(errorReturn);
            }
            returnValue[0] = fpchDLLVersionInfo.ToString();
            returnValue[1] = fpchAPIVersionInfo.ToString();
            returnValue[2] = fpchFWVersionInfo.ToString();

            return returnValue;
        }

        /// <summary>
        /// Returns information state of the connection and data link
        /// </summary>
        /// <param name="nClientID"></param>
        /// <param name="fpchClientInfo"></param>
        /// <param name="nInfoSize"></param>
        /// <param name="nBlockOnRequest"></param>
        /// <returns></returns>
        public RP1210_Returns RP1210_GetHardwareStatus(StringBuilder fpchClientInfo, short nInfoSize, short nBlockOnRequest)
        {
            return (RP1210_Returns)pRP1210_GetHardwareStatus(_connectedDevice, fpchClientInfo, nInfoSize, nBlockOnRequest);
        }

        /// <summary>
        /// Translates an RP1210 error code into a textual description of the error
        /// </summary>
        /// <param name="err_code"></param>
        /// <param name="fpchMessage"></param>
        /// <param name="nClientID"></param>
        /// <returns></returns>
        public string RP1210_GetErrorMsg(RP1210_Returns err_code)
        {
            StringBuilder fpchMessage = new StringBuilder();
            pRP1210_GetErrorMsg((short)err_code, fpchMessage);
            return fpchMessage.ToString();
        }
        #endregion

        /// <summary>
        /// Looks for installed devices that support the RP1210 API. According to TMC RP1210 the file rp121032.ini should
        /// exist in the %SystemRoot% directory with a list of devices. Those device names should match a cooresponding dll
        /// in the system32 directory. There may also exist an ini file using the same device name.
        /// </summary>
        /// <returns>List&lt;string&gt; of device names that are able to service the RP1210 API</returns>
        public static List<string> ScanForDrivers()
        {
            List<string> returnList;
            string temp = Environment.GetEnvironmentVariable("SystemRoot") + "\\" + RP1210_INI;
            try
            {
                IniParser parser = new IniParser(temp);
                string temp2 = parser.GetSetting("RP1210Support", "APIImplementations");
                returnList = new List<string>(temp2.Split(new char[] { ',' }));
                return returnList;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Loads RP12010 Device Parameters from the driver specific INI file. This file is also located in the 
        /// %SystemRoot% Directory with the rp121032.ini file.
        /// </summary>
        /// <param name="strDeviceIniPath"></param>
        /// <returns></returns>
        public static RP1210BDriverInfo LoadDeviceParameters(string strDeviceIniPath)
        {
            RP1210BDriverInfo driverInfo = new RP1210BDriverInfo();

            IniParser deviceIniParser = new IniParser(strDeviceIniPath);

            try
            {
                driverInfo.DriverVersion = deviceIniParser.GetSetting("VendorInformation", "Version");
            }
            catch
            {
                driverInfo.DriverVersion = "Unknown";
            }

            try
            {
                driverInfo.VendorName = deviceIniParser.GetSetting("VendorInformation", "Name");
            }
            catch
            {
                driverInfo.VendorName = "Unknown";
            }

            try
            {   //Should return A or B. We want B.
                driverInfo.RP1210Version = deviceIniParser.GetSetting("VendorInformation", "RP1210");   
            }
            catch
            {   //Default to B and hope for the best.
                driverInfo.RP1210Version = "B";
            }

            try
            {   //Sets up the common timebase for the device
                driverInfo.TimestampWeight = Convert.ToInt16(deviceIniParser.GetSetting("VendorInformation", "TimestampWeight"));
            }
            catch
            {   //Assume a normal resolution if it is not specified
                driverInfo.TimestampWeight = 1000;
            }

            try
            {
                string strTemp = deviceIniParser.GetSetting("VendorInformation", "CANFormatsSupported");
                driverInfo.CANFormatsSupported = new List<short>(Array.ConvertAll(strTemp.Split(new char[] {','}), x => Convert.ToInt16(x)));
            }
            catch
            {   // RP1210A Defaults
                driverInfo.CANFormatsSupported = new List<short>(new short[] { 4 });
            }

            try
            {   // RP1210A Defaults
                string strTemp = deviceIniParser.GetSetting("VendorInformation", "J1939FormatsSupported");
                driverInfo.J1939FormatsSupported = new List<short>(Array.ConvertAll(strTemp.Split(new char[] { ',' }), x => Convert.ToInt16(x)));
            }
            catch
            {   // RP1210A Defaults
                driverInfo.J1939FormatsSupported = new List<short>(new short[] { 2 });
            }

            try
            {
                string strTemp = deviceIniParser.GetSetting("VendorInformation", "J1708FormatsSupported");
                driverInfo.J1708FormatsSupported = new List<short>(Array.ConvertAll(strTemp.Split(new char[] { ',' }), x => Convert.ToInt16(x)));
            }
            catch
            {   // RP1210A Defaults
                driverInfo.J1708FormatsSupported = new List<short>(new short[] { 2 });
            }

            try
            {
                string strTemp = deviceIniParser.GetSetting("VendorInformation", "ISO15765FormatsSupported");
                driverInfo.CANFormatsSupported = new List<short>(Array.ConvertAll(strTemp.Split(new char[] { ',' }), x => Convert.ToInt16(x)));
            }
            catch
            {   // RP1210A Defaults
                driverInfo.ISO15765FormatsSupported = new List<short>(new short[] { 2 });
            }

            //Comma delimited list of device numbers
            string strDevices = deviceIniParser.GetSetting("VendorInformation", "Devices");
            List<string> listDevices = new List<string>(strDevices.Split(new char[] { ',' }));

            foreach (string temp in listDevices)
            {
                DeviceInfo devTemp = new DeviceInfo();
                devTemp.DeviceId = Convert.ToInt16(deviceIniParser.GetSetting("DeviceInformation" + temp, "DeviceId"));
                devTemp.DeviceDescription = deviceIniParser.GetSetting("DeviceInformation" + temp, "DeviceDescription");
                devTemp.DeviceName = deviceIniParser.GetSetting("DeviceInformation" + temp, "DeviceName");
                devTemp.DeviceParams = deviceIniParser.GetSetting("DeviceInformation" + temp, "DeviceParams");
                driverInfo.RP1210Devices.Add(devTemp);
            }

            //Comma delimited list of supported protocols
            string strProtocols = deviceIniParser.GetSetting("VendorInformation", "Protocols");
            List<string> listProtocols = new List<string>(strProtocols.Split(new char[] { ',' }));

            foreach (string temp in listProtocols)
            {
                ProtocolInfo protoTemp = new ProtocolInfo();
                protoTemp.ProtocolString = deviceIniParser.GetSetting("ProtocolInformation" + temp, "ProtocolString");
                protoTemp.ProtocolDescription = deviceIniParser.GetSetting("ProtocolInformation" + temp, "ProtocolDescription");
                protoTemp.ProtocolParams = deviceIniParser.GetSetting("ProtocolInformation" + temp, "ProtocolParams");
                try //Protocol speed isn't always defined
                {
                    string strProtoSpeeds = deviceIniParser.GetSetting("ProtocolInformation" + temp, "ProtocolSpeed");
                    protoTemp.ProtocolSpeed = new List<string>(strProtoSpeeds.Split(new char[] { ',' }));
                }
                catch
                {
                }

                string strProtoDevs = deviceIniParser.GetSetting("ProtocolInformation" + temp, "Devices");
                List<string> devlistTemp = new List<string>(strProtoDevs.Split(new char[] { ',' }));
                foreach (string devTemp in devlistTemp)
                {
                    driverInfo.RP1210Devices.Find(x => x.DeviceId == Convert.ToInt16(devTemp)).SupportedProtocols.Add(protoTemp);
                }
            }
            return driverInfo;
        }

        public static J1939Message DecodeJ1939Message(byte[] message)
        {
            //  If ECHO_MODE is turned off (default)
            //     TS @ [0-3], PGN @ [4-6], How/Pri @ [7], SRC @ [8], DEST @ [9], Data @ [10-(nLength-10)]
            //  If ECHO_MODE is turned on (NOT the default)
            //     TS @ [0-3], EchoByte @ [4], PGN @ [5-7], How/Pri @ [8], SRC @ [9], DEST @ [10], Data @ [11-(nLength-11)]

            J1939Message decoded = new J1939Message();

            decoded.TimeStamp = (UInt32)((message[0] << 24) + (message[1] << 16) + (message[2] << 8) + message[3]);  //DPA is big endian?
            decoded.PGN = (UInt16)((message[6] << 16) + (message[5] << 8) + message[4]);
            decoded.Priority = message[7];
            decoded.SourceAddress = message[8];
            decoded.DestinationAddress = message[9];
            decoded.dataLength = (UInt16)(message.Length - 10);
            decoded.data = new byte[decoded.dataLength];
            Array.Copy(message, 10, decoded.data, 0, decoded.dataLength);

            return decoded;
        }

        public static byte[] EncodeJ1939Message(J1939Message MessageToEncode)
        {
            byte i = 0;
            byte[] returnValue = new byte[MessageToEncode.dataLength + 6];

            returnValue[i++] = (byte)(MessageToEncode.PGN & 0xFF);
            returnValue[i++] = (byte)((MessageToEncode.PGN >> 8) & 0xFF);
            returnValue[i++] = (byte)((MessageToEncode.PGN >> 16) & 0xFF);
            returnValue[i++] |= (byte)(MessageToEncode.Priority);
            returnValue[i++] = (byte)(MessageToEncode.SourceAddress);
            returnValue[i++] = (byte)(MessageToEncode.DestinationAddress);
            foreach (byte temp in MessageToEncode.data)
            {
                returnValue[i++] = temp;
            }
            return returnValue;
        }

        public static J1587Message DecodeJ1587Message(byte[] message)
        {
            //  If ECHO_MODE is turned on (NOT the default)
            //     TS @ [0-3], EchoByte @ [4], MID @ [5], PID @ [6], Data @ [7-(nLength-7)]
            //  If ECHO_MODE is turned off (default)
            //     TS @ [0-3], MID @ [4], PID @ [5], Data @ [6-(nLength-6)]

            J1587Message decoded = new J1587Message();

            decoded.TimeStamp = (UInt32)((message[0] << 24) + (message[1] << 16) + (message[2] << 8) + message[3]);  //DPA is big endian?
            decoded.MID = message[4];
            decoded.PID = message[5];
            decoded.dataLength = (UInt16)(message.Length - 6);
            decoded.data = new byte[decoded.dataLength];
            Array.Copy(message, 6, decoded.data, 0, decoded.dataLength);

            return decoded;
        }

        public static byte[] EncodeJ1587Message(J1587Message MessageToEncode)
        {
            byte i = 0;
            byte[] returnValue = new byte[MessageToEncode.dataLength + 3];

            returnValue[i++] = MessageToEncode.Priority;
            returnValue[i++] = MessageToEncode.MID;
            returnValue[i++] = MessageToEncode.PID;
            foreach (byte temp in MessageToEncode.data)
            {
                returnValue[i++] = temp;
            }

            return returnValue;
        }

        #region Garbage Collection
        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // Release unmanaged resources.
                try
                {
                    RP1210_SendCommand(RP1210_Commands.RP1210_Reset_Device, new StringBuilder(""), 0);
                    RP1210_ClientDisconnect();
                }
                catch
                {
                }

                bool result = Win32API.FreeLibrary(pDLL);
                if (!result)   //Problem in closing and releasing the resources.
                {
                    //throw
                }
                // Note that this is not thread safe.
                // Another thread could start disposing the object
                // after the managed resources are disposed,
                // but before the disposed flag is set to true.
                // If thread safety is necessary, it must be
                // implemented by the client.
            }
            disposed = true;
        }

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method 
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~RP121032()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose();
        }
        #endregion
    }
}