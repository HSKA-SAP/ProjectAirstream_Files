using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ApiSerialComm;
using eApi.Typedef;
using eApi.Commands;

namespace eApi.Commands.GetStatusForm
{
    public partial class ResponseForm : Form
    {
        #region Strings for Status,Action and Process
        private readonly string[] statusArr =
        {
            "not ready",
            "ready",
            "undef"
        };
        private string getStatusString(Status_t s)
        {
            byte index = (byte)s;
            if (index <= 2)
                return statusArr[index];
            else
                return statusArr[2];
        }

        private readonly string[] actionArr =
        {
            "idle",
            "queued",
            "suspended",
            "ending",
            "end cycle",
            "stoped",
            "started",
            "pumping",
            "milk interrupt",
            "cycle aborted",
            "powder chute",
            "clean tabs",
            "undef"
        };
        private string getActionString(Actions_t a)
        {
            byte index = (byte)a;
            if (index <= 12)
                return actionArr[index];
            else
                return actionArr[12];
        }

        private readonly string[] processArr =
        {
            "coffee",
            "steam",
            "hot water",
            "learn water quantity",
            "powder test",
            "clean",
            "rinse",
            "screen rinse",
            "service position",
            "depressurize",
            "empty boiler",
            "adjust pump press",
            "flow meter test",
            "grinder sensor test",
            "MotIni",    // ???
            "MotIniRebootAbort", // ???
            "brew move test",   // correct?
            "milk clean",
            "outlet rinse",
            "empty coffee boiler",
            "grinder adjust menu",
            "test ball dispenser",
            "test milk pump",
            "milk reactor warmup",
            "reduce pressure",
            "test security valve",
            "dispense ball",
            "milk detection test",
            "brew tightness test",
            "undef"
        };
        private string getProcessString(Process_t p)
        {
            byte index = (byte)p;
            if (index <= 29)
                return processArr[index];
            else
                return processArr[29];
        }

        #region Briggo
        private readonly string[] actionCoffeeArr =
        {
            "undef",
            "grinding",
            "wait after grinding",
            "tamping and pre-brew",
            "dispensing",
            "wait after dispensing",
            "press and drop cake"
        };
        private string getActionCoffeeString(byte index)
        {
            if (index <= 6)
                return actionCoffeeArr[index];
            else
                return actionCoffeeArr[0];
        }

        private readonly string[] actionMilkArr =
        {
            "undef",
            "wait for suction",
            "suction and detection",
            "rinse after dispensing"
        };
        private string getActionMilkString(byte index)
        {
            if (index <= 3)
                return actionMilkArr[index];
            else
                return actionMilkArr[0];
        }
        #endregion
        #endregion

        public GetStatusResponse_t response;
        private GetStatus ParentButton;
        private SerialComm.Packet_t stopPacket;

        public ResponseForm(GetStatus parentButton)
        {
            InitializeComponent();
            ParentButton = parentButton;

            this.FormClosing += new FormClosingEventHandler(ResponseForm_FormClosing);

            stopPacket = new SerialComm.Packet_t();
            stopPacket.message.command = (byte)API_Command_t.Stop_e;
            stopPacket.destination = (byte)ModuleId_t.Touch_e;
            stopPacket.source = (byte)ModuleId_t.Api_e;
            stopPacket.appPort = SerialComm.ApplicationPort_t.Api_e;
            stopPacket.type = SerialComm.PacketType_t.Data_e;

            cbxInterval.SelectedIndex = 3;

            if (btnStopLeft.Visible == false)
            {
                pnlCoffeeMilkLeft.Size = new Size(pnlCoffeeMilkLeft.Size.Width, pnlCoffeeMilkLeft.Size.Height - 33);
                pnlCoffeeMilkRight.Size = new Size(pnlCoffeeMilkRight.Size.Width, pnlCoffeeMilkRight.Size.Height - 33);
                pnlSteamLeft.Size = new Size(pnlSteamLeft.Size.Width, pnlSteamLeft.Size.Height - 33);
                pnlSteamRight.Size = new Size(pnlSteamRight.Size.Width, pnlSteamRight.Size.Height - 33);
                pnlWater.Size = new Size(pnlWater.Size.Width, pnlWater.Size.Height - 33);
            }

#if !Briggo
            //lblDetailedBriggo.Visible = false;
            lblCoffeeLeftBriggo.Visible = false;
            lblCoffeeRightBriggo.Visible = false;
            lblMilkLeftBriggo.Visible = false;
            lblMilkRightBriggo.Visible = false;
            txtCoffeeLeftBriggo.Visible = false;
            txtCoffeeRightBriggo.Visible = false;
            txtMilkLeftBriggo.Visible = false;
            txtMilkRightBriggo.Visible = false;
#endif
        }

