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
using eApi.Commands.DoProductForm;
using eApi.Typedef;

namespace eApi.Commands
{
    public class DoProduct : CmdButton
    {
        public ProductParameter_t[] defaultProductParams;
        CommandForm form;

        public DoProduct()
        {
            SerialComm.Packet_t packet = new SerialComm.Packet_t();
            this.Click += new EventHandler(OnButtonClick);

            this.Text = "DoProduct";
            this.toolTip = "make a product";
            packet.destination = (byte)ModuleId_t.Touch_e;
            packet.source = (byte)ModuleId_t.Api_e;
            packet.appPort = SerialComm.ApplicationPort_t.Api_e;
            packet.type = SerialComm.PacketType_t.Data_e;
            packet.message.command = (byte)API_Command_t.DoProduct_e;
            this.packet = packet;

            defaultProductParams = new ProductParameter_t[(int)ProductType_t.Max_e * 2];

            // Produktparameter-Klassen für die Defaultprodukte erstellen
            for (int i = 0; i < (int)ProductType_t.Max_e * 2; i++)
            {
                defaultProductParams[i] = new ProductParameter_t();
            }

            InitDefaultProductParams(ref defaultProductParams);
        }

        public override void OnButtonClick(object sender, EventArgs e)
        {
            if (showCommandForm == true)
            {
                form = new CommandForm(ref defaultProductParams, this.packet);
                form.Show();
                form.TopMost = true;

                form.FormClosing += new FormClosingEventHandler(OnDoProductClosing);
            }
            else
            {
                addPacketEvent(this, new PacketEventArgs(this.packet));
            }
        }

        public override void responseReceived(SerialComm.Packet_t p)
        {

        }

        void OnDoProductClosing(object sender, EventArgs e)
        {
            CommandForm form = sender as CommandForm;
            if (form.packet.dataLength > 0)
            {
                this.packet.data = form.packet.data;
                this.packet.dataLength = form.packet.dataLength;
                this.packet.message.parameter = form.packet.message.parameter;
                addPacketEvent(this, new PacketEventArgs(this.packet));
            }
        }

