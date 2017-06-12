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
    public partial class SerialRawPacketField : UserControl
    {
        private int dataLength;
        private byte[] data;
        public float timeInSeconds;

        public SerialRawPacketField(byte[] packetData, int packetDataLength)
        {
            InitializeComponent();
            showTime();
            data = packetData;
            dataLength = packetDataLength;
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
            if (dataLength > 0)
                txtData.Text = BitConverter.ToString(data, 0, dataLength).ToUpper();
            else
                txtData.Text = "no data";
        }
    }
}
