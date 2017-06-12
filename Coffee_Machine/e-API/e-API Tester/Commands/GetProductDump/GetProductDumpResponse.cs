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

namespace eApi.Commands.GetProductDumpForm
{
    public partial class ProductDumpForm : Form
    {
        public ProcDump_t response = new ProcDump_t( );
        private GetProductDump ParentButton;

        public ProductDumpForm( GetProductDump parentButton )
        {
            InitializeComponent();
            ParentButton = parentButton;
        }

        public ProcDump_t updateStatus(SerialComm.Packet_t p)
        {
            string str = "";

            if( p.dataLength > 0 )
            {
                // cast packet p into response
                processResponse( p );

                str = response.date.ToString( ) + " / ";
                str += response.time.ToString( ) + "\n";
                str += "Cake Press Before: " + ( response.cakePressBefore / 10.0 ).ToString( "0.0" ) + " mm\n";
                str += "Cake Press After: " + ( response.cakePressAfter / 10.0 ).ToString( "0.0" ) + " mm\n";
                str += "Cake Press Hub: " + ( response.cakePressHub / 10.0 ).ToString( "0.0" ) + " mm\n";
                str += "Cake Press Final: " + ( response.cakePressFinal / 10.0 ).ToString( "0.0" ) + " mm\n";
                str += "Brew Side: " + response.side.ToString( ) + "\n";
                str += "PQC: " + response.pwdrQntyCtrl.ToString( ) + "\n";
                str += "Grind Time: " + ( response.powderQty / 10.0 ).ToString( "0.0" ) + " s\n";
                str += "Extract Time: " + ( response.extractTime / 10.0 ).ToString( "0.0" ) + " s\n";
                str += "Water Qnty: " + response.waterQnty.ToString( ) + " ticks\n";
                str += "Water Temp: " + response.waterTemp.ToString( ) + " °C\n";
                str += "Product Type: " + response.prodType.ToString( ) + "\n";
                str += "Double Prod: " + response.doubleProd.ToString( ) + "\n";
                str += "Key Number: " + ( response.keyId + 1 ).ToString( ) + "\n";
                str += "Bean Hopper: " + response.beanHopp.ToString( ) + "\n";
                str += "Outlet Side: " + response.outSide.ToString( ) + "\n";
                str += "Stopped: " + response.prodAbort.ToString( ) + "\n";
                str += "Grind Adjust Left: " + ( ( short )response.grindAdjustLeft ).ToString( ) + "\n";
                str += "Grind Adjust Right: " + ( ( short )response.grindAdjustRight ).ToString( ) + "\n";
                str += "Ref Extract Time: " + response.refExtractTime.ToString( ) + "\n";
                str += "Milk Temp: " + response.milkTemp.ToString( ) + " °C\n";
                str += "Steam Press: " + ( response.steamPress / 10.0 ).ToString( "0.0" ) + " bar\n";
                str += "Milk Time: " + ( response.milkTime / 10.0 ).ToString( "0.0" ) + " s\n";
                str += "RPM Foam: " + response.rpmFoam.ToString( ) + "\n";
                str += "RPM Milk: " + response.rpmMilk.ToString( ) + "\n";

                if( p.dataLength > 42 )
                {
                    str += "Boiler Temp: " + response.boilerTemp.ToString( ) + " °C\n";
                    str += "Milk Control Temp Foam: " + response.mctTempFoam.ToString( ) + " °C\n";
                    str += "Milk Control Temp Milk: " + response.mctTempMilk.ToString( ) + " °C\n";
                    str += "Milk Control Time Foam: " + ( response.mctTimeFoam / 10.0 ).ToString( "0.0" ) + " s\n";
                    str += "Milk Control Time Milk: " + ( response.mctTimeMilk / 10.0 ).ToString( "0.0" ) + " s\n";
                    str += "Milk Input Temp: " + response.milkInputTemp.ToString( ) + " °C\n";
                    str += "Air Quantity: " + response.airQuantity.ToString( ) + " %\n";
                }
            }

            lblProcDump.Text = str;

            return response;
        }

        private void processResponse(SerialComm.Packet_t p)
        {
            response.FromArrayApi( p.data );
        }

    }
}
