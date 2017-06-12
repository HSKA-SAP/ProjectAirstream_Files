using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.IO.Ports;

using ApiSerialComm;

namespace eApi.Analyzer.Hub
{
    class ComportHub
    {
        private SerialPort ApiExt;
        private SerialPort Machine;
        private SerialComm MachineAnalyzer;
        private SerialComm ApiAnalyzer;
        private SerialComm Api;

        private bool useExternalApi = false;

        public ComportHub(SerialPort machine, SerialComm machineAnalyzer, SerialComm apiAnalyzer, SerialComm api)
        {
            Machine = machine;
            ApiAnalyzer = apiAnalyzer;
            Api = api;
            MachineAnalyzer = machineAnalyzer;

            Api.RegisterDataSentInterceptHandler(ApiSentDataInterceptHandler);
            Machine.DataReceived += new SerialDataReceivedEventHandler(MachineDataReceived);

            Machine.Open();
        }

        private void startExternalApi(SerialPort ApiPort)
        {
            ApiExt = ApiPort;
            ApiExt.DataReceived += new SerialDataReceivedEventHandler(ApiExtDataReceived);
            useExternalApi = true;

            ApiExt.Open();
        }

        private void stopExternalApi()
        {
            useExternalApi = false;
            ApiExt.Close();
        }

        void ApiSentDataInterceptHandler(byte[] data)
        {
            Machine.Write(data, 0, data.Length);
            ApiAnalyzer.addRawData(data, data.Length);
        }

        void ApiExtDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buffer = new byte[ApiExt.ReadBufferSize];
            int bytesRead = ApiExt.Read(buffer, 0, buffer.Length);

            Machine.Write(buffer, 0, bytesRead);
            ApiAnalyzer.addRawData(buffer, bytesRead);
        }

        void MachineDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buffer = new byte[Machine.ReadBufferSize];
            int bytesRead = Machine.Read(buffer, 0, buffer.Length);

            if (useExternalApi && ApiExt != null)
            {
                ApiExt.Write(buffer, 0, bytesRead);
            }
            Api.addRawData(buffer, bytesRead);
            MachineAnalyzer.addRawData(buffer, bytesRead);
        }

        public void changeMachinePort(SerialPort machine)
        {
            if (Machine.IsOpen)
                Machine.Close();
            Machine = machine;
            Machine.Open();
            Machine.DataReceived += new SerialDataReceivedEventHandler(MachineDataReceived);
        }

        public void openMachinePort()
        {
            try
            {
                Machine.Open();
            }
            catch { }
        }

        public void closeMachinePort()
        {
            Machine.Close();
        }

        public void Close()
        {
            if (ApiExt != null)
            {
                ApiExt.Close();
                ApiExt.Dispose();
            }

            Api.RegisterDataSentInterceptHandler(null);
            Machine.Close();
            Machine.Dispose();
        }
    }
}