        public GetStatusResponse_t updateStatus(SerialComm.Packet_t p)
        {
            // cast packet p into response
            processResponse(p);

            // Machine
            txtMachineStatus.Text = toBinaryString(response.machineStatus);

            // Coffee Left
            txtActionLeft.Text = getActionString(response.coffeeL.action);
            txtStatusLeft.Text = getStatusString(response.coffeeL.status);
            txtProcessLeft.Text = getProcessString(response.coffeeL.process);
            txtStatusByteLeft.Text = toBinaryString((byte)response.coffeeL.status);
            txtProcessByteLeft.Text = toBinaryString((byte)response.coffeeL.process);

            // Coffee Right
            txtActionRight.Text = getActionString(response.coffeeR.action);
            txtStatusRight.Text = getStatusString(response.coffeeR.status);
            txtProcessRight.Text = getProcessString(response.coffeeR.process);
            txtStatusByteRight.Text = toBinaryString((byte)response.coffeeR.status);
            txtProcessByteRight.Text = toBinaryString((byte)response.coffeeR.process);

            // Steam Left
            txtActionSteamLeft.Text = getActionString(response.steamL.action);
            txtStatusSteamLeft.Text = getStatusString(response.steamL.status);
            txtProcessSteamLeft.Text = getProcessString(response.steamL.process);
            txtStatusByteSteamLeft.Text = toBinaryString((byte)response.steamL.status);
            txtProcessByteSteamLeft.Text = toBinaryString((byte)response.steamL.process);

            // Steam Right
            txtActionSteamRight.Text = getActionString(response.steamR.action);
            txtStatusSteamRight.Text = getStatusString(response.steamR.status);
            txtProcessSteamRight.Text = getProcessString(response.steamR.process);
            txtStatusByteSteamRight.Text = toBinaryString((byte)response.steamR.status);
            txtProcessByteSteamRight.Text = toBinaryString((byte)response.steamR.process);

            // Hot Water
            txtActionWater.Text = getActionString(response.water.action);
            txtStatusWater.Text = getStatusString(response.water.status);
            txtProcessWater.Text = getProcessString(response.water.process);
            txtStatusByteWater.Text = toBinaryString((byte)response.water.status);
            txtProcessByteWater.Text = toBinaryString((byte)response.water.process);

#if Briggo
            if (response.remainder.Length >= 2)
            {
                txtCoffeeLeftBriggo.Text = getActionCoffeeString((byte)(response.remainder[0] >> 4));
                txtCoffeeRightBriggo.Text = getActionCoffeeString((byte)(response.remainder[0] & 0x0F));

                txtMilkLeftBriggo.Text = getActionMilkString((byte)(response.remainder[1] >> 4));
                txtMilkRightBriggo.Text = getActionMilkString((byte)(response.remainder[1] & 0x0F));
            }
            else
            {
                txtCoffeeLeftBriggo.Text = "check machine";
                txtCoffeeRightBriggo.Text = "software version";
            }
#endif

            return response;
        }

        private void processResponse(SerialComm.Packet_t p)
        {

            // Machine
            response.machineStatus = p.data[0];

            // Coffee Left
            response.coffeeL.status = (Status_t)(p.data[1] & 0x0F);
            response.coffeeL.action = (Actions_t)(p.data[1] >> 4);
            response.coffeeL.process = (Process_t)p.data[6];

            // Coffee Right
            response.coffeeR.status = (Status_t)(p.data[2] & 0x0F);
            response.coffeeR.action = (Actions_t)(p.data[2] >> 4);
            response.coffeeR.process = (Process_t)p.data[7];

            // Steam Left
            response.steamL.status = (Status_t)(p.data[3] & 0x0F);
            response.steamL.action = (Actions_t)(p.data[3] >> 4);
            response.steamL.process = (Process_t)p.data[8];

            // Steam Right
            response.steamR.status = (Status_t)(p.data[4] & 0x0F);
            response.steamR.action = (Actions_t)(p.data[4] >> 4);
            response.steamR.process = (Process_t)p.data[9];

            // Hot Water
            response.water.status = (Status_t)(p.data[5] & 0x0F);
            response.water.action = (Actions_t)(p.data[5] >> 4);
            response.water.process = (Process_t)p.data[10];

            if (p.dataLength > 11)
            {
                Array.Resize(ref response.remainder, p.dataLength - 11);
                Array.Copy(p.data, 11, response.remainder, 0, p.dataLength - 11);
            }
        }

