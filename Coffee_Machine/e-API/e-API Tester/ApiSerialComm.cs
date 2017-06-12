/**
********************************************************************************
  \file ApiSerialComm.cs

  \brief  Modul für die Kommunikation der eAPI
  \author Philippe Sauter

  $Id: ApiSerialComm.cs 248 2016-01-05 07:23:23Z p.sauter $

  © by delisys ag / All rights reserved.

********************************************************************************
**/

using System;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ApiSerialComm
//namespace Eversys.Api.Communication
{
    public class SerialComm
    {
        #region callbacks and events
        // callback delegate
        public delegate void PacketHandler(Packet_t packet);
        public delegate void ByteArrayHandler(byte[] data);

        // event delegate
        public delegate void PacketEvent(object sender, PacketEventArgs e);
        public class PacketEventArgs : EventArgs
        {
            public PacketEventArgs(Packet_t p) { packet = p; isTimeout = false; }
            public PacketEventArgs(bool timeout) { isTimeout = timeout; }
            public Packet_t packet;
            public bool isTimeout = false;
        }

        // callbacks
        private PacketHandler packetSent;
        private PacketHandler packetReceived;
        private PacketHandler ackTimeout;
        private PacketHandler nackFail;
        private PacketHandler responseTimeout;

        private ByteArrayHandler dataSentIntercept;

        // events
        public event PacketEvent ackNackReceived;

        /// <summary>
        /// Called when the Transmission was successfull
        /// </summary>
        /// <param name="handler">method with a parameter Packet_t</param>
        public void RegisterPacketSentHandler(PacketHandler handler)
        {
            this.packetSent = handler;
        }

        /// <summary>
        /// Called when a data or request packet was received.
        /// </summary>
        /// <param name="handler">method with a parameter Packet_t</param>
        public void RegisterPacketReceivedHandler(PacketHandler handler)
        {
            this.packetReceived = handler;
        }

        /// <summary>
        /// Called when the machine didn't answer at all
        /// </summary>
        /// <param name="handler">method with a parameter Packet_t</param>
        public void RegisterAckTimeoutHandler(PacketHandler handler)
        {
            this.ackTimeout = handler;
        }

        /// <summary>
        /// Called when the other machine received the packet but it wasn't correct. (interferences)
        /// </summary>
        /// <param name="handler">method with a parameter Packet_t</param>
        public void RegisterNackFailHandler(PacketHandler handler)
        {
            this.nackFail = handler;
        }

        /// <summary>
        /// Called when an ack but no response was received after a request.
        /// </summary>
        /// <param name="handler">method with a parameter Packet_t</param>
        public void RegisterResponseTimeoutHandler(PacketHandler handler)
        {
            this.responseTimeout = handler;
        }

        /// <summary>
        /// Called everytime a packet was sent.
        /// </summary>
        /// <param name="handler">method with a parameter byte[]</param>
        public void RegisterDataSentInterceptHandler(ByteArrayHandler handler)
        {
            this.dataSentIntercept = handler;
        }

        private void callPacketSentHandler(Packet_t p)
        {
            if (packetSent != null)
                packetSent(p);
        }

        private void callPacketReceivedHandler(Packet_t p)
        {
            if (packetReceived != null)
                packetReceived(p);
        }
        private void callAckTimeoutHandler(Packet_t p)
        {
            if (ackTimeout != null)
                ackTimeout(p);
        }
        private void callNackFailHandler(Packet_t p)
        {
            if (nackFail != null)
                nackFail(p);
        }
        private void callResponseTimeoutHandler(Packet_t p)
        {
            if (responseTimeout != null)
                responseTimeout(p);
        }
        private void callDataSentInterceptHandler(byte[] arr)
        {
            if (dataSentIntercept != null)
                dataSentIntercept(arr);
        }
        #endregion

        #region typedef
        /*  telegram structure: 
         * --------------------------------------------------------------------------------
         *  Byte number:     0 | 1 |2 |3 |4 |5 |6 | 7 | 8 | 9 |10 |10+n  |11+n|12+n|13+n
         *  Byte name:      SOH|PIP|PE|PN|SA|DA|MI|MP1|MP2|DL1|DL2|DATA[]|CRC1|CRC2|EOT
         *  n: data length (n=DL2 + DL1*256)
         *  SOH: Start of heading (0x01)
         *  PIP: currently not used (0x00)
         *  PIE: see typedef (command/response=0x68; request=0x6C; ack=0x6A; nack=0x6B
         *  PN: packet number (incrementing number)
         *  SA: source address ( usually 0x42)
         *  DA: destination address (usually 0x41)
         *  MI: command type (see documentation for more information)
         *  MP1: message parameter (lower byte)
         *  MP2: message parameter (higher byte)
         *  DL1: data length (lower byte)
         *  DL2: data length (higher byte)
         *  DATA[]: Data bytes
         *  CRC1: crc value (lower byte)
         *  CRC2: crc value (higher byte)
         *  EOT: End of text (0x04)
         ***********************************************************************************/

        /// <summary>
        /// Bit 0-2 in the PIE byte.
        /// </summary>
        public enum PacketType_t : byte
        {
            Data_e = 0,
            Reserved_e = 1, // reserved for future use
            PosAck_e = 2,
            NegAck_e = 3,
            Request_e = 4
        }

        /// <summary>
        /// Port ranges and what they are assigned to.
        /// Bit 3-6 in the PIE byte.
        /// </summary>
        public enum ApplicationPort_t : byte
        {
            //ProgrammingProtocol_e = 0x0,    // 0x0 ... 0xB Reserviert für das Programmier-Protokoll
            //ProgrammingMax_e = 0xB,

            //DominoApplication_e = 0xC,      // 0xC ... 0xF, Applikationen
            Api_e = 0xD
            //ApplicationMax_e = 0xF
        }

        /// <summary>
        /// Defines if the packet is encrypted.
        /// Bit 7 in the PIE byte.
        /// </summary>
        public enum Encrypt_t
        {
            No = 0,
            Yes = 1
        }

        /// <summary>
        /// Reserved and special characters
        /// </summary>
        public enum SpecialChars_t : byte
        {
            SOH_e = 0x01,           // Start of Header (Beginn eines Pakets)
            ETB_e = 0x17,           // End of Transmit Block (Ende eines Pakets)
            ShiftChar_e = 0x10,     // Shift Character (Next Char XOR with ShiftXOR)

            NUL_e = 0x00,           // NULL char
            STX_e = 0x02,           // Start of Text
            ETX_e = 0x03,           // End of Text
            EOT_e = 0x04,           // End of Transmission
            LF_e = 0x0A,            // Line Feed
            CR_e = 0x0D,            // Carriage Return
            ModemEsc_e = 0x2B,      // '+' Standard Modem Escape Character

            ShiftXOR_e = 0x40       // Shift XOR Value */
        }

        /// <summary>
        /// Data-header without the data length.
        /// Contains the command id and the 16bit parameter value.
        /// MI, MP[0:7], MP[8:15]
        /// </summary>
        public struct Message_t
        {
            public byte command;
            public ushort parameter;
            public Message_t(byte cmd, ushort param)
            {
                this.command = cmd;
                this.parameter = param;
            }
        }

        /// <summary>
        /// Raw telegram packet with data-array and the data length. (including SOH and EOT)
        /// Shifted character are still shifted.
        /// </summary>
        public struct RawPacket_t
        {
            public byte[] data;
            public int length;
            public RawPacket_t(byte[] data, int length)
            {
                this.data = new byte[length];
                this.length = length;
                Array.Copy(data, this.data, length);
            }
        }

        /// <summary>
        /// CRC value; Byte CRC[0:7] and CRC[8:15]
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct PacketCRC_t
        {
            [FieldOffset(0)]
            public ushort As16BitVal;
            [FieldOffset(0)]
            public byte LowByte;
            [FieldOffset(1)]
            public byte HighByte;
        }

        /// <summary>
        /// Telegram packet without SOH and EOT
        /// isFail doesn't belong to the actual packet. It is used to tag length and crc fails.
        /// </summary>
        public struct Packet_t
        {
            public byte parity;          // set for (number of bits set in header % 3) = 0
            public byte protoVersion;    // 0: initial version, 1: version with ack and nack
            public Encrypt_t isEncrypted;
            public ApplicationPort_t appPort;
            public PacketType_t type;
            public byte sequenceNumber;
            public byte source;
            public byte destination;
            public Message_t message;
            public ushort dataLength;
            public byte[] data;
            public PacketCRC_t CRC;

            public bool isFail;         // true= this packet has an error in it

            /// <summary>
            /// Converts the packet to a byte array.
            /// </summary>
            /// <returns> packet as a byte array. With CRC, without EOT and SOH. Data is not shifted. </returns>
            public byte[] ToByteArray()
            {
                byte[] buffer;
                if (type == PacketType_t.Data_e || type == PacketType_t.Request_e)
                {
                    buffer = new byte[DATAHEADER_OFFSET + PACKETHEADER_OFFSET + dataLength + CRC_SIZE];
                }
                else
                {
                    buffer = new byte[PACKETHEADER_OFFSET];
                }

                buffer[0] = (byte)((parity << 6) | (protoVersion & 0x3F));
                buffer[1] = (byte)(((byte)isEncrypted << 7) | ((byte)appPort << 3) | ((byte)type & 0x7));
                buffer[2] = sequenceNumber;
                buffer[3] = (byte)source;
                buffer[4] = (byte)destination;
                if (type == PacketType_t.Data_e || type == PacketType_t.Request_e)
                {
                    buffer[5] = (byte)message.command;
                    buffer[6] = (byte)message.parameter;
                    buffer[7] = (byte)(message.parameter >> 8);
                    buffer[8] = (byte)dataLength;
                    buffer[9] = (byte)(dataLength >> 8);

                    if (dataLength > 0)
                    {
                        Array.Copy(data, 0, buffer, DATAHEADER_OFFSET + PACKETHEADER_OFFSET, dataLength);
                    }

                    CRC = new PacketCRC_t();
                    CRC.As16BitVal = CalculateCRC(ref buffer, buffer.Length - CRC_SIZE);
                    buffer[DATAHEADER_OFFSET + PACKETHEADER_OFFSET + dataLength] = CRC.LowByte;
                    buffer[DATAHEADER_OFFSET + PACKETHEADER_OFFSET + dataLength + 1] = CRC.HighByte;
                }
                return buffer;
            }

            /// <summary>
            /// Converts this packet to a byte array with shifted chars, SOH and EOT
            /// </summary>
            /// <returns>raw data array</returns>
            public byte[] ToRawArray()
            {
                return ProcessToSend(this.ToByteArray());
            }
        }
        #endregion

        #region properties
        /// <summary>
        /// Address of this device.
        /// </summary>
        public byte Address { get; set; }

        /// <summary>
        /// Maximal timeout until an Ack/Nack is received.
        /// </summary>
        public TimeSpan AckTimeout { get; set; }

        /// <summary>
        /// Maximal timeout until a response is received.
        /// </summary>
        public TimeSpan ResponseTimeout { get; set; }

        /// <summary>
        /// Maximal number of retries until an ack is received, without the initial attempt.
        /// </summary>
        /// <see cref="AckTimeout"/>
        public int MaxSendRepetition { get; set; }

        /// <summary>
        /// How long the thread will be inactive after every execution.
        /// </summary>
        public int ThreadSleepTime { get; set; }

        private int _maxPacketSize;
        /// <summary>
        /// Maximal size of a packet in bytes.
        /// </summary>
        public int MaxPacketSize
        {
            get { return _maxPacketSize; }
            set
            {
                _maxPacketSize = value;
                if (ApiPort != null)
                {
                    ApiPort.WriteBufferSize = 2 * _maxPacketSize;
                    ApiPort.ReadBufferSize = 2 * _maxPacketSize;
                }
                Array.Resize(ref rawDataBuffer, _maxPacketSize);
            }
        }

        private ThreadPriority _transmissionThreadPriority;
        /// <summary>
        /// Priority of the thread which manages the serial communication
        /// </summary>
        public ThreadPriority TransmissionThreadPriority
        {
            get { return _transmissionThreadPriority; }
            set
            {
                _transmissionThreadPriority = value;
                if (TransmissionThread != null)
                    TransmissionThread.Priority = _transmissionThreadPriority;
            }
        }

        private Packet_t _lastSentPacket;
        /// <summary>
        /// Last packet which was given to the EnqueuePacket method
        /// </summary>
        public Packet_t LastSentPacket
        {
            get { return _lastSentPacket; }
        }

        private bool _sendIsIdle = true;
        private bool _receiveIsIdle = true;
        /// <summary>
        /// If the value is true then the port is currently not receiving or sending anything.
        /// This value is used to safely terminate the used thread and close the port.
        /// </summary>
        public bool PortIsIdle
        {
            get
            {
                if (rawDataBufferLength == 0 && _sendIsIdle && _receiveIsIdle)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// True if the port is open.
        /// </summary>
        public bool PortIsOpen
        {
            get { return ApiPort.IsOpen; }
        }

        /// <summary>
        /// True if the port will be changed.
        /// </summary>
        public bool PortIsChanging
        {
            get
            {
                if (newSerialName != "")
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// If this value is true nothing will be sent back.
        /// </summary>
        public bool ReceiveOnlyMode = false;

        private int _baudRate;
        /// <summary>
        /// Baudrate of the serial port.
        /// </summary>
        public int BaudRate
        {
            get { return _baudRate; }
        }

        private string _portName;
        /// <summary>
        /// Name of the serial port.
        /// </summary>
        public string PortName
        {
            get { return _portName; }
        }

        /// <summary>
        /// Sequence number of the last sent packet.
        /// </summary>
        public byte LastSequenceNumber
        {
            get
            {
                if (sequenceNumber == 0)
                    return 255;
                else
                    return (byte)(sequenceNumber - 1);
            }
        }

        #endregion

        #region private variables
        /// <summary>
        /// Sequence number of the last incoming and acknowledged command
        /// </summary>
        private int lastAckCommand = -1;

        /// <summary>
        /// Counts Ack_e timeouts until it reaches MaxSendRepetition
        /// </summary>
        private int ackTimeoutCounter = 0;

        /// <summary>
        /// Counts Nacks until it reaches MaxSendRepetition
        /// </summary>
        private int nackCounter = 0;

        /// <summary>
        /// This number increments after every new command/request; repetitions are with the same number.
        /// </summary>
        private byte sequenceNumber = 0;

        /// <summary>
        /// Flag; set when a request is in the queue and data with the same sequenceNumber was received.
        /// </summary>
        private bool responseReceived = false;

        /// <summary>
        /// Flag; set when a request was sent but the response was not yet received
        /// </summary>
        private bool requestOpen = false;

        /// <summary>
        /// Always set to the time when the last packet was received.
        /// Used to prohibit the sending of a response or Ack/Nack after the timeout time was exceeded.
        /// </summary>
        private DateTime incomingTimeStamp = DateTime.Now;

        /// <summary>
        /// Length of the rawDataBuffer array. (Number of filled fields rather than the array length).
        /// </summary>
        private int rawDataBufferLength = 0;

        /// <summary>
        /// Buffer to save the data which is received on the serial port. Its size equals MaxPacketSize.
        /// </summary>
        private byte[] rawDataBuffer = new byte[512];

        /// <summary>
        /// When this flag is set to true the Serialport will be closed as soon as the ongoing transmission is finished.
        /// </summary>
        private bool stopSerialPort = false;

        /// <summary>
        /// When the ChangePort function is called and the port is currently busy then the new baudrate is stored here.
        /// </summary>
        private int newSerialBaudrate = 0;

        /// <summary>
        /// When the ChangePort function is called and the port is currently busy then the new name is stored here.
        /// </summary>
        private string newSerialName = "";

        /// <summary>
        /// Stop flag for the transmission thread.
        /// </summary>
        private bool stopTransmissionThread = false;
        #endregion

        #region constant values
        /// <summary>
        /// number of bytes in the packet-header(PIP+PIE+PN+SA+DA)
        /// </summary>
        const int PACKETHEADER_OFFSET = 5;
        /// <summary>
        /// number of bytes in the data-header(MI+MP+DL)
        /// </summary>
        const int DATAHEADER_OFFSET = 5;
        /// <summary>
        /// number of bytes in the packet and in the data-header(PIP+PIE+PN+SA+DA+MI+MP+DL)
        /// </summary>
        const int HEADER_OFFSET = 10;

        /// <summary>
        /// initial value for the crc calculation
        /// </summary>
        const ushort CRC_INIT = 0xFFFF;
        /// <summary>
        /// size of the CRC in bytes
        /// </summary>
        const int CRC_SIZE = 2;
        /// <summary>
        /// Protocol version. Should be incremented after every major release.
        /// </summary>
        const byte PROTOCOL_VERSION = 0;
        #endregion

        #region port, thread and queues
        private SerialPort ApiPort;
        private Thread TransmissionThread;
        private Queue<Packet_t> OutgoingQueue = new Queue<Packet_t>();
        private Queue<Packet_t> IncomingDataQueue = new Queue<Packet_t>();
        private Queue<Packet_t> IncomingAckQueue = new Queue<Packet_t>();
        private Queue<RawPacket_t> IncomingRawQueue = new Queue<RawPacket_t>();
        #endregion

        #region CRC polynom
        /// <summary>
        /// LUT for the CRC-16-IBM (x^16 + x^15 + x^2 + 1)
        /// </summary>
        static ushort[] crcPolynomTable = 
        {
            0x0000,0xc0c1,0xc181,0x0140,0xc301,0x03c0,0x0280,0xc241,
            0xc601,0x06c0,0x0780,0xc741,0x0500,0xc5c1,0xc481,0x0440,
            0xcc01,0x0cc0,0x0d80,0xcd41,0x0f00,0xcfc1,0xce81,0x0e40,
            0x0a00,0xcac1,0xcb81,0x0b40,0xc901,0x09c0,0x0880,0xc841,
            0xd801,0x18c0,0x1980,0xd941,0x1b00,0xdbc1,0xda81,0x1a40,
            0x1e00,0xdec1,0xdf81,0x1f40,0xdd01,0x1dc0,0x1c80,0xdc41,
            0x1400,0xd4c1,0xd581,0x1540,0xd701,0x17c0,0x1680,0xd641,
            0xd201,0x12c0,0x1380,0xd341,0x1100,0xd1c1,0xd081,0x1040,
            0xf001,0x30c0,0x3180,0xf141,0x3300,0xf3c1,0xf281,0x3240,
            0x3600,0xf6c1,0xf781,0x3740,0xf501,0x35c0,0x3480,0xf441,
            0x3c00,0xfcc1,0xfd81,0x3d40,0xff01,0x3fc0,0x3e80,0xfe41,
            0xfa01,0x3ac0,0x3b80,0xfb41,0x3900,0xf9c1,0xf881,0x3840,
            0x2800,0xe8c1,0xe981,0x2940,0xeb01,0x2bc0,0x2a80,0xea41,
            0xee01,0x2ec0,0x2f80,0xef41,0x2d00,0xedc1,0xec81,0x2c40,
            0xe401,0x24c0,0x2580,0xe541,0x2700,0xe7c1,0xe681,0x2640,
            0x2200,0xe2c1,0xe381,0x2340,0xe101,0x21c0,0x2080,0xe041,
            0xa001,0x60c0,0x6180,0xa141,0x6300,0xa3c1,0xa281,0x6240,
            0x6600,0xa6c1,0xa781,0x6740,0xa501,0x65c0,0x6480,0xa441,
            0x6c00,0xacc1,0xad81,0x6d40,0xaf01,0x6fc0,0x6e80,0xae41,
            0xaa01,0x6ac0,0x6b80,0xab41,0x6900,0xa9c1,0xa881,0x6840,
            0x7800,0xb8c1,0xb981,0x7940,0xbb01,0x7bc0,0x7a80,0xba41,
            0xbe01,0x7ec0,0x7f80,0xbf41,0x7d00,0xbdc1,0xbc81,0x7c40,
            0xb401,0x74c0,0x7580,0xb541,0x7700,0xb7c1,0xb681,0x7640,
            0x7200,0xb2c1,0xb381,0x7340,0xb101,0x71c0,0x7080,0xb041,
            0x5000,0x90c1,0x9181,0x5140,0x9301,0x53c0,0x5280,0x9241,
            0x9601,0x56c0,0x5780,0x9741,0x5500,0x95c1,0x9481,0x5440,
            0x9c01,0x5cc0,0x5d80,0x9d41,0x5f00,0x9fc1,0x9e81,0x5e40,
            0x5a00,0x9ac1,0x9b81,0x5b40,0x9901,0x59c0,0x5880,0x9841,
            0x8801,0x48c0,0x4980,0x8941,0x4b00,0x8bc1,0x8a81,0x4a40,
            0x4e00,0x8ec1,0x8f81,0x4f40,0x8d01,0x4dc0,0x4c80,0x8c41,
            0x4400,0x84c1,0x8581,0x4540,0x8701,0x47c0,0x4680,0x8641,
            0x8201,0x42c0,0x4380,0x8341,0x4100,0x81c1,0x8081,0x4040
        };
        #endregion

        #region public methods
        /// <summary>
        /// Default values:
        /// ThreadSleepTime = 1ms
        /// MaxPacketSize = 256
        /// SequenceNumber = 0
        /// MaxSendRepetition = 3
        /// Timeouts = 200ms
        /// </summary>
        /// <param name="portName">Name of the port e.g. COM1</param>
        /// <param name="baudRate">Baudrate e.g. 9600</param>
        /// <param name="address">ID of this device (source id)</param>
        public SerialComm(string portName, int baudRate, byte address)
        {
            Address = address;
            AckTimeout = TimeSpan.FromMilliseconds(200);
            ResponseTimeout = TimeSpan.FromMilliseconds(200);
            MaxSendRepetition = 3;
            ThreadSleepTime = 1;
            MaxPacketSize = 256;
            sequenceNumber = 0;
            TransmissionThreadPriority = ThreadPriority.Highest;

            _baudRate = baudRate;
            _portName = portName;

            ApiPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
        }

        /// <summary>
        /// Opens the port and starts the thread.
        /// </summary>
        public void Start()
        {
            bool success = false;
            uint tries = 0;
            if (!ApiPort.IsOpen)
            {
                while (success == false && tries < 10)
                {
                    try
                    {
                        ApiPort.Open();
                        success = true;
                    }
                    catch { tries++; }
                    stopSerialPort = false;
                }
            }

            if (TransmissionThread == null)
            {
                TransmissionThread = new Thread(new ThreadStart(TransmissionThreadWorker));
                TransmissionThread.Priority = TransmissionThreadPriority;
                TransmissionThread.Start();
            }

            ApiPort.DataReceived += DataReceivedHandler;
        }

        /// <summary>
        /// Only starts the thread.
        /// </summary>
        public void StartThread()
        {
            if (TransmissionThread == null)
            {
                TransmissionThread = new Thread(new ThreadStart(TransmissionThreadWorker));
                TransmissionThread.Priority = TransmissionThreadPriority;
                TransmissionThread.Start();
            }
            stopSerialPort = false;
        }


        [Obsolete("Close() is deprecated, please use Pause() instead.")]
        public void Close()
        {
            Pause();
        }

        /// <summary>
        /// Closes the Port but does not terminate the thread.
        /// </summary>
        public void Pause()
        {
            stopSerialPort = true;
            if (PortIsIdle && PortIsOpen)
            {
                try
                {
                    ApiPort.Close();
                    _sendIsIdle = true;
                    _receiveIsIdle = true;
                }
                catch { }
            }
        }

        /// <summary>
        /// Initiates a port change.
        /// </summary>
        /// <param name="portname">new port name</param>
        /// <param name="baudrate">new baudrate</param>
        public void ChangePort(string portname, int baudrate)
        {
            if (!ApiPort.IsOpen)
            {
                ApiPort.BaudRate = baudrate;
                _baudRate = baudrate;
                ApiPort.PortName = portname;
                _portName = portname;
            }
            else
            {
                Pause();
                newSerialBaudrate = baudrate;
                newSerialName = portname;
            }
        }

        #region EnqueuePacket overload
        /// <summary>
        /// Normaly this should not be used. Used to manually send an ack/nack or
        /// to send a response with an undef command (unknown request)
        /// data = null
        /// data length = 0
        /// isRequest = false
        /// parity = 0
        /// Encrypted = no
        /// Application Port = Api_e
        /// </summary>
        /// <param name="destination"> destination of this packet as byte </param>
        /// <param name="sequenzNr"> Sequence number which will be transmitted. Should only be set manually in a response. </param>
        /// <param name="command"> Type of the Command see byte. used in the Message_t struct </param>
        public void EnqueuePacket(byte destination, byte command, byte sequenzNr)
        {
            EnqueuePacket(destination, new Message_t(command, 0), null, 0, false, Encrypt_t.No, 0, ApplicationPort_t.Api_e, sequenzNr);
        }

        /// <summary>
        /// Used to send a command.
        /// isRequest = false
        /// sequenceNumber = last Number + 1
        /// parity = 0
        /// Encrypted = no
        /// Application Port = Api_e
        /// </summary>
        /// <param name="destination"> destination of this packet as byte </param>
        /// <param name="message"> data-header w/o data length see Message_t </param>
        /// <param name="data"> data as a byte array. The size of this array should equal dataLength </param>
        /// <param name="dataLength"> how many data bytes will be sent </param>
        public void EnqueuePacket(byte destination, Message_t message, byte[] data, ushort dataLength)
        {
            EnqueuePacket(destination, message, data, dataLength, false, Encrypt_t.No, 0, ApplicationPort_t.Api_e, NextSequenceNumber());
        }

        /// <summary>
        /// Used to send a request.
        /// sequenceNumber = last Number + 1
        /// parity = 0
        /// Encrypted = no
        /// Application Port = Api_e
        /// </summary>
        /// <param name="destination"> destination of this packet as byte </param>
        /// <param name="message"> data-header w/o data length see Message_t </param>
        /// <param name="data"> data as a byte array. The size of this array should equal dataLength </param>
        /// <param name="dataLength"> how many data bytes will be sent </param>
        /// <param name="request"> true when this packet is a request, else false. </param>
        public void EnqueuePacket(byte destination, Message_t message, byte[] data, ushort dataLength, bool request)
        {
            EnqueuePacket(destination, message, data, dataLength, request, Encrypt_t.No, 0, ApplicationPort_t.Api_e, NextSequenceNumber());
        }

        /// <summary>
        /// Used to send a response.
        /// isRequest = false
        /// parity = 0
        /// Encrypted = no
        /// Application Port = Api_e
        /// </summary>
        /// <param name="destination"> destination of this packet as byte </param>
        /// <param name="message"> data-header w/o data length see Message_t </param>
        /// <param name="data"> data as a byte array. The size of this array should equal dataLength </param>
        /// <param name="dataLength"> how many data bytes will be sent </param>
        /// <param name="sequenzNr"> Sequence number which will be transmitted. Should only be set manually in a response. </param>
        public void EnqueuePacket(byte destination, Message_t message, byte[] data, ushort dataLength, byte sequenzNr)
        {
            EnqueuePacket(destination, message, data, dataLength, false, Encrypt_t.No, 0, ApplicationPort_t.Api_e, sequenzNr);
        }

        /// <summary>
        /// Used to repeat a packet or to sent a custom packet.
        /// </summary>
        /// <param name="p"> Packet_t which will be sent </param>
        public void EnqueuePacket(Packet_t p)
        {
            bool request;
            if (p.type == PacketType_t.Request_e)
                request = true;
            else
                request = false;

            EnqueuePacket(p.destination, p.message, p.data, p.dataLength, request, p.isEncrypted, p.parity, p.appPort, p.sequenceNumber);
        }
        #endregion

        /// <summary>
        /// Adds a packet to the send queue.
        /// </summary>
        /// <param name="destination"> destination of this packet as byte </param>
        /// <param name="message"> data-header w/o data length see Message_t </param>
        /// <param name="data"> data as a byte array. The size of this array should equal dataLength </param>
        /// <param name="dataLength"> how many data bytes will be sent </param>
        /// <param name="request"> true when this packet is a request, else false. </param>
        /// <param name="sequenzNr"> Sequence number which will be transmitted. Should only be set manually in a response. </param>
        /// <param name="appPort"> Application port which will be sent in the packet </param>
        /// <param name="encrypted"> Is the Packet Encrypted </param>
        /// <param name="parity"> Which parity does the packet have </param>
        public void EnqueuePacket(byte destination, Message_t message, byte[] data, ushort dataLength, bool request, Encrypt_t encrypted, byte parity, ApplicationPort_t appPort, byte sequenzNr)
        {
            // Prepare a new packet
            Packet_t packet = new Packet_t();
            packet.parity = parity;
            packet.protoVersion = PROTOCOL_VERSION;
            packet.isEncrypted = encrypted;
            packet.appPort = appPort;
            packet.sequenceNumber = sequenzNr;
            packet.source = Address;
            packet.destination = destination;
            packet.message = message;
            packet.dataLength = dataLength;
            packet.data = data;
            if (request == true)
                packet.type = PacketType_t.Request_e;
            else
                packet.type = PacketType_t.Data_e;
            packet.isFail = false;

            packet.ToByteArray();
            _lastSentPacket = packet;
            OutgoingQueue.Enqueue(packet);
        }

        /// <summary>
        /// This method is an alternate receive option.
        /// </summary>
        /// <param name="data">raw data</param>
        /// <param name="length">length of the raw data</param>
        /// <returns>true = successful, false = error occured</returns>
        public bool addRawData(byte[] data, int length)
        {
            bool success = false;
            try
            {
                if ((rawDataBufferLength + length) < MaxPacketSize)
                {
                    ProcessRawData(data, length);
                    success = true;
                }
            }
            catch { }
            return success;
        }

        /// <summary>
        /// Reverts the shifted characters.
        /// </summary>
        /// <param name="data">raw data</param>
        /// <param name="length">length of the raw data</param>
        /// <returns>data array without shifted characters</returns>
        public static byte[] RevertShiftedChars(byte[] data, int length)
        {
            int newLength = 0;
            byte[] newData = new byte[length];

            for (int i = 0; length > i; i++)
            {
                if (data[i] == (byte)SpecialChars_t.ShiftChar_e)
                {
                    i++;
                    newData[newLength] = (byte)(data[i] ^ (byte)SpecialChars_t.ShiftXOR_e);
                    newLength++;
                }
                else
                {
                    newData[newLength] = data[i];
                    newLength++;
                }
            }
            Array.Resize(ref newData, newLength);

            return newData;
        }

        #region IDisposable Member
        /// <summary>
        /// Aborts the thread and closes the port.
        /// </summary>
        public void Dispose()
        {
            if (TransmissionThread != null)
            {
                stopTransmissionThread = true;
                if (TransmissionThread.Join(200) == false)
                    TransmissionThread.Abort();
            }
            if (ApiPort != null)
            {
                ApiPort.Close();
            }
        }

        #endregion
        #endregion

        #region private methods
        /// <summary>
        /// Increments the sequence number and returns it.
        /// </summary>
        /// <returns>Next valid sequence number.</returns>
        private byte NextSequenceNumber()
        {
            return sequenceNumber++;
        }

        #region transmission state machine
        /// <summary>
        /// States of the state-machine in TransmissionThreadWorker()
        /// </summary>
        private enum SendStates
        {
            Idle,
            Sending,
            WaitingForAck,
            WaitingForResponse
        }

        private SendStates ThreadState = SendStates.Idle;

        /// <summary>
        /// Reads data from the serial ports and makes packets from the data.
        /// Writes data to the serial port and manages the ack/nack.
        /// </summary>
        private void TransmissionThreadWorker()
        {
            DateTime sent_timestamp = DateTime.Now;

            while (stopTransmissionThread == false)
            {
                try
                {
                    ProccessRawPacket();
                    IncomingDataQueueHandler();

                    switch (ThreadState)
                    {
                        #region State Idle
                        case SendStates.Idle:
                            if (OutgoingQueue.Count > 0)
                            {
                                ThreadState = SendStates.Sending;
                            }
                            break;
                        #endregion

                        #region State Sending
                        case SendStates.Sending:
                            sent_timestamp = SendState(sent_timestamp);
                            break;
                        #endregion

                        #region State WaitingForAck
                        case SendStates.WaitingForAck:
                            WaitingForAckState(sent_timestamp);
                            break;
                        #endregion

                        #region State WaitingForResponse
                        case SendStates.WaitingForResponse:
                            WaitingForResponseState(sent_timestamp);
                            break;
                        #endregion
                    }
                }
                catch (ThreadAbortException)
                {
                    break;
                }

                //save shutdown and portswitch
                if (PortIsIdle && stopSerialPort)
                {
                    ApiPort.Close();
                    _sendIsIdle = true;
                    _receiveIsIdle = true;

                    if (newSerialBaudrate != 0)
                    {
                        ApiPort.BaudRate = newSerialBaudrate;
                        _baudRate = newSerialBaudrate;
                        ApiPort.PortName = newSerialName;

                        _portName = newSerialName;
                        newSerialName = "";
                        newSerialBaudrate = 0;
                        Start();
                    }
                }

                Thread.Sleep(ThreadSleepTime);
            }
        }

        /// <summary>
        /// Checks if there is something to send and sends it.
        /// </summary>
        private DateTime SendState(DateTime sent_timestamp)
        {
            lock (OutgoingQueue)
            {
                if (OutgoingQueue.Count > 0 && _sendIsIdle && stopSerialPort == false)
                {
                    if (requestOpen == true && DateTime.Now.Subtract(incomingTimeStamp) > ResponseTimeout)
                    {
                        // if this is a response and ResponseTimeout was exceeded, then ignore the packet
                        OutgoingQueue.Dequeue();
                        requestOpen = false;
                        ThreadState = SendStates.Idle;
                        return sent_timestamp;
                    }

                    if (checkForReset(OutgoingQueue.Peek(), true))
                    {
                        lastAckCommand = -1;
                        sequenceNumber = 0;
                    }
                    WriteToPort(OutgoingQueue.Peek().ToByteArray());

                    if (OutgoingQueue.Peek().type == PacketType_t.Data_e || OutgoingQueue.Peek().type == PacketType_t.Request_e)
                    {
                        sent_timestamp = DateTime.Now;
                        if (requestOpen == true)
                        {
                            requestOpen = false;
                            ThreadState = SendStates.Idle;
                            callPacketSentHandler(OutgoingQueue.Dequeue());
                        }
                        else
                            ThreadState = SendStates.WaitingForAck;
                        return sent_timestamp;
                    }
                    else
                        OutgoingQueue.Dequeue();
                }
                ThreadState = SendStates.Idle;
            }

            return sent_timestamp;
        }

        /// <summary>
        /// Checks if an ack or a nack was received or if a ackTimeout occured.
        /// </summary>
        private void WaitingForAckState(DateTime sent_timestamp)
        {
            lock (OutgoingQueue)
            {
                lock (IncomingAckQueue)
                {
                    if (IncomingAckQueue.Count > 0)
                    {
                        Packet_t packet = IncomingAckQueue.Dequeue();
                        if (packet.sequenceNumber != OutgoingQueue.Peek().sequenceNumber)
                        {
                            // sequence number doesn't equal the last commands sequence number
                        }
                        else
                        {

                            switch (packet.type)
                            {
                                case PacketType_t.PosAck_e:
                                    ackTimeoutCounter = 0;
                                    nackCounter = 0;
                                    if (OutgoingQueue.Peek().type == PacketType_t.Request_e)
                                    {
                                        callPacketSentHandler(OutgoingQueue.Peek());
                                        ThreadState = SendStates.WaitingForResponse;
                                    }
                                    else
                                    {
                                        callPacketSentHandler(OutgoingQueue.Dequeue());
                                        ThreadState = SendStates.Idle;
                                    }
                                    break;

                                case PacketType_t.NegAck_e:
                                    ackTimeoutCounter = 0;
                                    if (++nackCounter > MaxSendRepetition)
                                    {
                                        nackCounter = 0;
                                        callNackFailHandler(OutgoingQueue.Dequeue());
                                        ThreadState = SendStates.Idle;
                                    }
                                    else
                                        ThreadState = SendStates.Sending;
                                    break;

                                default:
                                    break;
                            }
                        }
                    }

                    // ack timeout?
                    else if (DateTime.Now.Subtract(sent_timestamp) > AckTimeout)
                    {
                        if (ackNackReceived != null)
                        {
                            ackNackReceived(this, new PacketEventArgs(true));
                        }

                        if (++ackTimeoutCounter > MaxSendRepetition)
                        {
                            ackTimeoutCounter = 0;
                            nackCounter = 0;
                            callAckTimeoutHandler(OutgoingQueue.Dequeue());
                            ThreadState = SendStates.Idle;
                        }
                        else
                            ThreadState = SendStates.Sending;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if a response was received
        /// </summary>
        private void WaitingForResponseState(DateTime sent_timestamp)
        {
            if (responseReceived == true)
            {
                responseReceived = false;
                lock (OutgoingQueue)
                {
                    OutgoingQueue.Dequeue();
                }
                ThreadState = SendStates.Idle;
            }
            else if (DateTime.Now.Subtract(sent_timestamp) > ResponseTimeout)
            {
                lock (OutgoingQueue)
                {
                    callResponseTimeoutHandler(OutgoingQueue.Dequeue());
                }
                ThreadState = SendStates.Idle;
            }
        }
        #endregion

        /// <summary>
        /// This method takes the raw packets and processes them.
        /// Then it puts the packet in the DataQueue or in the AckQueue
        /// </summary>
        /// <param name="data">data from serial port</param>
        /// <param name="length">number of bytes of data</param>
        private void ProccessRawPacket()
        {
            if (IncomingRawQueue.Count > 0)
            {
                int index = 0;
                Packet_t packet = new Packet_t();
                packet.isFail = false;
                RawPacket_t rawPacket = new RawPacket_t(new byte[MaxPacketSize], MaxPacketSize);
                lock (IncomingRawQueue)
                {
                    rawPacket = IncomingRawQueue.Dequeue();
                }
                // delete SOT and EOT
                Array.Copy(rawPacket.data, 1, rawPacket.data, 0, rawPacket.length - 2);
                rawPacket.length -= 2;

                // revert shifted characters
                byte[] data = RevertShiftedChars(rawPacket.data, rawPacket.length);
                int length = data.Length;

                if (length < 5)
                {
                    // length fail -> NACK
                    packet.isFail = true;

                }

                // cast the rawPacket into a packet
                packet.parity = (byte)(data[0] >> 6);
                packet.protoVersion = (byte)(data[0] & 0x3F);
                packet.isEncrypted = (Encrypt_t)(data[1] >> 7);
                packet.appPort = (ApplicationPort_t)((data[1] >> 3) & 0xF);
                packet.type = (PacketType_t)(data[1] & 0x7);
                packet.sequenceNumber = data[2];
                packet.source = (byte)data[3];
                packet.destination = (byte)data[4];

                switch (packet.type)
                {
                    case PacketType_t.Data_e:
                    case PacketType_t.Request_e:
                        // cast command id and parameter
                        packet.message = new Message_t(
                            (byte)data[5],
                            (ushort)(data[6] | (data[7] << 8))
                            );

                        // cast data
                        packet.dataLength = (ushort)(data[8] | ((int)data[9] << 8));

                        if (packet.dataLength > 0 && length < MaxPacketSize)
                        {
                            packet.data = new byte[packet.dataLength];
                            while (index < packet.dataLength)
                            {
                                packet.data[index] = data[HEADER_OFFSET + index];
                                index++;
                            }
                        }

                        // cast crc
                        packet.CRC.LowByte = data[HEADER_OFFSET + index];
                        packet.CRC.HighByte = data[HEADER_OFFSET + index + 1];

                        // check crc
                        if (CalculateCRC(ref data, length) != 0)
                        {
                            // CRC fail -> NACK
                            packet.isFail = true;
                        }

                        lock (IncomingDataQueue)
                        {
                            IncomingDataQueue.Enqueue(packet);
                        }

                        break;
                    case PacketType_t.PosAck_e:
                    case PacketType_t.NegAck_e:
                        if (ackNackReceived != null)
                        {
                            ackNackReceived(this, new PacketEventArgs(packet));
                        }
                        lock (IncomingAckQueue)
                        {
                            IncomingAckQueue.Enqueue(packet);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Checks the GetStatus response for the justReset bit. If it is set
        /// the sequence number will be set to 0.
        /// </summary>
        /// <param name="p"></param>
        private bool checkForReset(Packet_t p, bool outgoing)
        {
            // commandID = GetStatus (0x01) and justReset bit is set
            if (p.message.command == 0x01 && p.type == PacketType_t.Data_e)
            {
                if ((p.data[0] & 0x01) == 0x01)
                {
                    if (outgoing)
                    {
                        return true;
                    }
                    else
                    {
                        if (OutgoingQueue.Peek().sequenceNumber == p.sequenceNumber)
                            return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Sends the ack/nack packet, checks if the Data_e packet was a response (no ack/nack needed)
        /// and calls the packetReceived callback
        /// </summary>
        private void IncomingDataQueueHandler()
        {
            lock (IncomingDataQueue)
            {
                if (IncomingDataQueue.Count > 0)
                {
                    if (IncomingDataQueue.Peek().destination == this.Address)
                    {
                        if (OutgoingQueue.Count == 0 || OutgoingQueue.Peek().type == PacketType_t.Data_e)
                        {   // if there is no request in the queue
                            Packet_t ackPacket = new Packet_t();
                            ackPacket.parity = 0;
                            ackPacket.protoVersion = PROTOCOL_VERSION;
                            ackPacket.isEncrypted = 0;
                            ackPacket.appPort = ApplicationPort_t.Api_e;
                            ackPacket.sequenceNumber = IncomingDataQueue.Peek().sequenceNumber;
                            ackPacket.source = this.Address;
                            ackPacket.destination = IncomingDataQueue.Peek().source;
                            ackPacket.dataLength = 0;
                            ackPacket.data = new byte[0];

                            if (DateTime.Now.Subtract(incomingTimeStamp) < AckTimeout)
                            {
                                if (IncomingDataQueue.Peek().isFail == false)
                                {
                                    ackPacket.type = PacketType_t.PosAck_e;
                                    _sendIsIdle = false;
                                    WriteToPort(ackPacket.ToByteArray());
                                    _sendIsIdle = true;

                                    if (IncomingDataQueue.Peek().type == PacketType_t.Request_e)
                                        requestOpen = true;

                                    // if the sequence Number of the last received packet is
                                    // the same as this then throw the packet away
                                    if (lastAckCommand != (int)IncomingDataQueue.Peek().sequenceNumber)
                                        callPacketReceivedHandler(IncomingDataQueue.Dequeue());
                                    else
                                        IncomingDataQueue.Dequeue();

                                    lastAckCommand = (int)ackPacket.sequenceNumber;
                                }
                                else
                                {
                                    ackPacket.type = PacketType_t.NegAck_e;
                                    _sendIsIdle = false;
                                    WriteToPort(ackPacket.ToByteArray());
                                    _sendIsIdle = true;
                                    IncomingDataQueue.Dequeue();
                                }
                            }
                        }
                        else // the data packet is a response
                        {
                            if (checkForReset(IncomingDataQueue.Peek(), false))
                            {
                                lastAckCommand = -1;
                                sequenceNumber = 0;
                            }

                            responseReceived = true;
                            callPacketReceivedHandler(IncomingDataQueue.Dequeue());
                        }
                    }
                    else
                    {
                        IncomingDataQueue.Dequeue();
                    }
                }
            }
        }

        /// <summary>
        /// Writes the packet to the serial port.
        /// </summary>
        /// <param name="data">data from ToByteArray()</param>
        private void WriteToPort(byte[] data)
        {
            _sendIsIdle = false;
            byte[] buffer = ProcessToSend(data);

            if (ReceiveOnlyMode == false)
            {
                if (ApiPort != null && ApiPort.IsOpen)
                {
                    ApiPort.Write(buffer, 0, buffer.Length);
                }

                callDataSentInterceptHandler(buffer);
            }
            _sendIsIdle = true;
        }

        /// <summary>
        /// Makes the Data ready to be sent to the port.
        /// </summary>
        /// <param name="data">data from ToByteArray()</param>
        /// <returns>final byte array with SOH and EOT</returns>
        private static byte[] ProcessToSend(byte[] data)
        {
            int count = 0;
            byte[] buffer = new byte[data.Length * 2 + 2]; // Length*2 + 2 = maximum possible length (every byte shifted)
            buffer[count++] = (byte)SpecialChars_t.SOH_e;
            for (int i = 0; i < data.Length; i++)
            {   // shift every special character
                if (IsSpecialChar(data[i]))
                {
                    buffer[count++] = (byte)SpecialChars_t.ShiftChar_e;
                    buffer[count++] = (byte)(data[i] ^ (byte)SpecialChars_t.ShiftXOR_e);
                }
                else
                {
                    buffer[count++] = data[i];
                }
            }
            buffer[count++] = (byte)SpecialChars_t.EOT_e;
            Array.Resize(ref buffer, count);
            return buffer;
        }

        /// <summary>
        /// Event handler for the serialport.DataReceived event.
        /// It reads everything from the serialport and calls the ProcessRawData function.
        /// </summary>
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                _receiveIsIdle = false;
                SerialPort sp = (SerialPort)sender;
                byte[] buffer = new byte[sp.ReadBufferSize];
                int bytesRead = sp.Read(buffer, 0, buffer.Length);
                ProcessRawData(buffer, bytesRead);
                _receiveIsIdle = true;
            }
            catch { }
        }


        /// <summary>
        /// This method searches for a SOH and it will save the data after it.
        /// Then it waits until it finds a EOT. If the MaxPacketSize was not exceeded,
        /// then it will save the data with the SOH and EOT as a rawPacket in the IncomingRawQueue.
        /// If the packet size grows over MaxPacketSize it will ignore the packet.
        /// If there is still data after the EOT it will recursively call itself until there is no data left.
        /// </summary>
        /// <param name="buffer">data as byte array</param>
        /// <param name="bytesRead">number of data bytes</param>
        private void ProcessRawData(byte[] buffer, int bytesRead)
        {
            int indexOfSoh = -1;
            int indexOfEot = -1;
            int bytesToProcess = bytesRead;

            // no SOH in the rawDataBuffer?
            if (rawDataBuffer[0] != (byte)SpecialChars_t.SOH_e)
            {
                indexOfSoh = Array.IndexOf(buffer, (byte)SpecialChars_t.SOH_e);
                if (indexOfSoh >= 0)
                {
                    Array.Copy(buffer, indexOfSoh, buffer, 0, bytesRead - indexOfSoh);
                    bytesRead = bytesToProcess = bytesRead - indexOfSoh;
                }
            }

            if (rawDataBuffer[0] == (byte)SpecialChars_t.SOH_e || indexOfSoh >= 0)
            {
                indexOfEot = Array.IndexOf(buffer, (byte)SpecialChars_t.EOT_e);
                if (indexOfEot >= 0)
                {
                    bytesToProcess = indexOfEot + 1;
                    // would MaxPacketSize be exceeded?
                    if (bytesToProcess + rawDataBufferLength <= MaxPacketSize)
                    {
                        // create a RawPacket and add it to the IncomingRawQueue
                        RawPacket_t rawPacket = new RawPacket_t(new byte[bytesToProcess + rawDataBufferLength],
                                                                bytesToProcess + rawDataBufferLength);
                        Array.Copy(rawDataBuffer, 0, rawPacket.data, 0, rawDataBufferLength);
                        Array.Copy(buffer, 0, rawPacket.data, rawDataBufferLength, bytesToProcess);
                        lock (IncomingRawQueue)
                        {
                            if (rawPacket.length >= 10)
                                rawDataBufferLength = 0;
                            IncomingRawQueue.Enqueue(rawPacket);
                        }
                        incomingTimeStamp = DateTime.Now;

                        // reset rawDataBuffer
                        rawDataBuffer = new byte[MaxPacketSize];
                        rawDataBufferLength = 0;
                    }
                    else
                    {
                        // MaxPacketSize would be exceeded, then ignore the packet and reset rawDataBuffer
                        rawDataBuffer = new byte[MaxPacketSize];
                        rawDataBufferLength = 0;
                    }

                    if (bytesToProcess < bytesRead)
                    {
                        // call this method recursively
                        Array.Copy(buffer, bytesToProcess, buffer, 0, bytesRead - bytesToProcess);
                        Array.Resize(ref buffer, bytesRead - bytesToProcess);
                        ProcessRawData(buffer, bytesRead - bytesToProcess);
                    }

                }
                else
                {
                    // would MaxPacketSize be exceeded?
                    if (bytesRead + rawDataBufferLength <= MaxPacketSize)
                    {
                        Array.Copy(buffer, 0, rawDataBuffer, rawDataBufferLength, bytesRead);
                        rawDataBufferLength += bytesRead;
                    }
                    else
                    {
                        rawDataBuffer = new byte[MaxPacketSize];
                        rawDataBufferLength = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Checks for special character (shifted characters)
        /// </summary>
        /// <param name="c">to be checked character</param>
        /// <returns>true/false</returns>
        private static bool IsSpecialChar(byte c)
        {
            return (bool)(c <= (byte)SpecialChars_t.EOT_e ||
                            c == (byte)SpecialChars_t.ETB_e ||
                            c == (byte)SpecialChars_t.LF_e ||
                            c == (byte)SpecialChars_t.CR_e ||
                            c == (byte)SpecialChars_t.ShiftChar_e);
        }

        /// <summary>
        /// calculates the crc value with the crcPolynomTable
        /// </summary>
        /// <param name="data_p">reference to a byte array. the CRC will be calculated on this array.</param>
        /// <param name="dataLength"> length of the array </param>
        /// <returns> crc value as 16bit value </returns>
        private static ushort CalculateCRC(ref byte[] data_p, int dataLength)
        {
            ushort checksum;

            checksum = CRC_INIT;

            for (int nIndex = 0; nIndex < dataLength; nIndex++)
            {
                checksum = (ushort)((checksum >> 8) ^ crcPolynomTable[(checksum ^ data_p[nIndex]) & 0xFF]);
            }

            return checksum;
        }
        #endregion
    }
}