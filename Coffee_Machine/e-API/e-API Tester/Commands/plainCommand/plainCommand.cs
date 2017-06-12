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
using eApi.Typedef;

namespace eApi.Commands
{
    public class plainCommand : CmdButton
    {
        public plainCommand(API_Command_t commandType, string description)
        {
            SerialComm.Packet_t packet = new SerialComm.Packet_t();
            this.Click += new EventHandler(OnButtonClick);

            this.Text = description;

            packet.destination = (byte)ModuleId_t.Touch_e;
            packet.source = (byte)ModuleId_t.Api_e;
            packet.appPort = SerialComm.ApplicationPort_t.Api_e;
            packet.type = SerialComm.PacketType_t.Data_e;
            packet.message.command = (byte)commandType;
            this.packet = packet;

            this.processResponse = false;
            this.showCommandForm = false;
            this.showResponseForm = false;
        }

        public override void OnButtonClick(object sender, EventArgs e)
        {
            addPacketEvent(this, new PacketEventArgs(packet));
        }

        public override void responseReceived(SerialComm.Packet_t p)
        {
        }
    }
}