        private string toBinaryString(byte b)
        {
            string s = "";
            s = Convert.ToString(b, 2);
            while( s.Length < 8)
            {
                s = "0" + s;
            }
            return s;
        }

        private void btnStopLeft_Click(object sender, EventArgs e)
        {
            stopPacket.message.parameter = (byte)StopModuleType_t.CoffeeLeft_e;
            ParentButton.addPacketEvent(ParentButton, new CmdButton.PacketEventArgs(stopPacket));
        }

        private void btnStopRight_Click(object sender, EventArgs e)
        {
            stopPacket.message.parameter = (byte)StopModuleType_t.CoffeeRight_e;
            ParentButton.addPacketEvent(ParentButton, new CmdButton.PacketEventArgs(stopPacket));
        }

        private void btnStopSteamLeft_Click(object sender, EventArgs e)
        {
            stopPacket.message.parameter = (byte)StopModuleType_t.SteamLeft_e;
            ParentButton.addPacketEvent(ParentButton, new CmdButton.PacketEventArgs(stopPacket));
        }

        private void btnStopSteamRight_Click(object sender, EventArgs e)
        {
            stopPacket.message.parameter = (byte)StopModuleType_t.SteamRight_e;
            ParentButton.addPacketEvent(ParentButton, new CmdButton.PacketEventArgs(stopPacket));
        }

        private void btnStopWater_Click(object sender, EventArgs e)
        {
            stopPacket.message.parameter = (byte)StopModuleType_t.HotWater_e;
            ParentButton.addPacketEvent(ParentButton, new CmdButton.PacketEventArgs(stopPacket));
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ParentButton.addPacketEvent(ParentButton, new CmdButton.PacketEventArgs(ParentButton.packet));
        }


        Timer automaticUpdate;
        private void btnStartAuto_Click(object sender, EventArgs e)
        {
            if (automaticUpdate == null)
            {
                btnStartAuto.Text = "stop";
                automaticUpdate = new Timer();
                switch (cbxInterval.SelectedIndex)
                {
                    case 0: // 0.5s
                        automaticUpdate.Interval = 500;
                        break;
                    case 1: // 1.0s
                        automaticUpdate.Interval = 1000;
                        break;
                    case 2: // 2.0s
                        automaticUpdate.Interval = 2000;
                        break;
                    case 3: // 5.0s
                    default:
                        automaticUpdate.Interval = 5000;
                        break;
                }
                automaticUpdate.Tick += new EventHandler(automaticUpdate_Tick);
                automaticUpdate.Start();
            }
            else
            {
                automaticUpdate.Stop();
                automaticUpdate = null;
                btnStartAuto.Text = "start";
            }
        }

        void automaticUpdate_Tick(object sender, EventArgs e)
        {
            ParentButton.addPacketEvent(ParentButton, new CmdButton.PacketEventArgs(ParentButton.packet, false));
        }

        private void cbxInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (automaticUpdate != null)
            {
                switch (cbxInterval.SelectedIndex)
                {
                    case 0: // 0.5s
                        automaticUpdate.Interval = 500;
                        break;
                    case 1: // 1.0s
                        automaticUpdate.Interval = 1000;
                        break;
                    case 2: // 2.0s
                        automaticUpdate.Interval = 2000;
                        break;
                    case 3: // 5.0s
                    default:
                        automaticUpdate.Interval = 5000;
                        break;
                }
            }
        }

        void ResponseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (automaticUpdate != null)
            {
                automaticUpdate.Stop();
                automaticUpdate = null;
            }
        }
    }
}
