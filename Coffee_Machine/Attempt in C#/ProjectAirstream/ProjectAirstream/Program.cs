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
        const byte MILK_OUTLET_DATA_SIZE = 3;
        const ushort MILK_TUBE_LENGTH = 350;
        const byte SCREEN_RINSE_DATA_SIZE = 2;
        const byte RIGHT_SIDE = 1;
        const byte LEFT_SIDE = 0;


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

                //Test
                string telegram = Commands.DoRinseRight();
                Console.WriteLine("Do right rinse telegram:");
                foreach(var val in telegram)
                {
                    Console.Write(val);
                }
                Console.WriteLine();

            }

            Console.ReadKey();


        }



        static void SendMessage(Telegram message)
        {

        }

        static void DeserializeResponse(byte[] message)
        {

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



}
}
