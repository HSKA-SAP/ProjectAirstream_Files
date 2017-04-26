using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;


namespace ProjectAirstream
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<SerialPort> openPorts = openSerialPorts();

            while(true){
                Console.Write("\n >> Send from Pi on port " + openPorts[0].PortName.ToString() + "....\n");

                string input = Console.ReadLine();
                if (input == "exit")
                {
                    break;

                }
                WriteToPi(input, openPorts);
                Console.Write("\n >> Read from Coffee Machine on port " + openPorts[1].PortName.ToString() + "....\n");
                string output = ReadFromCoffee(openPorts);
                Console.Write(output + "\n");


                //Do rinse example
                DoRinse();


                //Console.Write("\n");
                //byte[] someData = { 0, 1, 2 };
                //byte[] outputData = new byte[100];
                ////outputData = RawSerialize(someData);

                //foreach (var val in someData)
                //{
                //    Console.Write(val);
                //}


            }

            Console.ReadKey();


        }


        static byte[] TransformSerializedArray(byte[] serializedBuffer, byte[] shiftedBuffer)
        {
            // positions[0]  - tracks current shifted buffer position to copy into 
            // positions[1] - tracke current original buffer position to copy into

            byte[] positions = new byte[2];

            for (int i = 0; i <= serializedBuffer.Length - 1; i++)
            {
                //Note the value of next byte is initially given as the first byte in the original array: serializedBuffer
                //start with a recursion count of 0 since no recursion has occured
                positions = RecursiveStuffAndShift(positions, serializedBuffer[i], serializedBuffer, shiftedBuffer, positions[1], positions[0], 0);
                CopyArrays(shiftedBuffer, positions[0], serializedBuffer, positions[1]);

                if (positions[1] >= serializedBuffer.Length - 1)
                {
                    break;
                }
                i = positions[1]; //update i to increment/sync up with the positions[1] variable on the next iteration
                positions[0]++; //increment shifterBuffer position to copy to  because copy is complete
                positions[1]++; //increment serializedBuffer position to copy to because copy is complete
            }
            return shiftedBuffer;
        }


        static byte[] RecursiveStuffAndShift(byte[] positions, byte currentByte, byte[] originalArray, byte[] shiftedArray, byte cpyFromPosInOriginal, byte cpyToPosInShifted, byte recursionCnt)
        {
            bool specialChar = CheckIfSpecial(currentByte);

            // if special and not last or first byte (will skip this section if stuffing and shifting first and last byte)
            if (specialChar && (cpyFromPosInOriginal < originalArray.Length - 1 && cpyFromPosInOriginal != 0)) //stuff and shift copy
            {
                
                //Stuff - replace original position with 0x10
                StuffArray(shiftedArray, cpyToPosInShifted); //problem is here

                cpyToPosInShifted++;
                //Shift - replace next position with the XOR or the currentByte and 0x40 (currentByte ^ 0x40)
                ShiftArray(shiftedArray, cpyToPosInShifted, currentByte);

                cpyToPosInShifted++;
                //get next byte before incrementing to cpy from counter
                currentByte = originalArray[cpyFromPosInOriginal + 1];

                //increment cpyFrom position because we have performed a copy of cpyFrom position to copyToPos even if it isn't through the copy method
                cpyFromPosInOriginal++;
                positions[0] = cpyToPosInShifted;
                positions[1] = cpyFromPosInOriginal;

                RecursiveStuffAndShift(positions, currentByte, originalArray, shiftedArray, (byte)(cpyFromPosInOriginal), cpyToPosInShifted, recursionCnt += 1);

                return positions;
            
            }
            else
            {
                return positions;
            }
        }

        static int ShiftArray(byte[] stuffAndShiftedArray, byte cpyToPosInShifted, byte refByte)
        {
            stuffAndShiftedArray[cpyToPosInShifted] = (byte)(refByte ^ (byte)ControlDefinitions.DPT_SpecialChar_t.ShiftXOR_e);
            return cpyToPosInShifted + 2;
        }

        static void StuffArray(byte[] stuffAndShiftedArray, byte cpyToPosInShifted)
        {
            stuffAndShiftedArray[cpyToPosInShifted] = (byte)ControlDefinitions.DPT_SpecialChar_t.DLE_e;
        }


        static void CopyArrays(byte[] stuffAndShiftedArray, byte shiftedArrayIndex, byte[] originalArray, byte refIndex)
        {
            if(refIndex < originalArray.Length)
            {
                stuffAndShiftedArray[shiftedArrayIndex] = originalArray[refIndex];

            }

        }

        static void DoRinse()
        {

            // Do rinse example 

            Telegram doRinse = new Telegram();
            doRinse.SOH = (byte)ControlDefinitions.DPT_SpecialChar_t.SOH_e;
            doRinse.PIP = 0x00;
            doRinse.PIE = 0x6A;
            doRinse.PN = 0x01;
            doRinse.SA = 0x41;
            doRinse.DA = 0x42;
            doRinse.EOT = (byte)ControlDefinitions.DPT_SpecialChar_t.EOT_e;

            byte[] serializedBuffer =
            {
                    doRinse.SOH,
                    doRinse.PIP,
                    doRinse.PIE,
                    doRinse.PN,
                    doRinse.SA,
                    doRinse.DA,
                    0, // where the CRC goes

                };

            //For CRC CALCULATION


            //serialize the struct
            //byte[] serializedBuffer = RawSerialize(doRinse);
            //ushort CRC = API_CalculateCRC(ref serializedBuffer, serializedBuffer.Length);
            //serializedBuffer[serializedBuffer.Length-1] = CRC;



            //// TEST VARIABLES ////// 
            //byte[] serializedBuffer = new byte[]
            //{
            //    0x99,
            //    0x00,
            //    0x00,
            //    0x00,
            //    0x00,
            //    0x99,
            //};

            //Size must be double to ensure the case that each character is a special character
            byte[] shiftedBuffer = new byte[serializedBuffer.Length * 2];
            shiftedBuffer = TransformSerializedArray(serializedBuffer, shiftedBuffer);

            //get hexadecimal vals
            string serializedMessage_h = BitConverter.ToString(serializedBuffer);
            string shiftedBuffer_h = BitConverter.ToString(shiftedBuffer);

            foreach (var val in serializedMessage_h)
            {
                Console.Write(val);
            }
            Console.WriteLine();

            foreach (var val in shiftedBuffer_h)
            {
                Console.Write(val);
            }

        }

        static void SendMessage(Telegram message)
        {

        }

        static void DeserializeResponse(byte[] message)
        {

        }


        static bool CheckIfSpecial(byte byteToCheck)
        {
            foreach (var val in Enum.GetValues(typeof(ControlDefinitions.DPT_SpecialChar_t))) //can get into this loop is specialchar is false?
            {
                //if detecting a special character
                if (byteToCheck == (int)val)
                {
                    return true;
                }

            }
            return false;
        }

        public static byte[] RawSerialize(object anything)
        {
            int rawSize = Marshal.SizeOf(anything);
            IntPtr buffer = Marshal.AllocHGlobal(rawSize);
            Marshal.StructureToPtr(anything, buffer, false);
            byte[] rawDatas = new byte[rawSize];
            Marshal.Copy(buffer, rawDatas, 0, rawSize);
            Marshal.FreeHGlobal(buffer);
            return rawDatas;
        }

        static string ReadFromCoffee(List<SerialPort> ports)
        {
            return ports[1].ReadExisting();

        }

        static void WriteToPi(string input, List<SerialPort> ports)
        {
            ports[0].Write(input);



        }

        static List<SerialPort> openSerialPorts()
        {
            // Raspberry
            SerialPort raspPort = new SerialPort();
            raspPort.PortName = "COM1";
            raspPort.DataBits = 8;
            raspPort.Parity = Parity.None;
            raspPort.StopBits = StopBits.One;
            raspPort.BaudRate = 9600;

            // Coffee Machine
            SerialPort coffPort = new SerialPort();
            coffPort.PortName = "COM2";
            coffPort.DataBits = 8;
            coffPort.Parity = Parity.None;
            coffPort.StopBits = StopBits.One;
            coffPort.BaudRate = 9600;


            raspPort.Open();
            coffPort.Open();

            List<SerialPort> openPorts = new List<SerialPort>();
            openPorts.Add(raspPort);
            openPorts.Add(coffPort);

            return openPorts;
        }


        static ushort API_CalculateCRC(ref byte[] data_p, int dataLength)
        {
            ushort[] crcPolynomTable =
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

        int nIndex;
            ushort checkSum;
            const ushort INIT_CRC = 0xFFFF;
            checkSum = INIT_CRC;
            for (nIndex = 0; nIndex < dataLength; nIndex++)
            {
                checkSum = (ushort)((checkSum >> 8) ^ crcPolynomTable[(checkSum ^ data_p[nIndex]) & 0xFF]);
            }
            return checkSum;
        }


}
}
