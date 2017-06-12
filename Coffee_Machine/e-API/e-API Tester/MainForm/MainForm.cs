using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ApiSerialComm;
using eApi.Commands;
using eApi.Typedef;

namespace eApi
{
    public enum CommStatus : byte
    {
        NoComm = 0,
        AckReceived = 1,
        NackReceived = 2,
        AckTimeout = 3,
        ResponseReceived = 4,
        ResponseTimeout = 5,
        PacketSent = 6,
    }

    public partial class MainForm : Form
    {
        private delegate void CommCallback(SerialComm.Packet_t packet, CommStatus status);
        CommCallback updateUI;

        public class PacketEventArgs : EventArgs
        {
            public PacketEventArgs(SerialComm.Packet_t p)
            {
                packet = p;
            }
            public SerialComm.Packet_t packet;
        }
        public delegate void ErrorEventHandler(object sender, PacketEventArgs e);
        public event ErrorEventHandler errorOccurredEvent;

        private SerialComm serialSession;
        private PictureBox[] arrowArray = new PictureBox[5];

        SerialportForm serialportForm;


        public MainForm()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

#if Analyzer
            btnAnalyzerPng.Enabled = true;
            btnAnalyzerPng.Visible = true;
#else
            btnAnalyzerPng.Visible = false;
            btnAnalyzerPng.Enabled = false;
            btnAnalyzerPng.BackgroundImage.Dispose();

            // Resize Comm panel to fit the whole height of the window.
            pnlComm.Size = new Size(pnlComm.Size.Width, pnlComm.Size.Height + (pnlComm.Location.Y - btnAnalyzerPng.Location.Y));
            pnlComm.Location = btnAnalyzerPng.Location;
            flowCommStatus.Size = new Size(flowCommStatus.Width, pnlComm.Size.Height);
#endif
            prgmInit();

            // start the CommPort selector form.
            serialportForm = new SerialportForm();
            serialportForm.FormClosing += new FormClosingEventHandler(serialportForm_FormClosing);
            serialportForm.Show();
            serialportForm.TopMost = true;
        }

        private void prgmInit()
        {
            updateUI += new CommCallback(updateCommUI);

            commandsInit();

            CmdList = CmdList.OrderBy(a => a.Text).ToList();

            // fill tableLayoutPanel
            foreach (CmdButton cmd in CmdList)
            {
                cmd.Anchor = AnchorStyles.Top;
                cmd.Width = 180;
                cmd.Height = 30;
                toolTip.SetToolTip(cmd, cmd.toolTip);

                flowCmdList.Controls.Add(cmd);
            }
            flowCmdListCheckForScrolling();
            
            clearDetails();

            // initialize the Comm panel
            for (int i = 0; i < arrowArray.Length; i++ )
            {
                arrowArray[i] = new PictureBox();
                arrowArray[i].Height = 64;
                arrowArray[i].SizeMode = PictureBoxSizeMode.Zoom;
                arrowArray[i].Dock = DockStyle.Top;
                flowCommStatus.Controls.Add(arrowArray[i]);
            }
        }

        /// <summary>
        /// change margins when the scrollbar is visible and when the view is expanded.
        /// </summary>
        private void flowCmdListCheckForScrolling()
        {
            if (CmdExpanded == false)
            {
                if (flowCmdList.Controls.Count > 11)
                {
                    flowCmdList.HorizontalScroll.Maximum = 0;
                    flowCmdList.AutoScroll = false;
                    flowCmdList.VerticalScroll.Visible = false;
                    flowCmdList.AutoScroll = true;
                    foreach (CmdButton cmd in CmdList)
                    {
                        cmd.Margin = new Padding(0, 3, 6, 3);
                    }
                }
                else if(flowCmdList.Controls.Count == 11)
                {
                    foreach (CmdButton cmd in CmdList)
                    {
                        if(CmdList.IndexOf(cmd, 0, 1) != 0)
                            cmd.Margin = new Padding(3, 2, 3, 3);
                        else
                            cmd.Margin = new Padding(3, 3, 3, 3);
                    }

                }
                else
                {
                    foreach (CmdButton cmd in CmdList)
                    {
                        cmd.Margin = new Padding(3, 3, 3, 3);
                    }
                }
            }
            else
            {
                if (flowCmdList.Controls.Count > 30)
                {
                    foreach (CmdButton cmd in CmdList)
                    {
                        cmd.Margin = new Padding(0, 3, 6, 3);
                    }
                }
                else
                {
                    foreach (CmdButton cmd in CmdList)
                    {
                        cmd.Margin = new Padding(3, 3, 3, 3);
                    }
                }
            }
        }

