using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.InteropServices;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Telegram doRinse = new Telegram();

            List<byte> shiftArray = new List<byte>();

            shiftArray[0] = 1;

            foreach(var val in shiftArray)
            {
                Console.Write(val);
            }

            Console.ReadKey();
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

    }
}
