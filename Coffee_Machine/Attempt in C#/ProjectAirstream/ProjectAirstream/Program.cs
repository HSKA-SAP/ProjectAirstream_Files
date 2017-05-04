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
using System.Timers;


namespace ProjectAirstream
{
    public class Program
    {
        const byte MILK_OUTLET_DATA_SIZE = 3;
        const ushort MILK_TUBE_LENGTH = 350;
        const byte SCREEN_RINSE_DATA_SIZE = 2;
        const byte RIGHT_SIDE = 1;
        const byte LEFT_SIDE = 0;


        public enum PacketTypes
        {
            ACK = 0x6A,
            NACK = 0x6B,
            COMMAND = 0x68,
            REQUEST = 0x6C,
            RESPONSE = 0x68,
        }

        static void Main(string[] args)
        {
            List<SerialPort> openPorts = openSerialPorts();
            byte seqNumber = 0;
            string commandTelegram;


            Console.WriteLine("Check status...[ENTER]");

            //Send Get status command and check if ready 
            commandTelegram = Commands.GetStatus(seqNumber);

            //Will stay in here forever if no response from coffe machine
            DoCommand(openPorts, seqNumber, commandTelegram);

            Console.ReadKey();


        }

        static void DoCommand(List<SerialPort> ports, byte seqNumber, string telegram)
        {
            Timer commandTimer = InitCommand(ports, seqNumber, telegram);
            while (commandTimer.Enabled == true) ; 
        }


        static void CreateTimer(List<SerialPort> ports, ref byte seqNumber)
        {

        }

        static Timer InitCommand(List<SerialPort> ports, byte seqNumber, string telegram)
        {

                Timer pollingTimer = new Timer();
                pollingTimer.Enabled = true;
                pollingTimer.Elapsed += (state, e) => CheckForACK(state, e, pollingTimer, ports, seqNumber,telegram);
                pollingTimer.Interval = 1000;
                return pollingTimer;
          
        }

        static string DoRequest(List<SerialPort> ports, byte seqNumber, bool ready, string telegram) {
            InitCommand(ports, seqNumber, telegram);
            //Get response 
            System.Threading.Thread.Sleep(100);
            string response = ports[0].ReadExisting();
            return response;


        }
        static void CheckForACK(Object state, ElapsedEventArgs e, Timer timer, List<SerialPort> ports, byte seqNumber, string telegram)
        {

            //Send request
            ports[1].Write(telegram); //Write to Coffee Machine
            System.Threading.Thread.Sleep(200); // Wait for a response

            //Check for ACK PACKET
            string response = ports[0].ReadExisting(); //Read from Pi
            string[] responseArray = response.Split('-');
            byte[] responseArrayBytes = new byte[responseArray.Length];

            if(responseArrayBytes.Length > 3)
            {
                for (int i = 0; i < responseArray.Length; i++) responseArrayBytes[i] = Convert.ToByte(responseArray[i], 16);
                if ((responseArrayBytes[3] == (byte)PacketTypes.ACK))
                {
                    Console.WriteLine("ACK Packet detected...");
                    Console.WriteLine(response);
                    timer.Enabled = false;
                    //do something
                }
                else
                {
                    Console.WriteLine("Received no ACK packet from coffee machine...sending packet again");

                }
            }

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
            openPorts.Add(raspPort); // [0] Raspberry Pi 
            openPorts.Add(coffPort); // [1] Coffee Machine

            return openPorts;
        }



}
}
