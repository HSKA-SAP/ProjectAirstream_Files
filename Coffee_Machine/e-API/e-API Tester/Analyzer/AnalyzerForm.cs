using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.IO.Ports;

using ApiSerialComm;
using eApi.Typedef;

namespace eApi.Analyzer
{
    public partial class AnalyzerForm : Form
    {
        private delegate void ControlCallback(SerialComm.Packet_t p);
        ControlCallback addSpfToApi;
        ControlCallback addSpfToMachine;

        private Hub.ComportHub hub;

        private SerialComm ApiAnalyzer;
        private SerialComm MachineAnalyzer;
        public int MachineBaudrate;
        public string MachinePortName;

        public AnalyzerForm(SerialPort machine, SerialComm Api)
        {
            InitializeComponent();

            MachineBaudrate = machine.BaudRate;
            MachinePortName = machine.PortName;
            initAnalyzers();

            hub = new eApi.Analyzer.Hub.ComportHub(machine, MachineAnalyzer, ApiAnalyzer, Api);

            this.FormClosing += new FormClosingEventHandler(AnalyzerForm_FormClosing);
        }

        public AnalyzerForm(string PortName, int BaudRate, SerialComm Api)
        {
            InitializeComponent();

            addSpfToApi += new ControlCallback(addSerialPacketFieldApi);
            addSpfToMachine += new ControlCallback(addSerialPacketFieldMachine);

            MachineBaudrate = BaudRate;
            MachinePortName = PortName;
            SerialPort machine = new SerialPort(PortName, BaudRate);
            initAnalyzers();

            hub = new eApi.Analyzer.Hub.ComportHub(machine, MachineAnalyzer, ApiAnalyzer, Api);
        }

        private void initAnalyzers()
        {
            ApiAnalyzer = new SerialComm(MachinePortName, MachineBaudrate, (byte)ModuleId_t.Touch_e);
            ApiAnalyzer.ReceiveOnlyMode = true;
            ApiAnalyzer.RegisterAckTimeoutHandler(apiAckTimeoutCallback);
            ApiAnalyzer.RegisterNackFailHandler(apiNackFailCallback);
            ApiAnalyzer.RegisterPacketReceivedHandler(apiReceivedCallback);
            ApiAnalyzer.RegisterPacketSentHandler(apiSentCallback);
            ApiAnalyzer.RegisterResponseTimeoutHandler(apiResponseTimeoutCallback);
            ApiAnalyzer.ackNackReceived += new SerialComm.PacketEvent(ApiAnalyzer_ackNackReceived);
            ApiAnalyzer.StartThread();

            MachineAnalyzer = new SerialComm(MachinePortName, MachineBaudrate, (byte)ModuleId_t.Api_e);
            MachineAnalyzer.ReceiveOnlyMode = true;
            MachineAnalyzer.RegisterAckTimeoutHandler(machineAckTimeoutCallback);
            MachineAnalyzer.RegisterNackFailHandler(machineNackFailCallback);
            MachineAnalyzer.RegisterPacketReceivedHandler(machineReceivedCallback);
            MachineAnalyzer.RegisterPacketSentHandler(machineSentCallback);
            MachineAnalyzer.RegisterResponseTimeoutHandler(machineResponseTimeoutCallback);
            MachineAnalyzer.ackNackReceived += new SerialComm.PacketEvent(MachineAnalyzer_ackNackReceived);
            MachineAnalyzer.StartThread();
        }

        public void changeMachinePort(SerialPort sp)
        {
            hub.closeMachinePort();
        }

        public void closeMachinePort()
        {
            hub.closeMachinePort();
        }

        public void openMachinePort()
        {
            hub.openMachinePort();
        }

        #region api callbacks
        void ApiAnalyzer_ackNackReceived(object sender, SerialComm.PacketEventArgs e)
        {
            try
            {
                if (e.isTimeout == false)
                {
                    BeginInvoke(addSpfToApi, e.packet);
                }
            }
            catch { }
        }

        public void apiReceivedCallback(SerialComm.Packet_t packet)
        {
            try
            {
                BeginInvoke(addSpfToApi, packet);
            }
            catch { }
        }

        public void apiSentCallback(SerialComm.Packet_t packet)
        {
            try
            {
            }
            catch { }
        }

        public void apiAckTimeoutCallback(SerialComm.Packet_t packet)
        {
            try
            {
            }
            catch { }
        }

        public void apiNackFailCallback(SerialComm.Packet_t packet)
        {
            try
            {
            }
            catch { }
        }

        public void apiResponseTimeoutCallback(SerialComm.Packet_t packet)
        {
            try
            {
            }
            catch { }
        }
        #endregion

        #region machine callbacks
        void MachineAnalyzer_ackNackReceived(object sender, SerialComm.PacketEventArgs e)
        {
            try
            {
                BeginInvoke(addSpfToMachine, e.packet);
            }
            catch { }
        }

        public void machineReceivedCallback(SerialComm.Packet_t packet)
        {
            try
            {
                BeginInvoke(addSpfToMachine, packet);
            }
            catch { }
        }

        public void machineSentCallback(SerialComm.Packet_t packet)
        {
            try
            {

            }
            catch { }
        }

        public void machineAckTimeoutCallback(SerialComm.Packet_t packet)
        {
            try
            {
            }
            catch { }
        }

        public void machineNackFailCallback(SerialComm.Packet_t packet)
        {
            try
            {
            }
            catch { }
        }

        public void machineResponseTimeoutCallback(SerialComm.Packet_t packet)
        {
            try
            {
            }
            catch { }
        }
        #endregion


        void AnalyzerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Close();
        }

        public void Close()
        {
            if (ApiAnalyzer != null)
                ApiAnalyzer.Dispose();
            if (MachineAnalyzer != null)
                MachineAnalyzer.Dispose();
            if (hub != null)
                hub.Close();
        }

        private void addSerialPacketFieldApi(SerialComm.Packet_t packet)
        {
            switch (cmbxProcessLevel.SelectedIndex)
            {
                case 1: // raw packet
                    byte[] arr = packet.ToRawArray();
                    SerialRawPacketField srpf = new SerialRawPacketField(arr,arr.Length);

                    flowApi.Controls.Add(srpf);
                    flowApi.Controls.SetChildIndex(srpf, 0);
                    srpf.Show();
                    break;

                case 0: // processed packet
                default:
                    SerialPacketField spf = new SerialPacketField();
                    spf.showTime();
                    spf.telegram.FromPacket(packet);
                    spf.showTelegram();

                    flowApi.Controls.Add(spf);
                    flowApi.Controls.SetChildIndex(spf, 0);
                    spf.Show();
                    break;
            }
            
        }

        private void addSerialPacketFieldMachine(SerialComm.Packet_t packet)
        {
            switch (cmbxProcessLevel.SelectedIndex)
            {
                case 1: // raw packet
                    byte[] arr = packet.ToRawArray();
                    SerialRawPacketField srpf = new SerialRawPacketField(arr, arr.Length);

                    flowMachine.Controls.Add(srpf);
                    flowMachine.Controls.SetChildIndex(srpf, 0);
                    srpf.Show();
                    break;

                case 0: // processed packet
                default:
                    SerialPacketField spf = new SerialPacketField();
                    spf.showTime();
                    spf.telegram.FromPacket(packet);
                    spf.showTelegram();

                    flowMachine.Controls.Add(spf);
                    flowMachine.Controls.SetChildIndex(spf, 0);
                    spf.Show();
                    break;
            }
        }
    }
}
