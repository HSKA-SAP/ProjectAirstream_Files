using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ApiSerialComm;

namespace eApi.Analyzer
{
    public struct TelegramStructure
    {
        public byte pip;
        public byte pie;
        public byte pn;
        public byte sa;
        public byte da;

        public byte mi;
        public ushort mp;
        public ushort dl;
        public byte[] data;
        public ushort crc;

        public bool isAckNack;

        public void FromPacket(SerialComm.Packet_t p)
        {
            byte[] buffer = p.ToByteArray();

            pip = buffer[0];
            pie = buffer[1];
            pn = buffer[2];
            sa = buffer[3];
            da = buffer[4];

            if (p.type == SerialComm.PacketType_t.Data_e || p.type == SerialComm.PacketType_t.Request_e)
            {
                mi = buffer[5];
                mp = (ushort)((buffer[7] << 8) + buffer[6]);
                dl = (ushort)((buffer[9] << 8) + buffer[8]);
                if (dl > 0)
                {
                    Array.Resize(ref data, dl);
                    Array.Copy(buffer, 10, data, 0, dl);
                }
                crc = (ushort)((buffer[11 + dl] << 8) + buffer[10 + dl]);
                isAckNack = false;
            }
            else
            {
                mi = 0;
                mp = 0;
                dl = 0;
                crc = 0;
                isAckNack = true;
            }
        }
    }

    public partial class SerialPacketField : UserControl
    {
        public TelegramStructure telegram;
        public float timeInSeconds;

        public SerialPacketField()
        {
            InitializeComponent();
        }

        public SerialPacketField(SerialComm.Packet_t p)
        {
            InitializeComponent();
            showTime();
            telegram.FromPacket(p);
            showTelegram();
        }

        public void showTime()
        {
            DateTime now = DateTime.Now;

            lblTime.Text = now.Hour.ToString("00") + ":" + now.Minute.ToString("00") + ":" + now.Second.ToString("00") + ".";
            lblTime.Text += ((int)(now.Millisecond / 10)).ToString("00");

            timeInSeconds = ((now.Hour * 60 + now.Minute) * 60 + now.Second) + now.Millisecond / 1000;
        }

        public void showTelegram()
        {
            txtPIP.Text = String.Format("{0:x2}",telegram.pip).ToUpper();
            txtPIE.Text = String.Format("{0:x2}", telegram.pie).ToUpper();
            txtPN.Text = String.Format("{0:x2}", telegram.pn).ToUpper();
            txtSA.Text = String.Format("{0:x2}", telegram.sa).ToUpper();
            txtDA.Text = String.Format("{0:x2}", telegram.da).ToUpper();

            if (!telegram.isAckNack)
            {
                txtMI.Text = String.Format("{0:x2}", telegram.mi).ToUpper();
                txtMP.Text = String.Format("{0:x4}", telegram.mp).ToUpper();
                txtDL.Text = String.Format("{0:x4}", telegram.dl).ToUpper();
                txtCRC.Text = String.Format("{0:x4}", telegram.crc).ToUpper();
            }
            else
            {
                txtMI.Text = "--";
                txtMP.Text = "--";
                txtDL.Text = "--";
                txtCRC.Text = "----";
            }

            if(telegram.dl>0)
                txtData.Text = BitConverter.ToString(telegram.data, 0, telegram.dl).ToUpper();
            else
                txtData.Text = "no data";
        }
    }
}