        /// <summary>
        /// Terminate the programm gracefully.
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialportForm != null)
            {
                serialportForm.Dispose();
            }
            if (serialSession != null)
            {
                serialSession.Dispose();
            }
            if (anaForm != null)
            {
                anaForm.Close();
            }
        }

        /// <summary>
        /// Start the serial communication.
        /// </summary>
        private void serialportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SerialportForm spf = sender as SerialportForm;

            serialSession = new SerialComm(spf.serialPort.PortName, spf.serialPort.BaudRate, (byte)ModuleId_t.Api_e);
            serialSession.MaxSendRepetition = 3;

            // Register all handler methods for the various comm events.
            serialSession.RegisterPacketReceivedHandler(telegramReceivedCallback);
            serialSession.RegisterPacketSentHandler(telegramSentCallback);
            serialSession.RegisterAckTimeoutHandler(ackTimeoutCallback);
            serialSession.RegisterNackFailHandler(nackFailCallback);
            serialSession.RegisterResponseTimeoutHandler(responseTimeoutCallback);
            serialSession.ackNackReceived += new SerialComm.PacketEvent(ackNackReceivedHandler);

            ApiDataReceived += new PacketCallback(API_ReceiveData);

            serialSession.Start();

            // Comm Labels
            lblPort.Text = "Portname: " + spf.serialPort.PortName;
            lblBaud.Text = "Baudrate: " + spf.serialPort.BaudRate;
        }

        /// <summary>
        /// Closes the port. Doesn't terminate the communication thread. (Queued packets remain)
        /// </summary>
        public void CloseSerialPort()
        {
            serialSession.Pause();
        }

        /// <summary>
        /// Start the serial communication.
        /// </summary>
        public void OpenSerialPort()
        {
            serialSession.Start();
        }

        /// <summary>
        /// Change the used serial port.
        /// </summary>
        public void ChangeSerialPort(string portname, int baudrate)
        {
            serialSession.ChangePort(portname, baudrate);
        }

        public bool IsConnected()
        {
            bool isConnected = false;

            try
            {
                isConnected = serialSession.PortIsOpen;
            }
            catch
            {
                isConnected = false;
            }

            return isConnected;
        }

        private int nextPictureBox = 0;
        private bool timeoutFlag = false;
        /// <summary>
        /// Updates the displayed information. Used to show every sent/received packet.
        /// </summary>
        /// <param name="p">Sent or received packet</param>
        /// <param name="status">Current status of the communication</param>
        public void updateCommUI(SerialComm.Packet_t p, CommStatus status)
        {
            // fix; this might have caused an IndexOutOfRangeException
            if ( p.data != null && p.dataLength > p.data.Length)
                p.dataLength = (ushort)p.data.Length;

            // temporary fix of IndexOutOfRange exception
            //try
            //{
                // check if the packet should be ignored
                if (ignoreSeqNrQueue.Count >= 1)
                {
                    if (ignoreSeqNrQueue.Peek() + 1 == p.sequenceNumber)
                        ignoreSeqNrQueue.Dequeue();
                }
                if (ignoreSeqNrQueue.Count >= 1)
                {
                    if (ignoreSeqNrQueue.Peek() == p.sequenceNumber)
                        return;
                }

                // show the packet according to its status.
                switch (status)
                {
                    case CommStatus.AckReceived:
                        timeoutFlag = false;
                        arrowArray[nextPictureBox].Image = Properties.Resources.arrow_left_green;
                        flowCommStatus.Controls.Add(arrowArray[nextPictureBox]);
                        toolTip.SetToolTip(arrowArray[nextPictureBox], "ack");
                        arrowArray[nextPictureBox].Show();
                        if (nextPictureBox < 5)
                            nextPictureBox++;
                        break;

                    case CommStatus.AckTimeout:
                        if (timeoutFlag == false)
                        {
                            arrowArray[nextPictureBox].Image = Properties.Resources.arrow_left_transparent;
                            flowCommStatus.Controls.Add(arrowArray[nextPictureBox]);
                            toolTip.SetToolTip(arrowArray[nextPictureBox], "ack timeout");
                            arrowArray[nextPictureBox].Show();
                            if (nextPictureBox < 5)
                                nextPictureBox++;
                            timeoutFlag = true;
                        }
                        break;

                    case CommStatus.NackReceived:
                        timeoutFlag = false;
                        if (nextPictureBox < arrowArray.Length)
                        {
                            arrowArray[nextPictureBox].Image = Properties.Resources.arrow_left_red;
                            flowCommStatus.Controls.Add(arrowArray[nextPictureBox]);
                            toolTip.SetToolTip(arrowArray[nextPictureBox], "nack");
                            arrowArray[nextPictureBox].Show();
                            nextPictureBox++;
                        }
                        break;

                    case CommStatus.ResponseReceived:
                        timeoutFlag = false;
                        if (nextPictureBox < arrowArray.Length)
                        {
                            arrowArray[nextPictureBox].Image = Properties.Resources.arrow_left_grey;
                            flowCommStatus.Controls.Add(arrowArray[nextPictureBox]);
                            toolTip.SetToolTip(arrowArray[nextPictureBox], "response");
                            arrowArray[nextPictureBox].Show();
                            nextPictureBox++;
                        }
                        fillInDetails(p);
                        break;

                    case CommStatus.ResponseTimeout:
                        if (timeoutFlag == false && nextPictureBox < arrowArray.Length)
                        {
                            arrowArray[nextPictureBox].Image = Properties.Resources.arrow_left_transparent;
                            flowCommStatus.Controls.Add(arrowArray[nextPictureBox]);
                            toolTip.SetToolTip(arrowArray[nextPictureBox], "response timeout");
                            arrowArray[nextPictureBox].Show();
                            nextPictureBox++;
                            timeoutFlag = true;
                        }
                        break;

                    case CommStatus.PacketSent:
                        // clear all displayed information.
                        timeoutFlag = false;
                        nextPictureBox = 0;
                        for (int i = 0; i < arrowArray.Length; i++)
                        {
                            arrowArray[i].Hide();
                        }

                        clearDetails();
                        fillOutDetails(p);

                        if (nextPictureBox < arrowArray.Length)
                        {
                            arrowArray[nextPictureBox].Image = Properties.Resources.arrow_left_grey;
                            arrowArray[nextPictureBox].Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            flowCommStatus.Controls.Add(arrowArray[nextPictureBox]);
                            toolTip.SetToolTip(arrowArray[nextPictureBox], "command/request");
                            arrowArray[nextPictureBox].Show();
                            nextPictureBox++;
                        }
                        break;

                    default:
                        break;
                }

                // close the expanded view.
                if (CmdExpanded == true)
                    btnExpand_Click(this, new EventArgs());

            //}
            //catch { }
        }

        /// <summary>
        /// Clear all displayed textboxes.
        /// </summary>
        private void clearDetails()
        {
            txtOutSOH.Text = "";
            txtOutPIP.Text = "";
            txtOutPIE.Text = "";
            txtOutPN.Text = "";
            txtOutSA.Text = "";
            txtOutDA.Text = "";
            txtOutMI.Text = "";
            txtOutMP.Text = "";
            txtOutDL.Text = "";
            txtOutCRC.Text = "";
            txtOutData.Text = "";
            txtOutRawData.Text = "";
            txtOutEOT.Text = "";

            txtInSOH.Text = "";
            txtInPIP.Text = "";
            txtInPIE.Text = "";
            txtInPN.Text = "";
            txtInSA.Text = "";
            txtInDA.Text = "";
            txtInMI.Text = "";
            txtInMP.Text = "";
            txtInDL.Text = "";
            txtInCRC.Text = "";
            txtInData.Text = "";
            txtInRawData.Text = "";
            txtInEOT.Text = "";
        }

        /// <summary>
        /// Show a packet in the outgoing part of the view.
        /// </summary>
        /// <param name="p">packet containing the information</param>
        private void fillOutDetails(SerialComm.Packet_t p)
        {
            string s = "";
            eApi.Analyzer.TelegramStructure telegram = new eApi.Analyzer.TelegramStructure();
            telegram.FromPacket(p);

            txtOutEOT.Text = "04";
            txtOutSOH.Text = "01";

            txtOutPIP.Text = String.Format("{0:x2}", telegram.pip).ToUpper();
            txtOutPIE.Text = String.Format("{0:x2}", telegram.pie).ToUpper();
            txtOutPN.Text = String.Format("{0:x2}", telegram.pn).ToUpper();
            txtOutSA.Text = String.Format("{0:x2}", telegram.sa).ToUpper();
            txtOutDA.Text = String.Format("{0:x2}", telegram.da).ToUpper();

            if (!telegram.isAckNack)
            {
                txtOutMI.Text = String.Format("{0:x2}", telegram.mi).ToUpper();
                txtOutMP.Text = String.Format("{0:x4}", telegram.mp).ToUpper();
                txtOutDL.Text = String.Format("{0:x4}", telegram.dl).ToUpper();
                txtOutCRC.Text = String.Format("{0:x4}", telegram.crc).ToUpper();
            }
            else
            {
                txtOutMI.Text = "--";
                txtOutMP.Text = "--";
                txtOutDL.Text = "--";
                txtOutCRC.Text = "----";
            }

            // Data
            for ( int i=0; i<p.dataLength; i++)
            {
                s+= "0x" + String.Format ("{0:X02}", ((byte)p.data[i])) + ", ";

            }
            if (s == "")
                s = "no data";

            txtOutData.Text = s;
            s = "";

            // Raw
            byte[] buffer = p.ToRawArray();
            for (int i = 0; i < buffer.Length; i++)
            {
                s += "0x" + String.Format("{0:X02}", ((byte)buffer[i])) + ", ";
            }
            if (s == "")
                s = "no data";

            txtOutRawData.Text = s;
        }

        /// <summary>
        /// Show a packet in the incoming part of the view.
        /// </summary>
        /// <param name="p">packet containing the information</param>
        private void fillInDetails(SerialComm.Packet_t p)
        {
            string s = "";
            eApi.Analyzer.TelegramStructure telegram = new eApi.Analyzer.TelegramStructure();
            telegram.FromPacket(p);

            txtInEOT.Text = "04";
            txtInSOH.Text = "01";

            txtInPIP.Text = String.Format("{0:x2}", telegram.pip).ToUpper();
            txtInPIE.Text = String.Format("{0:x2}", telegram.pie).ToUpper();
            txtInPN.Text = String.Format("{0:x2}", telegram.pn).ToUpper();
            txtInSA.Text = String.Format("{0:x2}", telegram.sa).ToUpper();
            txtInDA.Text = String.Format("{0:x2}", telegram.da).ToUpper();

            if (!telegram.isAckNack)
            {
                txtInMI.Text = String.Format("{0:x2}", telegram.mi).ToUpper();
                txtInMP.Text = String.Format("{0:x4}", telegram.mp).ToUpper();
                txtInDL.Text = String.Format("{0:x4}", telegram.dl).ToUpper();
                txtInCRC.Text = String.Format("{0:x4}", telegram.crc).ToUpper();
            }
            else
            {
                txtInMI.Text = "--";
                txtInMP.Text = "--";
                txtInDL.Text = "--";
                txtInCRC.Text = "----";
            }

            // Data
            for (int i = 0; i < p.dataLength; i++)
            {
                s += "0x" + String.Format("{0:X02}", ((byte)p.data[i])) + ", ";

            }
            if (s == "")
                s = "no data";

            txtInData.Text = s;
            s = "";

            // Raw
            byte[] buffer = p.ToRawArray();
            for (int i = 0; i < buffer.Length; i++)
            {
                s += "0x" + String.Format("{0:X02}", ((byte)buffer[i])) + ", ";

            }
            if (s == "")
                s = "no data";

            txtInRawData.Text = s;
        }

        bool CmdExpanded = false;
        const int ExpandFactor = 3;
        /// <summary>
        /// Expands the command panel to display more buttons at once.
        /// </summary>
        private void btnExpand_Click(object sender, EventArgs e)
        {
            // calculate vertical size of panel
            int panelHeight = 0;
            if (flowCmdList.Controls.Count > 0)
            {
                panelHeight += flowCmdList.Controls[0].Height;
                panelHeight += flowCmdList.Controls[0].Margin.Top;
                panelHeight += flowCmdList.Controls[0].Margin.Bottom;
                panelHeight *= flowCmdList.Controls.Count;
            }

            // scroll to the top
            flowCmdList.VerticalScroll.Value = 0;

            if (CmdExpanded == false)
            {
                // expand panel
                pnlCmd.Size = new Size((pnlCmd.Size.Width * ExpandFactor), pnlCmd.Size.Height);
                pnlCmd.BringToFront();
                pnlCmdBtn.Size = new Size((pnlCmdBtn.Size.Width * ExpandFactor), pnlCmdBtn.Size.Height);
                flowCmdList.Size = new Size((flowCmdList.Size.Width * ExpandFactor), flowCmdList.Size.Height);
                flowCmdList.ColumnCount = ExpandFactor;
                flowCmdList.ColumnStyles.Clear();
                for(int i=0; i<ExpandFactor; i++)
                    flowCmdList.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100/ExpandFactor));

                // change filter and expand button
                btnExpand.Location = new Point((pnlCmdBtn.Width-btnExpand.Width-5), btnExpand.Location.Y);
                btnExpand.BackgroundImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                btnFilter.Location = new Point((int)(pnlCmd.Size.Width - btnFilter.Size.Width) / 2, btnFilter.Location.Y);

                CmdExpanded = true;
            }
            else
            {
                // change filter and expand button
                btnExpand.BackgroundImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                btnExpand.Location = new Point((pnlCmdBtn.Width/ExpandFactor - btnExpand.Width - 5), btnExpand.Location.Y);
                btnFilter.Location = new Point((int)(pnlCmd.Size.Width / ExpandFactor - btnFilter.Size.Width) / 2, btnFilter.Location.Y);

                // shrink panel
                flowCmdList.Size = new Size((flowCmdList.Size.Width / ExpandFactor), flowCmdList.Size.Height);
                flowCmdList.ColumnCount = 1;
                flowCmdList.ColumnStyles.Clear();
                flowCmdList.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                pnlCmd.Size = new Size((pnlCmd.Size.Width / ExpandFactor), pnlCmd.Size.Height);
                pnlCmdBtn.Size = new Size((pnlCmdBtn.Size.Width / ExpandFactor), pnlCmdBtn.Size.Height);

                CmdExpanded = false;
            }


            flowCmdList.VerticalScroll.Enabled = false;
            flowCmdList.VerticalScroll.Enabled = true;
            flowCmdListCheckForScrolling();
        }

        bool sortAlphabetic = true;
        /// <summary>
        /// Sort all buttons from a to z or reversed.
        /// </summary>
        private void btnSort_Click(object sender, EventArgs e)
        {
            sortAlphabetic = !sortAlphabetic;
            Image newImage;

            if (sortAlphabetic == true)
            {
                CmdList = CmdList.OrderBy(a => a.Text).ToList();
                newImage = Properties.Resources.sort_az;
            }
            else
            {
                CmdList = CmdList.OrderByDescending(a => a.Text).ToList();
                newImage = Properties.Resources.sort_za;
            }
            flowCmdList.Controls.Clear();

            // refill flowLayoutPanel
            foreach (CmdButton c in CmdList)
            {
                flowCmdList.Controls.Add(c);
            }

            Button b = sender as Button;
            b.BackgroundImage = newImage;
        }

        private List<string> FilterTagList = new List<string>();
        /// <summary>
        /// Currently not implemented. Open the filter form.
        /// </summary>
        private void btnFilter_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Update the shown buttons according to the search term.
        /// </summary>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            List<CmdButton> newList = new List<CmdButton>();
            newList = CmdList.FindAll(btn => btn.Text.ToLower().Contains(txtSearch.Text.ToLower()));

            flowCmdList.Controls.Clear();
            foreach (CmdButton cmd in newList)
            {
                flowCmdList.Controls.Add(cmd);
            }
        }

        /// <summary>
        /// Execute the first matching command when return is pressed.
        /// </summary>
        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                foreach (CmdButton cmd in CmdList)
                {
                    if (cmd.Text.ToLower() == txtSearch.Text.ToLower())
                    {
                        cmd.PerformClick();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Open a new port selection window to change ports.
        /// </summary>
        private void btnChangePort_Click(object sender, EventArgs e)
        {
            SerialportForm serialportForm = new SerialportForm();
            serialportForm.FormClosing += new FormClosingEventHandler(serialportChange_FormClosing);
            serialportForm.Show();
            serialportForm.TopMost = true;
        }

        /// <summary>
        /// Changes the port when a new one got selected.
        /// </summary>
        private void serialportChange_FormClosing(object sender, FormClosingEventArgs e)
        {
            SerialportForm spf = sender as SerialportForm;

            if (analyzerIsActive == false)
                serialSession.ChangePort(spf.serialPort.PortName, spf.serialPort.BaudRate);
            else
                anaForm.changeMachinePort(new System.IO.Ports.SerialPort(spf.serialPort.PortName, spf.serialPort.BaudRate));

            lblPort.Text = "Portname: " + spf.serialPort.PortName;
            lblBaud.Text = "Baudrate: " + spf.serialPort.BaudRate;
        }

        eApi.Analyzer.AnalyzerForm anaForm;
        private bool analyzerIsActive = false;
        /// <summary>
        /// Start the analyzer.
        /// </summary>
        private void btnAnalyzerPng_Click(object sender, EventArgs e)
        {
#if Analyzer
            if (serialSession != null)
            {
                serialSession.Pause();
                anaForm = new eApi.Analyzer.AnalyzerForm(serialSession.PortName, serialSession.BaudRate, serialSession);
                anaForm.FormClosing += new FormClosingEventHandler(anaForm_FormClosing);
                lblPort.Text = "Portname: " + serialSession.PortName;
                lblBaud.Text = "Baudrate: " + serialSession.BaudRate;
                anaForm.Show();
                serialSession.StartThread();
                analyzerIsActive = true;
            }
#endif
        }

        /// <summary>
        /// Terminate the analyzer.
        /// </summary>
        void anaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            anaForm.Close();
            serialSession.ChangePort(anaForm.MachinePortName, anaForm.MachineBaudrate);
            serialSession.Start();

            lblPort.Text = "Portname: " + anaForm.MachinePortName;
            lblBaud.Text = "Baudrate: " + anaForm.MachineBaudrate;

            analyzerIsActive = false;
        }

        bool btnConnectionIsDisconnect = true;
        /// <summary>
        /// Open or close the port.
        /// </summary>
        private void btnConnection_Click(object sender, EventArgs e)
        {
            if (btnConnectionIsDisconnect)
            {
                this.CloseSerialPort();
                btnConnection.Text = "Connect";
                btnConnectionIsDisconnect = false;
                lblPort.Text = "Portname: " + "Disconnected";
            }
            else
            {
                this.OpenSerialPort();
                btnConnection.Text = "Disconnect";
                btnConnectionIsDisconnect = true;
                lblPort.Text = "Portname: " + serialSession.PortName;
            }
        }

    }
}
