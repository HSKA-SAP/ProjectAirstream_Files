using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAirstream
{
    public class Commands
    {

        const byte MILK_OUTLET_DATA_SIZE = 3;
        const ushort MILK_TUBE_LENGTH = 350;
        const byte SCREEN_RINSE_DATA_SIZE = 2;
        const byte RIGHT_SIDE = 1;
        const byte LEFT_SIDE = 0;

        public Commands()
        {

        }
        static string CreateTelegram(byte PIP, byte PIE, byte PN, byte SA, byte DA, byte MI, ushort MP, ushort DL, bool dataPackets, byte[] data)
        {

            Telegram doRinse = new Telegram();
            doRinse.SOH = (byte)ControlDefinitions.DPT_SpecialChar_t.SOH_e;
            doRinse.MP = MP;
            doRinse.DL = DL;
            doRinse.EOT = (byte)ControlDefinitions.DPT_SpecialChar_t.EOT_e;
            //Convert 16 bit chars
            ushort[] list = { doRinse.MP, doRinse.DL };


            //create an array of converted 16 bit values
            byte[] array = new byte[list.Length * 3];
            for (int i = 0; i < list.Length; i++)
            {
                array[i + 1 * i] = (BitConverter.GetBytes(list[i]))[0];
                array[(i + 1) + 1 * i] = (BitConverter.GetBytes(list[i]))[1];
            }

            /******************Calculate CRC********************/

            byte[] serializedBufferOriginal =
            {
                    //Packet Header
                    doRinse.PIP = PIP,
                    doRinse.PIE = PIE,
                    doRinse.PN = PN,
                    doRinse.SA = SA,
                    doRinse.DA = DA,
                    //Data header
                    doRinse.MI = MI,
                    array[0], array[1], //MP
                    array[2], array[3], //DL
                    //Data 
                };


            byte dataLength = (byte)(serializedBufferOriginal.Length);
            doRinse.CRC = API_CalculateCRC(ref serializedBufferOriginal, dataLength);
            /*************************************************/

            array[4] = (BitConverter.GetBytes(doRinse.CRC))[0];
            array[5] = (BitConverter.GetBytes(doRinse.CRC))[1];

            List<byte> serializedBufferCRCList = new List<byte>();
            serializedBufferCRCList.AddRange(serializedBufferOriginal);
            if (dataPackets)
            {
                serializedBufferCRCList.AddRange(data);
            }
            //Add CRC
            serializedBufferCRCList.Add(array[4]);
            serializedBufferCRCList.Add(array[5]);
            byte[] serializedBufferCRC = new byte[serializedBufferOriginal.Length + 2];
            serializedBufferCRC = serializedBufferCRCList.ToArray();

            //Stuffing and shifting
            //Size must be double incase each character is a special character
            byte[] shiftedBuffer = new byte[serializedBufferCRC.Length * 2];
            Tuple<byte[], byte> output = TransformSerializedArray(serializedBufferCRC, shiftedBuffer);


            List<byte> shiftedBufferList = new List<byte>();
            shiftedBufferList.Add((byte)ControlDefinitions.DPT_SpecialChar_t.SOH_e);
            shiftedBufferList.AddRange(shiftedBuffer.Take(output.Item2).ToArray());
            shiftedBufferList.Add((byte)ControlDefinitions.DPT_SpecialChar_t.EOT_e);

            byte[] finalTelegram = shiftedBufferList.ToArray();

            string finalTelegramAsString = BitConverter.ToString(finalTelegram);
            return finalTelegramAsString;


        }



        static Tuple<byte[], byte> TransformSerializedArray(byte[] serializedBuffer, byte[] shiftedBuffer)
        {
            // positions[0]  - tracks current shifted buffer position to copy into 
            // positions[1] - tracks current original buffer position to copy into

            byte[] positions = new byte[2];
            byte finalIndex = 0;

            for (int i = 0; i <= serializedBuffer.Length - 1; i++)
            {
                //Note the value of next byte is initially given as the first byte in the original array: serializedBuffer
                //start with a recursion count of 0 since no recursion has occured
                positions = RecursiveStuffAndShift(positions, serializedBuffer[i], serializedBuffer, shiftedBuffer, positions[1], positions[0], 0);
                CopyArrays(shiftedBuffer, positions[0], serializedBuffer, positions[1]);

                if (positions[1] >= serializedBuffer.Length - 1)
                {
                    finalIndex = positions[0];
                    break;
                }
                i = positions[1]; //update i to increment/sync up with the positions[1] variable on the next iteration
                positions[0]++; //increment shifterBuffer position to copy to  because copy is complete
                positions[1]++; //increment serializedBuffer position to copy to because copy is complete
            }
            return Tuple.Create(shiftedBuffer, finalIndex += 1);
        }


        static byte[] RecursiveStuffAndShift(byte[] positions, byte currentByte, byte[] originalArray, byte[] shiftedArray, byte cpyFromPosInOriginal, byte cpyToPosInShifted, byte recursionCnt)
        {
            bool specialChar = CheckIfSpecial(currentByte);

            // if special and not last or first byte (will skip this section if stuffing and shifting first and last byte)
            if (specialChar) //stuff and shift copy
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
            if (refIndex < originalArray.Length)
            {
                stuffAndShiftedArray[shiftedArrayIndex] = originalArray[refIndex];

            }

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

        /**************************************** COMMANDS  *****************************************/
        static string DoRinse(byte MI, ushort MP, ushort DL, byte seqNum)
        {
            string telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, MI, MP, DL, false, new byte[] { 0 });
            return telegram;

        }
        public static string DoRinseLeft(byte seqNum)
        {
            string telegram = DoRinse((byte)ControlDefinitions.API_Command_t.DoRinse_e, LEFT_SIDE, 0,seqNum);
            return telegram;
        }
        public static string DoRinseRight(byte seqNum)
        {
            string telegram = DoRinse((byte)ControlDefinitions.API_Command_t.DoRinse_e, RIGHT_SIDE, 0,seqNum);
            return telegram;

        }
        public static string StartClean(byte seqNum)
        {
            string telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, (byte)ControlDefinitions.API_Command_t.StartCleaning_e, 0, 0, false, new byte[] { 0 });
            return telegram;
        }

        /// <summary>
        ///  Check if L or R outlet needs to be rinsed 
        /// </summary>
        /// <returns>The telegram for the request </returns>
        public static string GetRequest(byte seqNum)
        {
            string telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, (byte)ControlDefinitions.API_Command_t.GetRequests_e, 0, 0, false, new byte[] { 0 });
            return telegram;
        }


        public static string StopProcess(byte module, byte seqNum)
        {
            string telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, (byte)ControlDefinitions.API_Command_t.Stop_e, module, 0x00, false, new byte[] { 0 });
            return telegram;
        }

        public static string StopAllProcess(byte seqNum)
        {
            string telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, (byte)ControlDefinitions.API_Command_t.Stop_e, 0, 0x00, false, new byte[] { 0 });
            return telegram;
        }

        static string RinseMilkOutlet(byte rinseMode, ushort side, byte seqNum)
        {
            byte[] array = new byte[MILK_OUTLET_DATA_SIZE];
            array[0] = rinseMode;
            array[1] = (BitConverter.GetBytes(MILK_TUBE_LENGTH))[0];
            array[2] = (BitConverter.GetBytes(MILK_TUBE_LENGTH))[1];

            string telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, (byte)ControlDefinitions.API_Command_t.MilkOutletRinse_e, side, MILK_OUTLET_DATA_SIZE, true, array);
            return telegram;
        }

        public static string RinseRightMilkOutlet(byte seqNum)
        {
            string telegram = RinseMilkOutlet(1, RIGHT_SIDE,seqNum);
            return telegram;
        }
        public static string RinseLeftMilkOutlet(byte seqNum)
        {
            string telegram = RinseMilkOutlet(1, LEFT_SIDE,seqNum);
            return telegram;
        }

        public static string RinseRightTubes(byte seqNum)
        {
            string telegram = RinseMilkOutlet(2, RIGHT_SIDE,seqNum);
            return telegram;
        }

        public static string RinseLeftTubes(byte seqNum)
        {
            string telegram = RinseMilkOutlet(2, LEFT_SIDE,seqNum);
            return telegram;
        }
        public static string RinseRightTubesAndOutlet(byte seqNum)
        {
            string telegram = RinseMilkOutlet(0, RIGHT_SIDE,seqNum);
            return telegram;
        }
        public static string RinseLeftTubesAndOutlet(byte seqNum)
        {
            string telegram = RinseMilkOutlet(0, LEFT_SIDE,seqNum);
            return telegram;
        }

        static string DoScreenRinse(byte side, byte seqNum)
        {
            byte[] array = new byte[SCREEN_RINSE_DATA_SIZE];
            array[0] = 3; // screen rinse cycles
            array[1] = 10; //repetitions

            string telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, (byte)ControlDefinitions.API_Command_t.ScreenRinse_e, side, SCREEN_RINSE_DATA_SIZE, true, array);
            return telegram;
        }

        public static string DoRightScreenRinse(byte seqNum)
        {
            string telegram = DoScreenRinse(RIGHT_SIDE,seqNum);
            return telegram;
        }
        public static string DoLeftScreenRinse(byte seqNum)
        {
            string telegram = DoScreenRinse(LEFT_SIDE,seqNum);
            return telegram;
        }

        public static string GetInfoMessage(byte seqNum)
        {
            byte[] array = new byte[] { 0, 0, 0 };
            string telegram = CreateTelegram(0x00, 0x68,seqNum, 0x42, 0x41, (byte)ControlDefinitions.API_Command_t.GetInfoMessages_e, 0, 3, true, array);
            return telegram;
        }

        public static string DisplayAction(ControlDefinitions.API_DisplayAction_t action, byte seqNum)
        {
            string telegram = CreateTelegram(0x00, 0x68,seqNum, 0x42, 0x41, (byte)ControlDefinitions.API_Command_t.DisplayAction_e, (byte)action, 0, false, new byte[] { 0 });
            return telegram;
        }


        static string GetProductDump(byte side, byte seqNum)
        {
            string telegram = CreateTelegram(0x00, 0x68,seqNum, 0x42, 0x41, (byte)ControlDefinitions.API_Command_t.GetProductDump_e, side, 0, false, new byte[] { 0 });
            return telegram;

        }

        public static string GetProductDumpRight(byte seqNum)
        {
            string telegram = GetProductDump(RIGHT_SIDE, seqNum);
            return telegram;
        }
        public static string GetProductDumpLeft(byte seqNum)
        {
            string telegram = GetProductDump(LEFT_SIDE,seqNum);
            return telegram;
        }

        public static string GetSensorValues(byte seqNum)
        {
            string telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, (byte)ControlDefinitions.API_Command_t.GetSensorValues_e, 0, 0, false, new byte[] { 0 });
            return telegram;
        }
        public static string GetStatus(byte seqNum)
        {
            string telegram = CreateTelegram(0x00,0x6C, seqNum, 0x42, 0x41, (byte)ControlDefinitions.API_Command_t.GetStatus_e,0,0, false, new byte[] { 0 });
            return telegram;

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
