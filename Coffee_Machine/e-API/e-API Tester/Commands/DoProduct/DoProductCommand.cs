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
using eApi.Filesystem;

namespace eApi.Commands.DoProductForm
{
    public partial class CommandForm : Form
    {
        public SerialComm.Packet_t packet;
        private ProductParameter_t ProductParams;
        private ProductParameter_t[] defaultProductParams;
        private filesystem.FileFormat_t[] files;
        private int nmbrOfCustomButtons = 0;

        public CommandForm(ref ProductParameter_t[] defaultProducts, SerialComm.Packet_t p)
        {
            InitializeComponent();
            packet = p;
            defaultProductParams = defaultProducts;

            // load saved products
            files = filesystem.loadFromFiles();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].id != null)
                {
                    files[i] = CompleteDataOfFile(files[i]);
                    nmbrOfCustomButtons++;
                    ParamButton dpb = new ParamButton(new ProductParameter_t(files[i].packet), OnButtonClick, files[i].name, files[i].id);
                    dpb.parameter = files[i].packet.message.parameter;
                    flowProductButtons.Controls.Add(dpb);
                }
            }

            // if there are no saved products load the default ones
            if (flowProductButtons.Controls.Count == 0)
            {
                ParamButton Espresso = new ParamButton(defaultProductParams[(byte)ProductType_t.Espresso_e], OnButtonClick, "Espresso", "defEsp");
                ParamButton Coffee = new ParamButton(defaultProductParams[(byte)ProductType_t.Coffee_e], OnButtonClick, "Coffee", "defCof");
                ParamButton Cappuccino = new ParamButton(defaultProductParams[(byte)ProductType_t.Cappuccino_e], OnButtonClick, "Cappuccino", "defCap");
                ParamButton Americano = new ParamButton(defaultProductParams[(byte)ProductType_t.Americano_e], OnButtonClick, "Americano", "defAmericano");
                ParamButton AutoSteam = new ParamButton(defaultProductParams[(byte)ProductType_t.AutoSteam_e], OnButtonClick, "Auto steam", "defAutoSteam");
                ParamButton HotWater = new ParamButton(defaultProductParams[(byte)ProductType_t.HotWater_e], OnButtonClick, "Hot water", "defHotWater");
                ParamButton Everfoam = new ParamButton(defaultProductParams[(byte)ProductType_t.Everfoam_e], OnButtonClick, "Everfoam", "defEFoam");
                ParamButton Milk = new ParamButton(defaultProductParams[(byte)ProductType_t.Milk_e], OnButtonClick, "Milk", "defMilk");
                ParamButton MilkFoam = new ParamButton(defaultProductParams[(byte)ProductType_t.MilkFoam_e], OnButtonClick, "Milk foam", "defMilkFoam");
                ParamButton ManualSteam = new ParamButton(defaultProductParams[(byte)ProductType_t.ManualSteam_e], OnButtonClick, "Manual steam", "defManSteam");

                flowProductButtons.Controls.Add(Espresso);
                flowProductButtons.Controls.Add(Coffee);
                flowProductButtons.Controls.Add(Cappuccino);
                flowProductButtons.Controls.Add(Americano);
                flowProductButtons.Controls.Add(AutoSteam);
                flowProductButtons.Controls.Add(Everfoam);
                flowProductButtons.Controls.Add(Milk);
                flowProductButtons.Controls.Add(MilkFoam);
                flowProductButtons.Controls.Add(ManualSteam);
            }

            // select Espresso as default
            cbxOutletSide.SelectedIndex = 0;
            this.ProductParams = defaultProductParams[(byte)ProductType_t.Espresso_e];
            paramsToControls();

        }

        /// <summary>
        /// Show effect when a file is hovered over the form
        /// </summary>
        void CommandForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        /// <summary>
        /// Check if the droped files are products and load them if they are
        /// </summary>
        void CommandForm_DragDrop(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string s in files)
                {
                    try
                    {
                        filesystem.FileFormat_t file = new filesystem.FileFormat_t();
                        file = filesystem.getDataFromFile(s);

                        if (cbxOutletSide.SelectedIndex != 0 && cbxOutletSide.SelectedIndex != 1)
                            cbxOutletSide.SelectedIndex = 0;
                        file.packet.message.parameter = (ushort)cbxOutletSide.SelectedIndex;
                        if (!file.id.Contains("product_"))
                            file.id = "product_" + file.id;
                        filesystem.saveToFile(file.packet, file.name, file.id);
                        nmbrOfCustomButtons++;
                        ParamButton dpb = new ParamButton(new ProductParameter_t(file.packet), OnButtonClick, file.name, file.id);
                        dpb.parameter = file.packet.message.parameter;
                        flowProductButtons.Controls.Add(dpb);
                    }
                    catch { }
                }
            }
        }

        private bool loadParamOnIndexChange = true;
        /// <summary>
        /// Load parameter of a product when its button is clicked
        /// </summary>
        private void OnButtonClick(object sender, EventArgs e)
        {
            ParamButton button = sender as ParamButton;
            txtSaveName.Text = button.ProductParams.prodName;

            foreach (ParamButton btn in flowProductButtons.Controls)
            {
                btn.UseVisualStyleBackColor = true;
            }
            button.UseVisualStyleBackColor = false;

            try
            {
                loadParamOnIndexChange = false;
                this.ProductParams = button.ProductParams;
                paramsToControls();
                loadParamOnIndexChange = true;
                cbxOutletSide.SelectedIndex = button.parameter;

            }
            catch
            {
                filesystem.deleteFile(button.id);
                flowProductButtons.Controls.Remove(button);
            }
        }

        /// <summary>
        /// Completes missing data bytes with the default bytes for this product type and saves the file.
        /// </summary>
        private filesystem.FileFormat_t CompleteDataOfFile(filesystem.FileFormat_t file)
        {
            if (file.packet.data.Length < 30)
            {
                if (file.packet.data.Length >= 1)
                {
                    int oldLength = file.packet.data.Length;
                    byte[] defaultData = defaultProductParams[file.packet.data[0]].ToArrayApi();
                    Array.Resize(ref file.packet.data, defaultData.Length);
                    Array.Copy(defaultData, oldLength, file.packet.data, oldLength, defaultData.Length - oldLength);
                    file.packet.dataLength = (ushort)file.packet.data.Length;
                }
                else
                {
                    file.packet.data = defaultProductParams[0].ToArrayApi();
                    file.packet.dataLength = (ushort)file.packet.data.Length;
                }

                filesystem.saveToFile(file.packet, file.name, file.id);
            }

            return file;            
        }

        /// <summary>
        /// If another product type is selected change the controls to the default values of this type
        /// </summary>
        private void cbxProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadParamOnIndexChange)
            {
                if (cbxSingleDouble.SelectedIndex == 1)
                    ProductParams = defaultProductParams[cbxProductType.SelectedIndex + 1 + (int)ProductType_t.Max_e];
                else
                    ProductParams = defaultProductParams[cbxProductType.SelectedIndex + 1];

                paramsToControls();
            }
        }

        /// <summary>
        /// If singe changes to double or reversed, change the controls to the correct default values
        /// </summary>
        private void cbxSingleDouble_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadParamOnIndexChange)
            {
                if (cbxSingleDouble.SelectedIndex == 1)
                    ProductParams = defaultProductParams[cbxProductType.SelectedIndex + 1 + (int)ProductType_t.Max_e];
                else
                    ProductParams = defaultProductParams[cbxProductType.SelectedIndex + 1];

                paramsToControls();
            }
        }

        /// <summary>
        /// Close Form without sending a packet
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            packet.dataLength = 0;
            this.Close();
        }

        /// <summary>
        /// Create and send the packet
        /// </summary>
        private void btnSend_Click(object sender, EventArgs e)
        {
            controlsToParams();
            ProductParams.productProcess = getProdProcess(ProductParams.productType);
            packet.data = ProductParams.ToArrayApi();
            packet.dataLength = (ushort)packet.data.Length;
            packet.message.parameter = (ushort)cbxOutletSide.SelectedIndex;
            this.Close();
        }

        /// <summary>
        /// Show the current parameters on the controls
        /// </summary>
        private void paramsToControls()
        {
            decimal temp = 0;

            // General
            if ((byte)ProductParams.productType <= 17)
                cbxProductType.SelectedIndex = (byte)ProductParams.productType - 1;
            else
            {
                cbxProductType.SelectedIndex = 1;
                ProductParams.productType = ProductType_t.Espresso_e;
            }
            if (ProductParams.waterQnty >= numWaterQuantity.Minimum && ProductParams.waterQnty <= numWaterQuantity.Maximum)
                numWaterQuantity.Value = ProductParams.waterQnty;
            else
                numWaterQuantity.Value = numWaterQuantity.Minimum;

            // Coffee
            if (hasCoffeeParams(ProductParams.productType))
            {
                if ((byte)ProductParams.beanHopper <= 1)
                    cbxBeanHopper.SelectedIndex = (byte)ProductParams.beanHopper;
                else
                {
                    cbxBeanHopper.SelectedIndex = 0;
                    ProductParams.beanHopper = BeanHopper_t.Front_e;
                }
                if (ProductParams.cakeThickness >= (double)numCakeThickness.Minimum && ProductParams.cakeThickness <= (double)numCakeThickness.Maximum)
                    numCakeThickness.Value = (decimal)ProductParams.cakeThickness;
                else
                    numCakeThickness.Value = numCakeThickness.Minimum;

                switch (ProductParams.powderPress)
                {
                    case 64: cbxTamping.SelectedIndex = 0; break;
                    case 92: cbxTamping.SelectedIndex = 1; break;
                    case 120: cbxTamping.SelectedIndex = 2; break;
                    default: cbxTamping.SelectedIndex = 0; break;
                }
                numSecTamping.Value = (decimal)ProductParams.pressAfter;
                numPreInfusion.Value = (decimal)ProductParams.prebrewTime;
                numRelaxTime.Value = (decimal)ProductParams.relaxTime;
            }

            // Milk
            if (hasMilkParams(ProductParams.productType))
            {
                if ((byte)ProductParams.milkSequence <= 5)
                    cbxMilkSequence.SelectedIndex = (byte)ProductParams.milkSequence;
                else
                {
                    cbxMilkSequence.SelectedIndex = 0;
                    ProductParams.milkSequence = MilkSequence_t.MilkSeqCofThenMilk_e;
                }
                numMilkQuantity.Value = (decimal)ProductParams.milkQnty;
                if ((byte)ProductParams.milkTemp <= 1)
                    cbxMilkTemperature.SelectedIndex = (byte)ProductParams.milkTemp;
                else
                {
                    cbxMilkTemperature.SelectedIndex = 0;
                    ProductParams.milkTemp = MilkTemp_t.MlkTempWarm_e;
                }
                numMilkPercent.Value = ProductParams.milkPercent;
                if ((byte)ProductParams.foamSequence <= 2)
                    cbxFoamSequence.SelectedIndex = (byte)ProductParams.foamSequence;
                else
                {
                    cbxFoamSequence.SelectedIndex = 0;
                    ProductParams.foamSequence = FoamSequence_t.FoamSeqFoamThenMilk_e;
                }
            }

            // Everfoam
            if (cbxEverfoamTempUnit.SelectedIndex < 0)
                cbxEverfoamTempUnit.SelectedIndex = 0;

            if (hasEverfoamParams(ProductParams.productType))
            {
                if (cbxSteamTempUnit.SelectedIndex == 1) // Fahrenheit
                    temp = (decimal)(ProductParams.airStopTemp*1.8 +32);
                else // Celsius
                    temp = ProductParams.airStopTemp;

                if (ProductParams.airStopTime >= 1 && ProductParams.airStopTime <= 100)
                    numEverfoamTime.Value = (decimal)ProductParams.airStopTime;
                else
                    ProductParams.airStopTime = 1;
                cbxEverfoamMode.SelectedIndex = (int)ProductParams.everfoamMode;
            }
            else
            {
                if (cbxEverfoamTempUnit.SelectedIndex == 1) // Fahrenheit
                    temp = 32;
                else // Celsius
                    temp = 0;
                numEverfoamTime.Value = 1;
                cbxEverfoamMode.SelectedIndex = (int)EverfoamMode_t.NTC_e;
            }
            if (numEverfoamTemp.Minimum <= temp && numEverfoamTemp.Maximum >= temp)
                numEverfoamTemp.Value = temp;
            else
                numEverfoamTemp.Value = numEverfoamTemp.Minimum;


            // Steam
            if (cbxSteamTempUnit.SelectedIndex < 0)
                cbxSteamTempUnit.SelectedIndex = 0;

            if (hasSteamParams(ProductParams.productType))
            {
                if (cbxSteamTempUnit.SelectedIndex == 1) // Fahrenheit
                    temp = (decimal)(ProductParams.steamTemp * 1.8 + 32);
                else // Celsius
                    temp = ProductParams.steamTemp;

                if (ProductParams.steamTime >= 1 && ProductParams.steamTime <= 300)
                    temp = ProductParams.steamTime;
                else
                    temp = 1;
            }
            else
            {
                if (cbxSteamTempUnit.SelectedIndex == 1) // Fahrenheit
                    temp = 86;
                else // Celsius
                    temp = 30;
                numSteamTime.Value = 1;
            }
            if (numSteamTemp.Minimum <= temp && numSteamTemp.Maximum >= temp)
                numSteamTemp.Value = temp;
            else
                numSteamTemp.Value = numSteamTemp.Minimum;


            // others
            if (hasLatteMacchParams(ProductParams.productType))
            {
                numLatteMacchiatoTime.Value = ProductParams.latteMacchTime;
            }

            if( ProductParams.pumpSpeedMilk >= ( double )numPumpSpeedMilk.Minimum && ProductParams.pumpSpeedMilk <= ( double )numPumpSpeedMilk.Maximum )
            {
                numPumpSpeedMilk.Value = ( decimal )ProductParams.pumpSpeedMilk;
            }
            else
            {
                numPumpSpeedMilk.Value = 1500;
            }

            if( ProductParams.pumpSpeedFoam >= ( double )numPumpSpeedFoam.Minimum && ProductParams.pumpSpeedFoam <= ( double )numPumpSpeedFoam.Maximum )
            {
                numPumpSpeedFoam.Value = ( decimal )ProductParams.pumpSpeedFoam;
            }
            else
            {
                numPumpSpeedFoam.Value = 3000;
            }

            changeActiveControls(ProductParams.productType);
        }

        /// <summary>
        /// Save the current control values to the parameters
        /// </summary>
        private void controlsToParams()
        {
            // General
            if(cbxProductType.SelectedIndex <= 17)
                ProductParams.productType = (ProductType_t)cbxProductType.SelectedIndex + 1;
            else
                ProductParams.productType = ProductType_t.Undef_e;

            ProductParams.waterQnty = (int)numWaterQuantity.Value;

            // Coffee
            if(hasCoffeeParams(ProductParams.productType))
            {
                if(cbxBeanHopper.SelectedIndex <= 1)
                    ProductParams.beanHopper = (BeanHopper_t)cbxBeanHopper.SelectedIndex;
                else
                    ProductParams.beanHopper = BeanHopper_t.Undef_e;

                ProductParams.cakeThickness = (double)numCakeThickness.Value;
                switch(cbxTamping.SelectedIndex)
                {
                    case 0: ProductParams.powderPress = 64; break;
                    case 1: ProductParams.powderPress = 92; break;
                    case 2: ProductParams.powderPress = 120; break;
                    default: ProductParams.powderPress = 64; break;
                }
                ProductParams.pressAfter = (double)numSecTamping.Value;
                ProductParams.prebrewTime = (double)numPreInfusion.Value;
                ProductParams.relaxTime = (double)numRelaxTime.Value;
                ProductParams.bypassQnty = ( int )numBypass.Value;
                ProductParams.hotWaterQnty = ( int )numHotWaterQuantity.Value;

            }
            else
            {
                ProductParams.beanHopper = BeanHopper_t.Undef_e;
                ProductParams.cakeThickness = 0;
                ProductParams.powderPress = 0;
                ProductParams.pressAfter = 0;
                ProductParams.prebrewTime = 0;
                ProductParams.relaxTime = 0;
                ProductParams.bypassQnty = 0;
                ProductParams.hotWaterQnty = 0;
            }

            // Milk
            if(hasMilkParams(ProductParams.productType))
            {
                if(cbxMilkSequence.SelectedIndex <= 5)
                    ProductParams.milkSequence = (MilkSequence_t)cbxMilkSequence.SelectedIndex;
                else
                    ProductParams.milkSequence = MilkSequence_t.MilkSeqUndef_e;

                ProductParams.milkQnty = (double)numMilkQuantity.Value;
                if(cbxMilkTemperature.SelectedIndex <= 1)
                    ProductParams.milkTemp = (MilkTemp_t)cbxMilkTemperature.SelectedIndex;
                else
                    ProductParams.milkTemp = MilkTemp_t.MlkTempUndef_e;

                ProductParams.milkPercent = (int)numMilkPercent.Value;
                if(cbxFoamSequence.SelectedIndex <= 2)
                    ProductParams.foamSequence = (FoamSequence_t)cbxFoamSequence.SelectedIndex;
                else
                    ProductParams.foamSequence = FoamSequence_t.FoamSeqUndef_e;

                ProductParams.airQuantity = ( int )numAirQuantity.Value;
            }
            else
            {
                ProductParams.milkSequence = MilkSequence_t.MilkSeqUndef_e;
                ProductParams.milkQnty = 0;
                ProductParams.milkTemp = MilkTemp_t.MlkTempUndef_e;
                ProductParams.milkPercent = 0;
                ProductParams.foamSequence = FoamSequence_t.FoamSeqUndef_e;
                ProductParams.airQuantity = 0;
            }

            // Everfoam
            if(hasEverfoamParams(ProductParams.productType))
            {
                if(cbxEverfoamTempUnit.SelectedIndex == 1) // Fahrenheit
                    ProductParams.airStopTemp = (int)Math.Round(((float)numEverfoamTemp.Value-32)/1.8);
                else // Celsius
                    ProductParams.airStopTemp = (int)Math.Round(numEverfoamTemp.Value);
                ProductParams.airStopTime = (double)numEverfoamTime.Value;
                ProductParams.everfoamMode = (EverfoamMode_t)cbxEverfoamMode.SelectedIndex;
            }
            else
            {
                ProductParams.airStopTemp = 0;
                ProductParams.airStopTime = 1;
                ProductParams.everfoamMode = EverfoamMode_t.Ignored_e;
            }

            // Steam
            if(hasSteamParams(ProductParams.productType))
            {
                if (cbxSteamTempUnit.SelectedIndex == 1) // Fahrenheit
                    ProductParams.steamTemp = (int)Math.Round(((float)numSteamTemp.Value - 32) / 1.8);
                else // Celsius
                    ProductParams.steamTemp = (int)Math.Round(numSteamTemp.Value);
                ProductParams.steamTime = (int)Math.Round(numSteamTime.Value);
            }
            else
            {
                ProductParams.steamTemp = 30;
                ProductParams.steamTime = 1;
            }

            // others
            if(hasLatteMacchParams(ProductParams.productType))
            {
                ProductParams.latteMacchTime = (int)numLatteMacchiatoTime.Value;
            }
            else
                ProductParams.latteMacchTime = 0;

            // Produkttyp mit Milch?
            if( ProductType_t.MilkCoffee_e <= ProductParams.productType && ProductParams.productType <= ProductType_t.MilkFoam_e )
            {
                ProductParams.pumpSpeedMilk = Convert.ToInt32( Math.Round( numPumpSpeedMilk.Value ) );
                ProductParams.pumpSpeedFoam = Convert.ToInt32( Math.Round( numPumpSpeedFoam.Value ) );
            }
        }

        /// <summary>
        /// Hide all unnecessary controls for the selected product type
        /// </summary>
        private void changeActiveControls(ProductType_t type)
        {
            // Coffee
            if (hasCoffeeParams(type))
            {
                cbxBeanHopper.Enabled = true;
                numCakeThickness.Enabled = true;
                cbxTamping.Enabled = true;
                numSecTamping.Enabled = true;
                numPreInfusion.Enabled = true;
                numRelaxTime.Enabled = true;
            }
            else
            {
                cbxBeanHopper.Enabled = false;
                numCakeThickness.Enabled = false;
                cbxTamping.Enabled = false;
                numSecTamping.Enabled = false;
                numPreInfusion.Enabled = false;
                numRelaxTime.Enabled = false;
            }

            // Milk
            if (hasMilkParams(type))
            {
                cbxMilkSequence.Enabled = true;
                numMilkQuantity.Enabled = true;
                cbxMilkTemperature.Enabled = true;
                numMilkPercent.Enabled = true;
                cbxFoamSequence.Enabled = true;
            }
            else
            {
                cbxMilkSequence.Enabled = false;
                numMilkQuantity.Enabled = false;
                cbxMilkTemperature.Enabled = false;
                numMilkPercent.Enabled = false;
                cbxFoamSequence.Enabled = false;
            }

            // others
            if (hasLatteMacchParams(type))
            {
                numLatteMacchiatoTime.Enabled = true;
            }
            else
            {
                numLatteMacchiatoTime.Enabled = false;
            }

            // everfoam
            if (hasEverfoamParams(type))
            {
                numEverfoamTemp.Enabled = true;
                numEverfoamTime.Enabled = true;
                cbxEverfoamMode.Enabled = true;
                cbxEverfoamTempUnit.Enabled = true;
            }
            else
            {
                numEverfoamTemp.Enabled = false;
                numEverfoamTime.Enabled = false;
                cbxEverfoamMode.Enabled = false;
                cbxEverfoamTempUnit.Enabled = false;
            }

            // steam
            if (hasSteamParams(type))
            {
                numSteamTemp.Enabled = true;
                numSteamTime.Enabled = true;
                cbxSteamTempUnit.Enabled = true;
            }
            else
            {
                numSteamTemp.Enabled = false;
                numSteamTime.Enabled = false;
                cbxSteamTempUnit.Enabled = false;
            }
        }

        /// <summary>
        /// Check if the product type needs milk parameters
        /// </summary>
        private bool hasMilkParams(ProductType_t type)
        {
            if ((byte)type >= 12 && (byte)type <= 17)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if the product type needs coffee parameters
        /// </summary>
        private bool hasCoffeeParams(ProductType_t type)
        {
            if (((byte)type >= 1 && (byte)type <= 7) || hasMilkParams(type) == true)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if the product type needs latte macchiato parameters
        /// </summary>
        private bool hasLatteMacchParams(ProductType_t type)
        {
            if (type == ProductType_t.LatteMacchiato_e)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if the product type needs everfoam parameters
        /// </summary>
        private bool hasEverfoamParams(ProductType_t type)
        {
            if ((int)type > 8 && (int)type < 18) // steam or milk
                return true;
            else
                return false;
        }

        private bool hasSteamParams(ProductType_t type)
        {
            if ((int)type > 8 && (int)type < 18) // steam or milk
                return true;
            else
                return false;
        }

        /// <summary>
        /// Select the correct product process based on the product type
        /// </summary>
        private ProductProcess_t getProdProcess(ProductType_t type)
        {
            if (type == ProductType_t.HotWater_e)
            {
                return ProductProcess_t.ProProcWater_e;
            }
            else if (hasCoffeeParams(type) == true)
            {
                return ProductProcess_t.ProProcCoffee_e;
            }
            else if ((byte)type >= 9 && (byte)type <= 11)
            {
                return ProductProcess_t.ProProcSteam_e;
            }
            else
            {
                return ProductProcess_t.ProProcUndef_e;
            }
        }

        /// <summary>
        /// Create a new product button and saves it
        /// </summary>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            ParamButton button = new ParamButton(new ProductParameter_t(), OnButtonClick,"","");
            filesystem.FileFormat_t file = new filesystem.FileFormat_t();

            foreach (ParamButton pb in flowProductButtons.Controls)
            {
                // when the save name is the same as the name of the selected button overwrite it
                // usevisualstylebackcolor is used as an indicator of the clicked state
                if (pb.ProductParams.prodName == txtSaveName.Text && pb.UseVisualStyleBackColor == false)
                {
                    button = pb;
                    flowProductButtons.Controls.Remove(pb);
                }
            }

            controlsToParams();

            if (button.id == "")
            {
                if (txtSaveName.Text != "")
                    file.name = txtSaveName.Text;
                else
                    file.name = "Custom " + nmbrOfCustomButtons;

                file.id = "custom" + nmbrOfCustomButtons;
                controlsToParams();

                file.packet = new SerialComm.Packet_t();
                file.packet.data = ProductParams.ToArrayApi();
                file.packet.dataLength = (ushort)file.packet.data.Length;

                nmbrOfCustomButtons++;
            }
            else
            {
                file.id = button.id;
                file.name = txtSaveName.Text;
                file.packet.data = ProductParams.ToArrayApi();
                file.packet.dataLength = (ushort)file.packet.data.Length;
            }

            if (cbxOutletSide.SelectedIndex != 0 && cbxOutletSide.SelectedIndex != 1)
                cbxOutletSide.SelectedIndex = 0;
            file.packet.message.parameter = (ushort)cbxOutletSide.SelectedIndex;
            if(!file.id.Contains("product_"))
                file.id = "product_" + file.id;
            filesystem.saveToFile(file.packet, file.name, file.id);
            ParamButton dpb = new ParamButton(new ProductParameter_t(file.packet), OnButtonClick, file.name, file.id);
            flowProductButtons.Controls.Add(dpb);
        }

        /// <summary>
        /// Delete the selected product button
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (ParamButton pb in flowProductButtons.Controls)
            {
                // remove the selected button
                if (pb.UseVisualStyleBackColor == false)
                {
                    flowProductButtons.Controls.Remove(pb);
                    filesystem.deleteFile(pb.id);
                }
            }
        }


        private bool tempAdjustValueFromUnit = true;
        private void cbxEverfoamTempUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal value = numEverfoamTemp.Value;
            if (cbxEverfoamTempUnit.SelectedIndex == 1) // Fahrenheit
            {
                numEverfoamTemp.Minimum = 32;
                numEverfoamTemp.Value = numEverfoamTemp.Minimum;
                numEverfoamTemp.Maximum = 212;

                if(tempAdjustValueFromUnit == true)
                    value = (decimal)Math.Round((float)value * 1.8 + 32);
                if (value > numEverfoamTemp.Minimum && value < numEverfoamTemp.Maximum)
                    numEverfoamTemp.Value = value;
            }
            else // Celsius
            {
                numEverfoamTemp.Minimum = 0;
                numEverfoamTemp.Value = numEverfoamTemp.Minimum;
                numEverfoamTemp.Maximum = 100;

                if (tempAdjustValueFromUnit == true)
                    value = (decimal)Math.Round(((float)value - 32)/1.8);
                if (value > numEverfoamTemp.Minimum && value < numEverfoamTemp.Maximum)
                    numEverfoamTemp.Value = value;
            }
        }

        private void cbxSteamTempUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal value = numEverfoamTemp.Value;
            if (cbxSteamTempUnit.SelectedIndex == 1) // Fahrenheit
            {
                numSteamTemp.Minimum = 86;
                numSteamTemp.Value = numSteamTemp.Minimum;
                numSteamTemp.Maximum = 172;

                if (tempAdjustValueFromUnit == true)
                    value = (decimal)Math.Round((float)value * 1.8 + 32);
                if (value > numSteamTemp.Minimum && value < numSteamTemp.Maximum)
                    numSteamTemp.Value = value;
            }
            else // Celsius
            {
                numSteamTemp.Minimum = 30;
                numSteamTemp.Value = numSteamTemp.Minimum;
                numSteamTemp.Maximum = 80;

                if (tempAdjustValueFromUnit == true)
                    value = (decimal)Math.Round(((float)value - 32) / 1.8);
                if (value > numSteamTemp.Minimum && value < numSteamTemp.Maximum)
                    numSteamTemp.Value = value;
            }
        }

    }

    /// <summary>
    /// An expanded button which includes the product parameters and the outlet side
    /// </summary>
    public class ParamButton : Button
    {
        public ParamButton(ProductParameter_t param, EventHandler handler, string name, string _id)
        {
            this.Width = 100;
            this.Height = 30;
            ProductParams = param;
            ProductParams.prodName = name;
            this.Click += new EventHandler(handler);
            this.Text = name;
            this.BackColor = Color.FromKnownColor(KnownColor.Control);
            this.UseVisualStyleBackColor = true;
            this.id = _id;
        }

        public string id;
        public ProductParameter_t ProductParams;
        public ushort parameter;
    }
}
