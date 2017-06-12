using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ApiSerialComm;
using eApi;
using eApi.Commands.GetStatusForm;
using eApi.Typedef;

namespace eApi.Commands
{
    public class GetStatus : CmdButton
    {
        public class StatusEventArgs : EventArgs
        {
            public StatusEventArgs(GetStatusResponse_t resp)
            {
                response = resp;
            }
            public GetStatusResponse_t response;
        }
        public delegate void StatusEventHandler(object sender, StatusEventArgs e);

        public event StatusEventHandler StatusReceivedEvent;
        public GetStatusResponse_t lastResponse;
        private ResponseForm form;
        private bool formWasClosed = false;

        public GetStatus()
        {
            SerialComm.Packet_t packet = new SerialComm.Packet_t();
            lastResponse = new GetStatusResponse_t();
            this.Click += new EventHandler(OnButtonClick);

            this.Text = "GetStatus";
            this.toolTip = "get the current status";
            packet.destination = (byte)ModuleId_t.Touch_e;
            packet.source = (byte)ModuleId_t.Api_e;
            packet.appPort = SerialComm.ApplicationPort_t.Api_e;
            packet.type = SerialComm.PacketType_t.Request_e;
            packet.message.command = (byte)API_Command_t.GetStatus_e;
            this.packet = packet;
            this.processResponse = true;
            this.showResponseForm = true;
        }

        public override void OnButtonClick(object sender, EventArgs e)
        {
            addPacketEvent(this, new PacketEventArgs(packet));
        }

        public override void responseReceived(SerialComm.Packet_t p)
        {
            if (form == null || formWasClosed == true)
            {
                formWasClosed = false;
                form = new ResponseForm(this);
                form.FormClosing += new FormClosingEventHandler(form_FormClosing);
                if (showResponseForm == true)
                {
                    form.WindowState = FormWindowState.Normal;
                    form.Show();
                }
            }
            if (showResponseForm == true)
            {
                //form.WindowState = FormWindowState.Normal;
                //form.TopMost = true;
                form.Show();
                counter++;
            }

            lastResponse = form.updateStatus(p);
            form.updateStatus(p);

            if(StatusReceivedEvent != null)
                StatusReceivedEvent(this, new StatusEventArgs(lastResponse));
        }
        private decimal counter = 0;

        void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            formWasClosed = true;
        }
    }
}
