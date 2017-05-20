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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectAirstream
{
    public partial class serialForm : Form
    {
        public serialForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void openPortsBtn_Click(object sender, EventArgs e)
        {
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

            openPortsBtn.Enabled = false;
            prtsOpenLbl.Text = "Open";
            prtsOpenLbl.ForeColor = Color.Black;
        }
    }
}
