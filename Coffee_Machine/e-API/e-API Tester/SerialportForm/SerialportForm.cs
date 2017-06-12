/**
********************************************************************************
  \file PasswordForm.cs

  \brief  Fenster für Passworteingabe

  \author Stephan Zink

  $Id: PasswordForm.cs 71 2009-12-17 18:20:04Z s.zink $

  (c) by delisys ag / All rights reserved. */

/** \addtogroup 
  @{
 *******************************************************************************
*/

using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

using ApiSerialComm;
using eApi.Typedef;

namespace eApi
{
    public partial class SerialportForm : Form
    {
        public SerialPort serialPort;

        private int nbrOfSerialPorts = 0;
        private SerialComm autoSession;

        public SerialportForm()
        {
            InitializeComponent( );
            serialPort = new SerialPort();
            serialPort.BaudRate = 115200;
        }

        private void enter_MouseDown( object sender, MouseEventArgs e )
        {
            enter.ForeColor = Color.FromArgb( 80, 80, 80 );
            enter.BackColor = Color.White;
        }

        private void enter_MouseUp( object sender, MouseEventArgs e )
        {
            enter.ForeColor = Color.White;
            enter.BackColor = Color.FromArgb( 64, 64, 64 );
        }

        private void enter_Click( object sender, EventArgs e )
        {
            try
            {
                if( nbrOfSerialPorts > 0 )
                {
                    serialPort.PortName = comboBoxPorts.SelectedItem.ToString( );
                    serialPort.BaudRate = Convert.ToInt32( textBoxBaudRate.Text );
                    serialPort.Open();
                    serialPort.Close();
                }
                this.Close( );
            }
            catch { }
        }

        private void SerialportForm_Load( object sender, EventArgs e )
        {
            bool portIsAvailable = false;
            nbrOfSerialPorts = SerialPort.GetPortNames( ).Length;

            if( nbrOfSerialPorts > 0 )
            {
                if (nbrOfSerialPorts == 1)
                {
                    serialPort.PortName = SerialPort.GetPortNames()[0];
                    serialPort.BaudRate = 115200;
                    this.Close();
                }
                else
                {
                    foreach (string s in SerialPort.GetPortNames())
                    {
                        portIsAvailable = false;
                        try
                        {
                            serialPort.PortName = s;
                            serialPort.BaudRate = 115200;
                            serialPort.Open();
                            serialPort.Close();
                            portIsAvailable = true;
                        }
                        catch { }
                        if(portIsAvailable == true)
                            comboBoxPorts.Items.Add(s);
                    }

                    comboBoxPorts.SelectedIndex = 0;
                    textBoxBaudRate.Text = serialPort.BaudRate.ToString();
                }
            }
            else
            {
                comboBoxPorts.Items.Add( "NO COM" );
                comboBoxPorts.SelectedIndex = 0;
            }
        }

        private enum MachineStatus_t : int
        {
            TryConnection = -1,
            MachineFound = 1,
            MachineFailed = 0
        }
        private MachineStatus_t status = MachineStatus_t.TryConnection;
        private bool autoConnectTimeout = false;
        private Timer autoConnectTimeoutTmr = new Timer();
        private void btnAutomatic_Click(object sender, EventArgs e)
        {
            attemptAutoConnect();
        }

        void attemptAutoConnect()
        {
            btnAutomatic.Text = "searching...";
            btnAutomatic.Update();
            foreach (string s in comboBoxPorts.Items)
            {
                try
                {
                    if (autoSession != null)
                    {
                        autoSession.Pause();
                        while (autoSession.PortIsOpen) ;
                    }

                    autoSession = new SerialComm(s, Convert.ToInt32(textBoxBaudRate.Text), (byte)ModuleId_t.Api_e);
                    autoSession.RegisterAckTimeoutHandler(ackTimeoutCallback);
                    autoSession.RegisterPacketSentHandler(packetSentCallback);
                    autoSession.MaxSendRepetition = 1;
                    autoSession.AckTimeout = TimeSpan.FromMilliseconds(100);
                    autoSession.ResponseTimeout = TimeSpan.FromMilliseconds(100);
                    autoSession.Start();

                    autoConnectTimeoutTmr.Interval = 1000;
                    autoConnectTimeoutTmr.Tick += new EventHandler(autoConnectTimeoutTmr_Tick);
                    autoConnectTimeoutTmr.Start();

                    // send GetStatus request
                    autoSession.EnqueuePacket((byte)ModuleId_t.Touch_e, new SerialComm.Message_t((byte)API_Command_t.GetStatus_e, 0), new byte[0],
                                                0, true, SerialComm.Encrypt_t.No, 0, SerialComm.ApplicationPort_t.Api_e, 0xFF);
                    while (status == MachineStatus_t.TryConnection && autoConnectTimeout == false)
                    {
                    }
                    autoConnectTimeoutTmr.Stop();
                    autoConnectTimeout = false;

                    if (status == MachineStatus_t.MachineFound)
                    {
                        autoSession.Dispose();
                        serialPort.PortName = autoSession.PortName;
                        serialPort.BaudRate = autoSession.BaudRate;
                        this.Close();
                    }
                    else
                    {
                        status = MachineStatus_t.TryConnection;
                    }
                }
                catch { }
            }
            if(autoSession != null)
                autoSession.Dispose();
            btnAutomatic.Text = "nothing found";
            resetTimer.Interval = 2000;
            resetTimer.Tick += new EventHandler(resetTimer_Tick);
            resetTimer.Start();
        }

        void autoConnectTimeoutTmr_Tick(object sender, EventArgs e)
        {
            autoConnectTimeoutTmr.Stop();
            autoConnectTimeout = true;
        }

        Timer resetTimer = new Timer();
        void resetTimer_Tick(object sender, EventArgs e)
        {
            resetTimer.Stop();
            btnAutomatic.Text = "Auto connect";
        }

        private void packetSentCallback(SerialComm.Packet_t packet)
        {
            status = MachineStatus_t.MachineFound;
        }

        private void ackTimeoutCallback(SerialComm.Packet_t packet)
        {
            status = MachineStatus_t.MachineFailed;
        }

        new public void Dispose()
        {
            if (autoSession != null)
                autoSession.Dispose();
            if (resetTimer != null)
                resetTimer.Stop();
        }
    }
}