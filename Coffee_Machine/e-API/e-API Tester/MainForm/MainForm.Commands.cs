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
    public partial class MainForm : Form
    {
        private delegate void PacketCallback(SerialComm.Packet_t packet);
        PacketCallback ApiDataReceived;

        List<CmdButton> CmdList= new List<CmdButton>();
        object LastRequest = null;
        byte DestAddress = (byte)ModuleId_t.Touch_e;
        public Queue<byte> ignoreSeqNrQueue = new Queue<byte>();

        public void addCmdButton(CmdButton button)
        {
            button.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            CmdList.Add(button);
            flowCmdList.Controls.Add(button);
        }

        public void commandsInit()
        {
            SerialComm.Packet_t p = new SerialComm.Packet_t();
            // Do Product 1
            DoProduct DoProduct1 = new DoProduct();
            DoProduct1.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            CmdList.Add(DoProduct1);

            // GetStatus
            GetStatus GetStatus1 = new GetStatus();
            GetStatus1.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            CmdList.Add(GetStatus1);

            // DoRinse Left
            plainCommand DoRinse1 = new plainCommand(API_Command_t.DoRinse_e, "DoRinse left");
            DoRinse1.packet.message.parameter = 0x00;
            DoRinse1.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            DoRinse1.toolTip = "rinse the machine";
            CmdList.Add(DoRinse1);

            // DoRinse Right
            plainCommand DoRinse2 = new plainCommand(API_Command_t.DoRinse_e, "DoRinse right");
            DoRinse2.packet.message.parameter = 0x01;
            DoRinse2.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            DoRinse2.toolTip = "rinse the machine";
            CmdList.Add(DoRinse2);

            // Start Cleaning
            plainCommand StartCleaning = new plainCommand( API_Command_t.StartCleaning_e, "Start Cleaning" );
            StartCleaning.packet.message.parameter = 0x00;
            StartCleaning.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            StartCleaning.toolTip = "Start Cleaning";
            CmdList.Add( StartCleaning );
#if Briggo
            // Turn On Breakpoints
            plainCommand TurnOnBreakpoints = new plainCommand( API_Command_t.TurnOnBreakpoints_e, "Turn On Breakpoints" );
            TurnOnBreakpoints.packet.message.parameter = 0x01;
            TurnOnBreakpoints.packet.data = new byte[] { 60, 0 };
            TurnOnBreakpoints.packet.dataLength = 2;
            TurnOnBreakpoints.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            TurnOnBreakpoints.toolTip = "Turn On Breakpoints";
            CmdList.Add( TurnOnBreakpoints );

            // Turn Off Breakpoints
            plainCommand TurnOffBreakpoints = new plainCommand( API_Command_t.TurnOnBreakpoints_e, "Turn Off Breakpoints" );
            TurnOffBreakpoints.packet.message.parameter = 0x00;
            TurnOffBreakpoints.packet.data = new byte[] { 60, 0 };
            TurnOffBreakpoints.packet.dataLength = 2;
            TurnOffBreakpoints.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            TurnOffBreakpoints.toolTip = "Turn Off Breakpoints";
            CmdList.Add( TurnOffBreakpoints );

            // Coffee Action Continue Left
            plainCommand CoffeeActionContinueLeft = new plainCommand( API_Command_t.CoffeeActionContinue_e, "Coffee Action Continue Left" );
            CoffeeActionContinueLeft.packet.message.parameter = 0x00;
            CoffeeActionContinueLeft.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            CoffeeActionContinueLeft.toolTip = "Coffee Action Continue Left";
            CmdList.Add( CoffeeActionContinueLeft );

            // Coffee Action Continue Right
            plainCommand CoffeeActionContinueRight = new plainCommand( API_Command_t.CoffeeActionContinue_e, "Coffee Action Continue Right" );
            CoffeeActionContinueRight.packet.message.parameter = 0x01;
            CoffeeActionContinueRight.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            CoffeeActionContinueRight.toolTip = "Coffee Action Continue Right";
            CmdList.Add( CoffeeActionContinueRight );

            // Milk Action Continue Left
            plainCommand MilkActionContinueLeft = new plainCommand( API_Command_t.MilkActionContinue_e, "Milk Action Continue Left" );
            MilkActionContinueLeft.packet.message.parameter = 0x00;
            MilkActionContinueLeft.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            MilkActionContinueLeft.toolTip = "Coffee Action Continue Left";
            CmdList.Add( MilkActionContinueLeft );

            // Milk Action Continue Right
            plainCommand MilkActionContinueRight = new plainCommand( API_Command_t.MilkActionContinue_e, "Milk Action Continue Right" );
            MilkActionContinueRight.packet.message.parameter = 0x01;
            MilkActionContinueRight.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            MilkActionContinueRight.toolTip = "Milk Action Continue Right";
            CmdList.Add( MilkActionContinueRight );
#endif
            // Stop all
            plainCommand StopAll = new plainCommand(API_Command_t.Stop_e, "Stop All");
            StopAll.packet.message.parameter = (ushort)StopModuleType_t.All_e;
            StopAll.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            StopAll.toolTip = "Stop All";
            CmdList.Add(StopAll);

            // Stop coffee left
            plainCommand StopCoffeeL = new plainCommand(API_Command_t.Stop_e, "Stop Coffee Left");
            StopCoffeeL.packet.message.parameter = (ushort)StopModuleType_t.CoffeeLeft_e;
            StopCoffeeL.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            StopCoffeeL.toolTip = "Stop Coffee/Milk Left";
            CmdList.Add(StopCoffeeL);

            // Stop coffee right
            plainCommand StopCoffeeR = new plainCommand(API_Command_t.Stop_e, "Stop Coffee Right");
            StopCoffeeR.packet.message.parameter = (ushort)StopModuleType_t.CoffeeRight_e;
            StopCoffeeR.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            StopCoffeeR.toolTip = "Stop Coffee/Milk Right";
            CmdList.Add(StopCoffeeR);

            // Stop steam left
            plainCommand StopSteamL = new plainCommand( API_Command_t.Stop_e, "Stop Steam Left" );
            StopSteamL.packet.message.parameter = (ushort)StopModuleType_t.SteamLeft_e;
            StopSteamL.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            StopSteamL.toolTip = "Stop Steam Left";
            CmdList.Add(StopSteamL);

            // Stop steam right
            plainCommand StopSteamR = new plainCommand( API_Command_t.Stop_e, "Stop Steam Right" );
            StopSteamR.packet.message.parameter = (ushort)StopModuleType_t.SteamRight_e;
            StopSteamR.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            StopSteamR.toolTip = "Stop Steam Right";
            CmdList.Add(StopSteamR);

            // Stop water
            plainCommand StopWater = new plainCommand( API_Command_t.Stop_e, "Stop Hot Water" );
            StopWater.packet.message.parameter = (ushort)StopModuleType_t.HotWater_e;
            StopWater.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            StopWater.toolTip = "Stop Hot Water";
            CmdList.Add(StopWater);

            // GetRequests
            plainCommand getRequests = new plainCommand( API_Command_t.GetRequests_e, "Get Requests" );
            getRequests.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            getRequests.packet.type = SerialComm.PacketType_t.Request_e;
            getRequests.toolTip = "Get Requests";
            CmdList.Add( getRequests );

#if Briggo
            // Milk Outlet Rinse Left
            plainCommand milkOutletRinseLeft = new plainCommand( API_Command_t.MilkOutletRinse_e, "Milk Outlet Rinse Left" );
            milkOutletRinseLeft.packet.message.parameter = 0x00;
            milkOutletRinseLeft.packet.data = new byte[] { 0, 10, 0 };
            milkOutletRinseLeft.packet.dataLength = 3;
            milkOutletRinseLeft.registerPacketEventHandler( new CmdButton.PacketEventHandler( PacketToSendEventHandler ) );
            milkOutletRinseLeft.toolTip = "Milk Outlet Rinse Left";
            CmdList.Add( milkOutletRinseLeft );

            // Milk Outlet Rinse Right
            plainCommand milkOutletRinseRight = new plainCommand( API_Command_t.MilkOutletRinse_e, "Milk Outlet Rinse Right" );
            milkOutletRinseRight.packet.message.parameter = 0x01;
            milkOutletRinseRight.packet.data = new byte[] { 1, 255, 0 };
            milkOutletRinseRight.packet.dataLength = 3;
            milkOutletRinseRight.registerPacketEventHandler( new CmdButton.PacketEventHandler( PacketToSendEventHandler ) );
            milkOutletRinseRight.toolTip = "Milk Outlet Rinse Right";
            CmdList.Add( milkOutletRinseRight );

            // Screen Rinse Left
            plainCommand screenRinseLeft = new plainCommand( API_Command_t.ScreenRinse_e, "Screen Rinse Left" );
            screenRinseLeft.packet.message.parameter = 0x00;
            screenRinseLeft.packet.data = new byte[] { 2, 5 };
            screenRinseLeft.packet.dataLength = 2;
            screenRinseLeft.registerPacketEventHandler( new CmdButton.PacketEventHandler( PacketToSendEventHandler ) );
            screenRinseLeft.toolTip = "Screen Rinse Left";
            CmdList.Add( screenRinseLeft );
            CmdList.Add( screenRinseLeft );

            // Screen Rinse Right
            plainCommand screenRinseRight = new plainCommand( API_Command_t.ScreenRinse_e, "Screen Rinse Right" );
            screenRinseRight.packet.message.parameter = 0x01;
            screenRinseRight.packet.data = new byte[] { 3, 7 };
            screenRinseRight.packet.dataLength = 2;
            screenRinseRight.registerPacketEventHandler( new CmdButton.PacketEventHandler( PacketToSendEventHandler ) );
            screenRinseRight.toolTip = "Screen Rinse Right";
            CmdList.Add( screenRinseRight );

            // Do ETC Calibration Hopper Front (Left Outlet)
            plainCommand doEtcCalibration = new plainCommand( API_Command_t.DoEtcCalibration_e, "Do ETC Calibration Hopper Front" );
            doEtcCalibration.packet.message.parameter = 0x00;
            doEtcCalibration.packet.data = new byte[] { 0 };
            doEtcCalibration.packet.dataLength = 1;
            doEtcCalibration.registerPacketEventHandler( new CmdButton.PacketEventHandler( PacketToSendEventHandler ) );
            doEtcCalibration.toolTip = "Do ETC Calibration Hopper Front (Left Outlet)";
            CmdList.Add( doEtcCalibration );

            // Do ETC Calibration Hopper Rear (Right Outlet)
            plainCommand doEtcCalibration2 = new plainCommand( API_Command_t.DoEtcCalibration_e, "Do ETC Calibration Hopper Rear" );
            doEtcCalibration2.packet.message.parameter = 0x01;
            doEtcCalibration2.packet.data = new byte[] { 1 };
            doEtcCalibration2.packet.dataLength = 1;
            doEtcCalibration2.registerPacketEventHandler( new CmdButton.PacketEventHandler( PacketToSendEventHandler ) );
            doEtcCalibration2.toolTip = "Do ETC Calibration Hopper Rear (Right Outlet)";
            CmdList.Add( doEtcCalibration2 );

            // Display Action
            plainCommand displayAction = new plainCommand( API_Command_t.DisplayAction_e, "Display Action" );
            displayAction.packet.message.parameter = 0x01;
            displayAction.registerPacketEventHandler( new CmdButton.PacketEventHandler( PacketToSendEventHandler ) );
            displayAction.toolTip = "Display Action";
            CmdList.Add( displayAction );
#else
            // Milk Outlet Rinse Left
            plainCommand milkOutletRinseLeft = new plainCommand( API_Command_t.MilkOutletRinse_e, "Milk Outlet Rinse Left" );
            milkOutletRinseLeft.packet.message.parameter = 0x00;
            milkOutletRinseLeft.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            milkOutletRinseLeft.toolTip = "Milk Outlet Rinse Left";
            CmdList.Add( milkOutletRinseLeft );

            // Milk Outlet Rinse Right
            plainCommand milkOutletRinseRight = new plainCommand( API_Command_t.MilkOutletRinse_e, "Milk Outlet Rinse Right" );
            milkOutletRinseRight.packet.message.parameter = 0x01;
            milkOutletRinseRight.registerPacketEventHandler(new CmdButton.PacketEventHandler(PacketToSendEventHandler));
            milkOutletRinseRight.toolTip = "Milk Outlet Rinse Right";
            CmdList.Add( milkOutletRinseRight );

            // Screen Rinse Left
            plainCommand screenRinseLeft = new plainCommand( API_Command_t.ScreenRinse_e, "Screen Rinse Left" );
            screenRinseLeft.packet.message.parameter = 0x00;
            screenRinseLeft.registerPacketEventHandler(new CmdButton.PacketEventHandler( PacketToSendEventHandler ));
            screenRinseLeft.toolTip = "Screen Rinse Left";
            CmdList.Add( screenRinseLeft );
            CmdList.Add( screenRinseLeft );

            // Screen Rinse Right
            plainCommand screenRinseRight = new plainCommand( API_Command_t.ScreenRinse_e, "Screen Rinse Right" );
            screenRinseRight.packet.message.parameter = 0x01;
            screenRinseRight.registerPacketEventHandler( new CmdButton.PacketEventHandler( PacketToSendEventHandler ) );
            screenRinseRight.toolTip = "Screen Rinse Right";
            CmdList.Add( screenRinseRight );
#endif
            // Get Info Messages
            plainCommand getInfoMessages = new plainCommand( API_Command_t.GetInfoMessages_e, "Get Info Messages" );
            getInfoMessages.registerPacketEventHandler( new CmdButton.PacketEventHandler( PacketToSendEventHandler ) );
            getInfoMessages.packet.type = SerialComm.PacketType_t.Request_e;
            getInfoMessages.toolTip = "Get Info Messages";
            CmdList.Add( getInfoMessages );

            // Get Product Dump
            GetProductDump getProductDump = new GetProductDump( );
            getProductDump.registerPacketEventHandler( new CmdButton.PacketEventHandler( PacketToSendEventHandler ) );
            CmdList.Add( getProductDump );
        }

        void PacketToSendEventHandler(object sender, CmdButton.PacketEventArgs e)
        {
            CmdButton btn = sender as CmdButton;
            bool isRequest = false;
            if (e.packet.type == SerialComm.PacketType_t.Request_e)
            {
                isRequest = true;
                LastRequest = sender;
            }

            serialSession.EnqueuePacket(DestAddress, e.packet.message, e.packet.data, e.packet.dataLength, isRequest);

            e.packet.sequenceNumber = serialSession.LastSequenceNumber;

            if(e.showPacket == false)
                ignoreSeqNrQueue.Enqueue(e.packet.sequenceNumber);

            // Show sucessful send arrow
            //try // temp fix
            //{
                this.BeginInvoke(updateUI, e.packet, CommStatus.PacketSent);
            //}
            //catch { }
        }

        public void showResponse(SerialComm.Packet_t packet)
        {
            CmdButton btn = LastRequest as CmdButton;
            if(btn.processResponse)
                btn.responseReceived(packet);

            // Eingang anzeige aktualisieren
            //try // temp fix
            //{
                this.BeginInvoke(updateUI, packet, CommStatus.ResponseReceived);
            //}
            //catch { }
        }
    }
}