        public static void InitDefaultProductParams(ref ProductParameter_t[] prodParam)
        {
            int prodTypeNbr = (int)ProductType_t.None_e;
            int doubleProductOffset = (int)ProductType_t.Max_e;

            // None Parameters
            prodTypeNbr = (int)ProductType_t.None_e;
            prodParam[prodTypeNbr].productType = ProductType_t.None_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcUndef_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempUndef_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 0;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Undef_e;
            prodParam[prodTypeNbr].cakeThickness = 0.0;
            prodParam[prodTypeNbr].powderPress = 0;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].prebrewTime = 0;
            prodParam[prodTypeNbr].relaxTime = 0;
            prodParam[prodTypeNbr].pressAfter = 0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortUndef_e;
            prodParam[prodTypeNbr].milkQnty = 0;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqUndef_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].waterQnty = 0;
            prodParam[prodTypeNbr].iconNbr = 0;
            prodParam[prodTypeNbr].doubleProduct = false;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].referenceKey = true;
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;

            // Ristretto Parameters
            prodTypeNbr = (int)ProductType_t.Ristretto_e;
            prodParam[prodTypeNbr].productType = ProductType_t.Ristretto_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].doubleProduct = false;
            prodParam[prodTypeNbr].waterQnty = 40;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Front_e;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutCentral_e;
            prodParam[prodTypeNbr].cakeThickness = 14;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].prebrewTime = 0.8;
            prodParam[prodTypeNbr].relaxTime = 2.0;
            prodParam[prodTypeNbr].pressAfter = 2.0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortUndef_e;
            prodParam[prodTypeNbr].milkQnty = 0;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempUndef_e;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqUndef_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].iconNbr = 6;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Double Ristretto Parameters
            prodTypeNbr = (int)ProductType_t.Ristretto_e + doubleProductOffset;
            prodParam[prodTypeNbr].productType = ProductType_t.Ristretto_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].doubleProduct = true;
            prodParam[prodTypeNbr].waterQnty = 60;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Front_e;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutCentral_e;
            prodParam[prodTypeNbr].cakeThickness = 21.0;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].prebrewTime = 0.0;
            prodParam[prodTypeNbr].relaxTime = 0.0;
            prodParam[prodTypeNbr].pressAfter = -2.0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortUndef_e;
            prodParam[prodTypeNbr].milkQnty = 0;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempUndef_e;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqUndef_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].iconNbr = 6;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Espresso Parameters
            prodTypeNbr = (int)ProductType_t.Espresso_e;
            prodParam[prodTypeNbr].productType = ProductType_t.Espresso_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempUndef_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Front_e;
            prodParam[prodTypeNbr].cakeThickness = 14;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].prebrewTime = 0.8;
            prodParam[prodTypeNbr].relaxTime = 2.0;
            prodParam[prodTypeNbr].pressAfter = 2.0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortUndef_e;
            prodParam[prodTypeNbr].milkQnty = 0;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqUndef_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].waterQnty = 50;
            prodParam[prodTypeNbr].iconNbr = 6;
            prodParam[prodTypeNbr].doubleProduct = false;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].referenceKey = true;
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Double Espresso Parameters
            prodTypeNbr = (int)ProductType_t.Espresso_e + doubleProductOffset;
            prodParam[prodTypeNbr].productType = ProductType_t.Espresso_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempUndef_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Front_e;
            prodParam[prodTypeNbr].cakeThickness = 21.0;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].prebrewTime = 0;
            prodParam[prodTypeNbr].relaxTime = 0;
            prodParam[prodTypeNbr].pressAfter = -2.0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortUndef_e;
            prodParam[prodTypeNbr].milkQnty = 0;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqUndef_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].waterQnty = 80;
            prodParam[prodTypeNbr].iconNbr = 6;
            prodParam[prodTypeNbr].doubleProduct = true;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].referenceKey = true;
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Coffee Parameters
            prodTypeNbr = (int)ProductType_t.Coffee_e;
            prodParam[prodTypeNbr].productType = ProductType_t.Coffee_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempUndef_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Rear_e;
            prodParam[prodTypeNbr].cakeThickness = 14.0;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].prebrewTime = 0.0;
            prodParam[prodTypeNbr].relaxTime = 0.0;
            prodParam[prodTypeNbr].pressAfter = 0.0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortUndef_e;
            prodParam[prodTypeNbr].milkQnty = 0;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqUndef_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].waterQnty = 135;
            prodParam[prodTypeNbr].iconNbr = 61;
            prodParam[prodTypeNbr].doubleProduct = false;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].referenceKey = true;
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Double Coffee Parameters
            prodTypeNbr = (int)ProductType_t.Coffee_e + doubleProductOffset;
            prodParam[prodTypeNbr].productType = ProductType_t.Coffee_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempUndef_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Rear_e;
            prodParam[prodTypeNbr].cakeThickness = 21.0;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 60;
            prodParam[prodTypeNbr].prebrewTime = 0;
            prodParam[prodTypeNbr].relaxTime = 0;
            prodParam[prodTypeNbr].pressAfter = -2.0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortUndef_e;
            prodParam[prodTypeNbr].milkQnty = 0;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqUndef_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].waterQnty = 270;
            prodParam[prodTypeNbr].iconNbr = 61;
            prodParam[prodTypeNbr].doubleProduct = true;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].referenceKey = true;
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Filter Coffee Parameters
            prodTypeNbr = (int)ProductType_t.FilterCoffee_e;
            prodParam[prodTypeNbr].productType = ProductType_t.FilterCoffee_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].doubleProduct = false;
            prodParam[prodTypeNbr].waterQnty = 135;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Rear_e;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutCentral_e;
            prodParam[prodTypeNbr].cakeThickness = 14.0;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].prebrewTime = 0.0;
            prodParam[prodTypeNbr].relaxTime = 0.0;
            prodParam[prodTypeNbr].pressAfter = -5.0;
            prodParam[prodTypeNbr].bypassQnty = 50;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortUndef_e;
            prodParam[prodTypeNbr].milkQnty = 0;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempUndef_e;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqUndef_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].iconNbr = 72;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Double Filter Coffee Parameters
            prodTypeNbr = (int)ProductType_t.FilterCoffee_e + doubleProductOffset;
            prodParam[prodTypeNbr].productType = ProductType_t.FilterCoffee_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].doubleProduct = true;
            prodParam[prodTypeNbr].waterQnty = 270;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Rear_e;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutCentral_e;
            prodParam[prodTypeNbr].cakeThickness = 21.0;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].prebrewTime = 0.0;
            prodParam[prodTypeNbr].relaxTime = 0.0;
            prodParam[prodTypeNbr].pressAfter = -5.0;
            prodParam[prodTypeNbr].bypassQnty = 60;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortUndef_e;
            prodParam[prodTypeNbr].milkQnty = 0;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempUndef_e;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqUndef_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].iconNbr = 72;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Americano Parameters
            prodTypeNbr = (int)ProductType_t.Americano_e;
            prodParam[prodTypeNbr].productType = ProductType_t.Americano_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].doubleProduct = false;

            //prodParam[prodTypeNbr].waterQnty = 50;
            //prodParam[prodTypeNbr].hotWaterQnty = 150;
            //prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].waterQnty = 200;
            prodParam[prodTypeNbr].bypassQnty = 70;

            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Front_e;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutCentral_e;
            prodParam[prodTypeNbr].cakeThickness = 14.0;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].prebrewTime = 0.8;
            prodParam[prodTypeNbr].relaxTime = 2.0;
            prodParam[prodTypeNbr].pressAfter = 2.0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortUndef_e;
            prodParam[prodTypeNbr].milkQnty = 0;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempUndef_e;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqUndef_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].iconNbr = 3;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Double Americano Parameters
            prodTypeNbr = (int)ProductType_t.Americano_e + doubleProductOffset;
            prodParam[prodTypeNbr].productType = ProductType_t.Americano_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].doubleProduct = true;

            //prodParam[prodTypeNbr].waterQnty = 80;
            //prodParam[prodTypeNbr].hotWaterQnty = 320;
            //prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].waterQnty = 400;
            prodParam[prodTypeNbr].bypassQnty = 80;

            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Front_e;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutCentral_e;
            prodParam[prodTypeNbr].cakeThickness = 21.0;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].prebrewTime = 0.0;
            prodParam[prodTypeNbr].relaxTime = 0.0;
            prodParam[prodTypeNbr].pressAfter = -2.0; ;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortUndef_e;
            prodParam[prodTypeNbr].milkQnty = 0;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempUndef_e;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqUndef_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].iconNbr = 3;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Coffee Pot Parameters
            prodTypeNbr = (int)ProductType_t.CoffeePot_e;
            prodParam[prodTypeNbr].productType = ProductType_t.CoffeePot_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].doubleProduct = false;
            prodParam[prodTypeNbr].waterQnty = 350;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Rear_e;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutPot_e;
            prodParam[prodTypeNbr].cakeThickness = 21.0;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].prebrewTime = 0.0;
            prodParam[prodTypeNbr].relaxTime = 0.0;
            prodParam[prodTypeNbr].pressAfter = -3.0;
            prodParam[prodTypeNbr].bypassQnty = 70;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortUndef_e;
            prodParam[prodTypeNbr].milkQnty = 0;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempUndef_e;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqUndef_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].iconNbr = 70;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Filter Coffee Pot Parameters
            prodTypeNbr = (int)ProductType_t.FilterCoffeePot_e;
            prodParam[prodTypeNbr].productType = ProductType_t.FilterCoffeePot_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].doubleProduct = false;
            prodParam[prodTypeNbr].waterQnty = 350;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Rear_e;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutPot_e;
            prodParam[prodTypeNbr].cakeThickness = 21.0;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].prebrewTime = 0.0;
            prodParam[prodTypeNbr].relaxTime = 0.0;
            prodParam[prodTypeNbr].pressAfter = -5.0;
            prodParam[prodTypeNbr].bypassQnty = 70;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortUndef_e;
            prodParam[prodTypeNbr].milkQnty = 0;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempUndef_e;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqUndef_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].iconNbr = 71;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Hot Water Parameters
            prodTypeNbr = (int)ProductType_t.HotWater_e;
            prodParam[prodTypeNbr].productType = ProductType_t.HotWater_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcWater_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempUndef_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 0;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Undef_e;
            prodParam[prodTypeNbr].cakeThickness = 0.0;
            prodParam[prodTypeNbr].powderPress = 0;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].prebrewTime = 0;
            prodParam[prodTypeNbr].relaxTime = 0;
            prodParam[prodTypeNbr].pressAfter = 0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortUndef_e;
            prodParam[prodTypeNbr].milkQnty = 0;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqUndef_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].waterQnty = 100;
            prodParam[prodTypeNbr].iconNbr = 7;
            prodParam[prodTypeNbr].doubleProduct = false;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;

            // Manual Steam Parameters
            prodTypeNbr = (int)ProductType_t.ManualSteam_e;
            prodParam[prodTypeNbr].productType = ProductType_t.ManualSteam_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcSteam_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempUndef_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 0;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Undef_e;
            prodParam[prodTypeNbr].cakeThickness = 0.0;
            prodParam[prodTypeNbr].powderPress = 0;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].prebrewTime = 0;
            prodParam[prodTypeNbr].relaxTime = 0;
            prodParam[prodTypeNbr].pressAfter = 0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortUndef_e;
            prodParam[prodTypeNbr].milkQnty = 0;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqUndef_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 90;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].waterQnty = 0;
            prodParam[prodTypeNbr].iconNbr = 4;
            prodParam[prodTypeNbr].doubleProduct = false;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;

            // Auto Steam Parameters
            prodTypeNbr = (int)ProductType_t.AutoSteam_e;
            prodParam[prodTypeNbr].productType = ProductType_t.AutoSteam_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcSteam_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempUndef_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 0;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Undef_e;
            prodParam[prodTypeNbr].cakeThickness = 0.0;
            prodParam[prodTypeNbr].powderPress = 0;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].prebrewTime = 0;
            prodParam[prodTypeNbr].relaxTime = 0;
            prodParam[prodTypeNbr].pressAfter = 0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortUndef_e;
            prodParam[prodTypeNbr].milkQnty = 0;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqUndef_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 65;
            prodParam[prodTypeNbr].waterQnty = 0;
            prodParam[prodTypeNbr].iconNbr = 43;
            prodParam[prodTypeNbr].doubleProduct = false;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;

            // Everfoam Parameters
            prodTypeNbr = (int)ProductType_t.Everfoam_e;
            prodParam[prodTypeNbr].productType = ProductType_t.Everfoam_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcSteam_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempUndef_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 0;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Undef_e;
            prodParam[prodTypeNbr].cakeThickness = 0.0;
            prodParam[prodTypeNbr].powderPress = 0;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].prebrewTime = 0;
            prodParam[prodTypeNbr].relaxTime = 0;
            prodParam[prodTypeNbr].pressAfter = 0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortUndef_e;
            prodParam[prodTypeNbr].milkQnty = 0;
            prodParam[prodTypeNbr].milkPercent = 40;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqUndef_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 5;
            prodParam[prodTypeNbr].steamTemp = 65;
            prodParam[prodTypeNbr].waterQnty = 0;
            prodParam[prodTypeNbr].iconNbr = 78;
            prodParam[prodTypeNbr].doubleProduct = false;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].airStopTemp = 40;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;

            // Milk Coffee Parameters
            prodTypeNbr = (int)ProductType_t.MilkCoffee_e;
            prodParam[prodTypeNbr].productType = ProductType_t.MilkCoffee_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempWarm_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Front_e;
            prodParam[prodTypeNbr].cakeThickness = 14.0;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].prebrewTime = 0.8;
            prodParam[prodTypeNbr].relaxTime = 2.0;
            prodParam[prodTypeNbr].pressAfter = 2.0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortOne_e;
            prodParam[prodTypeNbr].milkQnty = 15;
            prodParam[prodTypeNbr].milkPercent = 80;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqCofPlusMilk_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].waterQnty = 50;
            prodParam[prodTypeNbr].iconNbr = 73;
            prodParam[prodTypeNbr].doubleProduct = false;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].foamSequence = FoamSequence_t.FoamSeqMilkThenFoam_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Double Milk Coffee Parameters
            prodTypeNbr = (int)ProductType_t.MilkCoffee_e + doubleProductOffset;
            prodParam[prodTypeNbr].productType = ProductType_t.MilkCoffee_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempWarm_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Front_e;
            prodParam[prodTypeNbr].cakeThickness = 21.0;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].prebrewTime = 0;
            prodParam[prodTypeNbr].relaxTime = 0;
            prodParam[prodTypeNbr].pressAfter = -2.0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortOne_e;
            prodParam[prodTypeNbr].milkQnty = 30;
            prodParam[prodTypeNbr].milkPercent = 80;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqCofPlusMilk_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].waterQnty = 80;
            prodParam[prodTypeNbr].iconNbr = 73;
            prodParam[prodTypeNbr].doubleProduct = true;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].foamSequence = FoamSequence_t.FoamSeqMilkThenFoam_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Cappuccino Parameters
            prodTypeNbr = (int)ProductType_t.Cappuccino_e;
            prodParam[prodTypeNbr].productType = ProductType_t.Cappuccino_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempWarm_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Front_e;
            prodParam[prodTypeNbr].cakeThickness = 14.0;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].prebrewTime = 0.8;
            prodParam[prodTypeNbr].relaxTime = 2.0;
            prodParam[prodTypeNbr].pressAfter = 2.0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortOne_e;
            prodParam[prodTypeNbr].milkQnty = 9;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqCofPlusMilk_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].waterQnty = 50;
            prodParam[prodTypeNbr].iconNbr = 10;
            prodParam[prodTypeNbr].doubleProduct = false;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].foamSequence = FoamSequence_t.FoamSeqFoamThenMilk_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Double Cappuccino Parameters
            prodTypeNbr = (int)ProductType_t.Cappuccino_e + doubleProductOffset;
            prodParam[prodTypeNbr].productType = ProductType_t.Cappuccino_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempWarm_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Front_e;
            prodParam[prodTypeNbr].cakeThickness = 21.0;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].prebrewTime = 0.0;
            prodParam[prodTypeNbr].relaxTime = 0.0;
            prodParam[prodTypeNbr].pressAfter = -2.0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortOne_e;
            prodParam[prodTypeNbr].milkQnty = 18;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqCofPlusMilk_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].waterQnty = 80;
            prodParam[prodTypeNbr].iconNbr = 10;
            prodParam[prodTypeNbr].doubleProduct = true;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].foamSequence = FoamSequence_t.FoamSeqFoamThenMilk_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Espresso Macchiato Parameters
            prodTypeNbr = (int)ProductType_t.EspressoMacchiato_e;
            prodParam[prodTypeNbr].productType = ProductType_t.EspressoMacchiato_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempWarm_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Front_e;
            prodParam[prodTypeNbr].cakeThickness = 14.0;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].prebrewTime = 0.8;
            prodParam[prodTypeNbr].relaxTime = 2.0;
            prodParam[prodTypeNbr].pressAfter = 2.0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortOne_e;
            prodParam[prodTypeNbr].milkQnty = 2;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqCofDelayedMilk_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].waterQnty = 50;
            prodParam[prodTypeNbr].iconNbr = 86;
            prodParam[prodTypeNbr].doubleProduct = false;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].foamSequence = FoamSequence_t.FoamSeqFoamThenMilk_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Double Espresso Macchiato Parameters
            prodTypeNbr = (int)ProductType_t.EspressoMacchiato_e + doubleProductOffset;
            prodParam[prodTypeNbr].productType = ProductType_t.EspressoMacchiato_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempWarm_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Front_e;
            prodParam[prodTypeNbr].cakeThickness = 21.0;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].prebrewTime = 0.0;
            prodParam[prodTypeNbr].relaxTime = 0.0;
            prodParam[prodTypeNbr].pressAfter = -2.0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortOne_e;
            prodParam[prodTypeNbr].milkQnty = 4;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqCofDelayedMilk_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].waterQnty = 80;
            prodParam[prodTypeNbr].iconNbr = 86;
            prodParam[prodTypeNbr].doubleProduct = true;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].foamSequence = FoamSequence_t.FoamSeqFoamThenMilk_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Latte Macchiato Parameters
            prodTypeNbr = (int)ProductType_t.LatteMacchiato_e;
            prodParam[prodTypeNbr].productType = ProductType_t.LatteMacchiato_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempWarm_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Front_e;
            prodParam[prodTypeNbr].cakeThickness = 14;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].prebrewTime = 0.8;
            prodParam[prodTypeNbr].relaxTime = 2.0;
            prodParam[prodTypeNbr].pressAfter = 2.0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortOne_e;
            prodParam[prodTypeNbr].milkQnty = 15;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqMilkThenCof_e;
            prodParam[prodTypeNbr].latteMacchTime = 20;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].waterQnty = 60;
            prodParam[prodTypeNbr].iconNbr = 68;
            prodParam[prodTypeNbr].doubleProduct = false;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].foamSequence = FoamSequence_t.FoamSeqFoamThenMilk_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Double Latte Macchiato Parameters
            prodTypeNbr = (int)ProductType_t.LatteMacchiato_e + doubleProductOffset;
            prodParam[prodTypeNbr].productType = ProductType_t.LatteMacchiato_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempWarm_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Front_e;
            prodParam[prodTypeNbr].cakeThickness = 21.0;
            prodParam[prodTypeNbr].powderPress = 64;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].prebrewTime = 0.0;
            prodParam[prodTypeNbr].relaxTime = 0.0;
            prodParam[prodTypeNbr].pressAfter = -2.0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortOne_e;
            prodParam[prodTypeNbr].milkQnty = 30;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqMilkThenCof_e;
            prodParam[prodTypeNbr].latteMacchTime = 20;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].waterQnty = 120;
            prodParam[prodTypeNbr].iconNbr = 68;
            prodParam[prodTypeNbr].doubleProduct = true;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].foamSequence = FoamSequence_t.FoamSeqFoamThenMilk_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
            prodParam[prodTypeNbr].desiredExtractionTimeMin = 20;
            prodParam[prodTypeNbr].desiredExtractionTimeMax = 26;

            // Milk Parameters
            prodTypeNbr = (int)ProductType_t.Milk_e;
            prodParam[prodTypeNbr].productType = ProductType_t.Milk_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempWarm_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Undef_e;
            prodParam[prodTypeNbr].cakeThickness = 0.0;
            prodParam[prodTypeNbr].powderPress = 0;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].prebrewTime = 0;
            prodParam[prodTypeNbr].relaxTime = 0;
            prodParam[prodTypeNbr].pressAfter = 0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortOne_e;
            prodParam[prodTypeNbr].milkQnty = 10;
            prodParam[prodTypeNbr].milkPercent = 100;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqMilkOnly_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].waterQnty = 0;
            prodParam[prodTypeNbr].iconNbr = 65;
            prodParam[prodTypeNbr].doubleProduct = false;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].foamSequence = FoamSequence_t.FoamSeqMilkThenFoam_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;

            // Double Milk Parameters
            prodTypeNbr = (int)ProductType_t.Milk_e + doubleProductOffset;
            prodParam[prodTypeNbr].productType = ProductType_t.Milk_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempWarm_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Undef_e;
            prodParam[prodTypeNbr].cakeThickness = 0.0;
            prodParam[prodTypeNbr].powderPress = 0;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].prebrewTime = 0;
            prodParam[prodTypeNbr].relaxTime = 0;
            prodParam[prodTypeNbr].pressAfter = 0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortOne_e;
            prodParam[prodTypeNbr].milkQnty = 20;
            prodParam[prodTypeNbr].milkPercent = 100;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqMilkOnly_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].waterQnty = 0;
            prodParam[prodTypeNbr].iconNbr = 65;
            prodParam[prodTypeNbr].doubleProduct = true;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].foamSequence = FoamSequence_t.FoamSeqMilkThenFoam_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;

            // Milkfoam Parameters
            prodTypeNbr = (int)ProductType_t.MilkFoam_e;
            prodParam[prodTypeNbr].productType = ProductType_t.MilkFoam_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempWarm_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Undef_e;
            prodParam[prodTypeNbr].cakeThickness = 0.0;
            prodParam[prodTypeNbr].powderPress = 0;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].prebrewTime = 0;
            prodParam[prodTypeNbr].relaxTime = 0;
            prodParam[prodTypeNbr].pressAfter = 0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortOne_e;
            prodParam[prodTypeNbr].milkQnty = 10;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqMilkOnly_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].waterQnty = 0;
            prodParam[prodTypeNbr].iconNbr = 76;
            prodParam[prodTypeNbr].doubleProduct = false;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].foamSequence = FoamSequence_t.FoamSeqFoamThenMilk_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;

            // Double Milkfoam Parameters
            prodTypeNbr = (int)ProductType_t.MilkFoam_e + doubleProductOffset;
            prodParam[prodTypeNbr].productType = ProductType_t.MilkFoam_e;
            prodParam[prodTypeNbr].productProcess = ProductProcess_t.ProProcCoffee_e;
            prodParam[prodTypeNbr].milkTemp = MilkTemp_t.MlkTempWarm_e;
            prodParam[prodTypeNbr].productNbr = 1;
            prodParam[prodTypeNbr].productCyc = 1;
            prodParam[prodTypeNbr].beanHopper = BeanHopper_t.Undef_e;
            prodParam[prodTypeNbr].cakeThickness = 0.0;
            prodParam[prodTypeNbr].powderPress = 0;
            prodParam[prodTypeNbr].coffeeAroma = 0;
            prodParam[prodTypeNbr].bypassQnty = 0;
            prodParam[prodTypeNbr].prebrewTime = 0;
            prodParam[prodTypeNbr].relaxTime = 0;
            prodParam[prodTypeNbr].pressAfter = 0;
            prodParam[prodTypeNbr].milkSort = MilkSort_t.MlkSortOne_e;
            prodParam[prodTypeNbr].milkQnty = 20;
            prodParam[prodTypeNbr].milkPercent = 0;
            prodParam[prodTypeNbr].milkSequence = MilkSequence_t.MilkSeqMilkOnly_e;
            prodParam[prodTypeNbr].latteMacchTime = 0;
            prodParam[prodTypeNbr].steamTime = 0;
            prodParam[prodTypeNbr].steamTemp = 0;
            prodParam[prodTypeNbr].waterQnty = 0;
            prodParam[prodTypeNbr].iconNbr = 76;
            prodParam[prodTypeNbr].doubleProduct = true;
            //public ProductParameter_t[] prodParam; = GetTextArray( LanguageContentIndex_t.DefaultProductParamNames_e )[prodTypeNbr];
            prodParam[prodTypeNbr].airStopTemp = 0;
            prodParam[prodTypeNbr].coffeeOutlet = CoffeeOutlet_t.CofOutUndef_e;
            prodParam[prodTypeNbr].foamSequence = FoamSequence_t.FoamSeqFoamThenMilk_e;
            prodParam[prodTypeNbr].refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;
        }
    }
}
