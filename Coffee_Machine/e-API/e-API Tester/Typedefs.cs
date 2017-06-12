using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

using ApiSerialComm;

namespace eApi.Typedef
{
    /// <summary>
    /// List of all software module IDs which can commmunicate together
    /// </summary>
    public enum ModuleId_t : byte
    {
        Touch_e = 65,        // 0x41, Touch Serial Interface RS-232
        Api_e = 66,          // 0x42, API Remote Device
    }

    //------------------------------------------------------------------------------
    /** Prozess-Seite */
    public enum ProcSide_t
    {
        Left_e = 0,
        Right_e,
        Max_e,
        Undef_e = 0xFF,
    }

    //------------------------------------------------------------------------------
    /** Genereller on / off Status */
    public enum OnOffCtrl_t
    {
        Off_e = 0,
        On_e
    }

    //------------------------------------------------------------------------------
    /** Produkt-Abbruch-Zustand */
    public enum ProdAbortType_t
    {
        ProdFinished_e = 0,     // Produkt beendet / nicht gestoppt
        ProdStopped_e,          // Produkt gestoppt (Rückwärtskompatibilität)
        ProdAbortMachine_e,     // Produkt automatisch durch Maschine gestoppt
        ProdAbortUser_e         // Produkt durch Benutzer gestoppt
    }

    public enum StopModuleType_t : byte
    {
        All_e = 0,
        CoffeeLeft_e,
        CoffeeRight_e,
        SteamLeft_e,
        SteamRight_e,
        HotWater_e,
    }

    /// <summary>
    /// Command signitures for the API
    /// If an unknown request arrives, Undef_e is sent as response
    /// </summary>
    public enum API_Command_t : byte
    {
        Reserved_e = 0,
        GetStatus_e,
        DoProduct_e,
        DoRinse_e,
        StartCleaning_e,
        GrinderAdjust_e,
        Grind_e,
        ResetNextCleanDate_e,
        ScreenRinse_e,
        CPUInputTest_e,
        GetExtractionTime_e,
        Stop_e,
        GetRequests_e,
        GetInfoMessages_e,
        MilkOutletRinse_e,
        DisplayAction_e,
        GetProductDump_e,
        GetSensorValues_e,
        DoEtcCalibration_e,

        // Nur für Briggo-Extension relevant
        TurnOnBreakpoints_e = 100, // Offset for the extended commands
        CoffeeActionContinue_e,
        MilkActionContinue_e,

        Undef_e = 0xFF              // Unbekanntes Kommando
    }

    public enum Status_t
    {
        notReady_e = 0,
        Ready_e,
        Undef_e = 255
    }

    /******************************************************************************/
    // Process
    public enum Process_t
    {
        ProcessCoffee_e = 0,
        ProcessSteam_e,
        ProcessHotWater_e,
        ProcessLearnWaterQnty_e,
        ProcessPowderTest_e,
        ProcessClean_e,
        ProcessRinse_e,
        ProcessExpel_e,
        ProcessScreenRinse_e,
        ProcessServicePos_e,
        ProcessDePressurize_e = 10,
        ProcessEmptyBoiler_e,
        ProcessAdjPumpPress_e,
        ProcessFlowMeterTest_e,
        ProcessParallel_e,
        ProcessGrinderTestLeft_e,
        ProcessGrinderTestRight_e,
        ProcessMotIni_e,
        ProcessScrRin_e,
        ProcessDoBopMoveTest_e,
        ProcessMilkRinse_e = 20,
        ProcessMilkClean_e,
        ProcessMilkClnAir_e,
        ProcessEmptyCofBoiler_e = 23,

        Undef_e = 0xFF
    }

    /******************************************************************************/
    // Action
    public enum Actions_t : byte
    {
        ActionIdle_e = 0,       // No product process is running
        ActionQueued_e,         // Next product is already queued
        ActionSuspended_e,      // Product process interrupted and waiting for an action
        ActionEnding_e,         // Next product can already be ordered
        ActionEndCyc_e,         // Only used for multi cyle products (end of cylce)
        ActionStoped_e,         // Product has been stopped
        ActionStarted_e,        // Product has been started
        ActionPumping_e,        // Product is being dispensed
        ActionMilkInterrupt_e,  // Milk tank empty => Waiting for refilling
        ActionCycleAborted_e,   // Only used for multi cyle products (abortion of cylce)
        ActionPwdrChute_e = 10,      // Only used for powder chute products
        ActionCleanTabs_e = 11,      // Cleaning tabs empty => Waiting for refilling

        Undef_e = 255
    }

    public struct ModuleStatus_t
    {
        public Status_t status;
        public Actions_t action;
        public Process_t process;
    }

    public struct GetStatusResponse_t
    {
        public byte machineStatus;
        public ModuleStatus_t coffeeL;
        public ModuleStatus_t coffeeR;
        public ModuleStatus_t steamL;
        public ModuleStatus_t steamR;
        public ModuleStatus_t water;
        public byte[] remainder;
    }

    /******************************************************************************/
    /** ProduktTyp (Rezept) */
    public enum ProductType_t
    {
        None_e = 0,
        Ristretto_e,
        Espresso_e,
        Coffee_e,
        FilterCoffee_e,
        Americano_e,
        CoffeePot_e,
        FilterCoffeePot_e = 7,

        HotWater_e,

        ManualSteam_e,
        AutoSteam_e,
        Everfoam_e = 11,

        MilkCoffee_e,
        Cappuccino_e,
        EspressoMacchiato_e,
        LatteMacchiato_e,
        Milk_e,
        MilkFoam_e = 17,

        Max_e,
        Undef_e = 0xFF
    }

    /******************************************************************************/
    /** ProduktTemperatur */
    public enum MilkTemp_t
    {
        MlkTempWarm_e = 0,
        MlkTempCold_e,
        MlkTempWarmCold_e,
        MlkTempColdWarm_e,
        MlkTempMax_e,
        MlkTempUndef_e = 0xFF
    }

    /******************************************************************************/
    /** MilchSequenz */
    public enum MilkSequence_t
    {
        MilkSeqCofThenMilk_e = 0,
        MilkSeqMilkThenCof_e,
        MilkSeqCofPlusMilk_e,
        MilkSeqMilkOnly_e,
        MilkSeqCofDelayedMilk_e,

        MilkSeqMax_e,
        MilkSeqUndef_e = 0xFF
    }

    /******************************************************************************/
    /** ProduktProcess (Rezept) */
    public enum ProductProcess_t
    {
        ProProcCoffee_e = 0,
        ProProcSteam_e,
        ProProcWater_e,
        ProProcPowder_e,
        ProProcMax_e,
        ProProcLearn_e = 0x10,          // Bit 4: Lern-Prozess
        ProProcMilkAdjust_e = 0x20,     // Bit 5: Milch Arbeitspunkt-Einstellung
        ProProcAirAdjust_e = 0x40,      // Bit 6: Milch Luftregler-Einstellung
        ProProcGrinderCal_e = 0x80,     // Bit 7: Mühlen-Kalibrierung
        ProProcUndef_e = 0xFF
    };


    /******************************************************************************/
    /** MilchSequenz */
    public enum FoamSequence_t
    {
        FoamSeqFoamThenMilk_e = 0,
        FoamSeqMilkThenFoam_e,

        FoamSeqMax_e,
        FoamSeqUndef_e = 0xFF
    }

    /******************************************************************************/
    /** KaffeeSorte */
    public enum CoffeeSort_t
    {
        CofSortOne_e = 0,
        CofSortTwo_e,
        CofSortMix_e,
        CofSortExt_e,
        CofSortMax_e,
        CofSortUndef_e = 0xFF
    }

    /******************************************************************************/
    /** Bean Hopper */
    public enum BeanHopper_t
    {
        Front_e = 0,
        Rear_e,
        Mix_e,
        PowderChute_e,
        Max_e,
        Undef_e = 0xFF
    }
    /******************************************************************************/
    /** Coffee Outlet */
    public enum CoffeeOutlet_t
    {
        CofOutCentral_e = 0,
        CofOutPot_e,
        CofOutMax_e,
        CofOutUndef_e = 0xFF
    }

    /******************************************************************************/
    /** Referenzprodukt für Extraktionszeit-Regelung */
    public enum PRD_RefExtractTime_t
    {
        RefExtractTimeNone_e = 0,
        RefExtractTimeMaster_e,
        RefExtractTimeSlave_e,
        RefExtractTimeMax_e
    }

    /******************************************************************************/
    /** MilchSorte */
    public enum MilkSort_t
    {
        MlkSortOne_e = 0,
        MlkSortTwo_e,
        MlkSortMax_e,
        MlkSortUndef_e = 0xFF
    }

    public enum EverfoamMode_t
    {
        NTC_e = 0,
        Time_e,
        Ignored_e,

        Max_e
    }

    public enum ProductStatus_t
    {
        Ready_e = 0,
        Queued_e,
        Started_e,
        RunningAndCanBeStoped_e,
        Ending_e,
        Disabled_e,
        Suspended_e,
    }

    /******************************************************************************/
    /** \brief Parametersatz für Produktdaten
               IAR: 27 Byte >  record = 32 Byte
               WIN: 48 Byte >  record = 64 Byte
    */
    public class ProductParameter_t : ICloneable
    {
        private const int DATA_LENGTH = 42;     // Paket-Länge die zur CPU gesendet wird

        // Adresse des Parametersatzes
        public int productId;                   // #    ID des ausgelösten Produktes vom Touch

        // Allgemeine Produkt-Parameter
        public ProductType_t productType = ProductType_t.None_e;                    // enum
        public ProductProcess_t productProcess = ProductProcess_t.ProProcCoffee_e;  // enum
        public int productCyc;                  // #
        public int productNbr;                  // #    Wird für Process-Dump verwendet
        public bool doubleProduct;              // 0/1  Wird für Process-Dump verwendet (Touch: Statistik)
        public bool referenceKey = true;        // 0/1  Referenztaste (Daten auf dem anderen Panel werden automatisch synchronisiert)

        // Wasser Parameter
        public int waterQnty;                   // ticks
        public int hotWaterQnty;                // ticks für Americano

        // Kaffee-Parameter
        public BeanHopper_t beanHopper;         // enum
        public CoffeeOutlet_t coffeeOutlet;     // enum
        public double cakeThickness;            // mm/10
        public int powderPress;                 // eUnit
        public int coffeeAroma;                 // %
        public double prebrewTime;              // sec/10
        public double relaxTime;                // sec/10
        public double pressAfter;               // mm/10
        public int bypassQnty;                  // %
        public PRD_RefExtractTime_t refExtractTime = PRD_RefExtractTime_t.RefExtractTimeNone_e;          // enum
        public int desiredExtractionTimeMin = 20; // sec
        public int desiredExtractionTimeMax = 26; // sec

        // Milch-Parameter
        public MilkSort_t milkSort;             // enum
        public double milkQnty;                 // sec
        public MilkTemp_t milkTemp;             // enum
        public int milkPercent;                 // %
        public MilkSequence_t milkSequence;     // enum
        public double milkDelayTime = 5.0;      // sec/10
        public int latteMacchTime;              // sec
        public FoamSequence_t foamSequence = FoamSequence_t.FoamSeqFoamThenMilk_e;

        // Dampf -Parameter
        public int steamTime;                   // sec
        public int steamTemp;                   // grad
        public int airStopTemp;                 // grad
        public double airStopTime;                 // sec/10

        // Luft-Parameter
        public int airQuantity = 0;             // %

        // Pumpen Parameter
        public int pumpSpeedMilk = 0;               // rpm
        public int pumpSpeedFoam = 0;               // rpm

        // Ab hier Parameter nur im Touch verwendet
        // Everfoam Mode
        public EverfoamMode_t everfoamMode = EverfoamMode_t.Time_e;

        // Icon-Nummer
        public int iconNbr = 0;

        // Name des Produkts
        public string prodName = "";

        // Status
#if WindowsCE
        public ProductStatus_t prodStatus = ProductStatus_t.Disabled_e;
        public ProductStatus_t prodStatusOld = ProductStatus_t.Disabled_e;
#else
        public ProductStatus_t prodStatus = ProductStatus_t.Ready_e;
        public ProductStatus_t prodStatusOld = ProductStatus_t.Ready_e;
#endif


        /// <summary>Kopiert die Produkt-Daten in eine neue Instanz</summary>
        /// <returns>Kopie</returns>
        public object Clone( )
        {
            return this.MemberwiseClone( );
        }

        public byte[] ToArrayApi()
        {
            byte[] data = new byte[35];
            ushort buffer = 0;

            // Param 1-5
            data[0] = (byte)this.productType;
            data[1] = (byte)this.productProcess;
            buffer = (ushort)this.waterQnty;
            data[2] = (byte)(buffer & 0x00FF);
            data[3] = (byte)(buffer >> 8);
            data[4] = (byte)this.beanHopper;
            buffer = (ushort)Math.Round(this.cakeThickness * 10);
            data[5] = (byte)(buffer & 0x00FF);
            data[6] = (byte)(buffer >> 8);

            // Param 6-10
            data[7] = (byte)this.powderPress;
            data[8] = (byte)Math.Round(this.prebrewTime * 10);
            data[9] = (byte)Math.Round(this.relaxTime * 10);
            data[10] = (byte)Math.Round(this.pressAfter * 10);
            buffer = (ushort)Math.Round(this.milkQnty * 10);
            data[11] = (byte)(buffer & 0x00FF);
            data[12] = (byte)(buffer >> 8);

            // Param 11-15
            data[13] = (byte)this.milkTemp;
            data[14] = (byte)this.milkPercent;
            data[15] = (byte)this.milkSequence;
            data[16] = (byte)this.latteMacchTime;
            data[17] = (byte)this.foamSequence;

            // Param 16-20
            data[18] = (byte)(this.steamTime & 0x00FF);
            data[19] = (byte)(this.steamTime >> 8);
            data[20] = (byte)this.steamTemp;
            data[21] = (byte)this.everfoamMode;
            data[22] = (byte)this.airStopTemp;
            buffer = (ushort)Math.Round(this.airStopTime * 10);
            data[23] = (byte)(buffer & 0x00FF);
            data[24] = (byte)(buffer >> 8);

            // Param 22-23
            data[25] = (byte)(this.pumpSpeedMilk & 0x0FF);
            data[26] = (byte)(this.pumpSpeedMilk >> 8);
            data[27] = (byte)(this.pumpSpeedFoam & 0x0FF);
            data[28] = (byte)(this.pumpSpeedFoam >> 8);

            // Param 24
            data[29] = ( byte )( this.airQuantity );

            data[30] = ( byte )( ( int )( 10 * this.milkDelayTime ) & 0x0FF );
            data[31] = ( byte )( ( int )( 10 * this.milkDelayTime ) >> 8 );

            
            data[32] = ( byte )( this.hotWaterQnty & 0x0FF );
            data[33] = ( byte )( this.hotWaterQnty >> 8 );

            data[34] = ( byte )( this.bypassQnty );
            
            return data;
        }

        public ProductParameter_t(SerialComm.Packet_t p)
        {
            FromArrayApi(p.data);
        }
        public ProductParameter_t()
        {
        }

        public void FromArrayApi(byte[] data)
        {
            // Param 1-5
            this.productType = (ProductType_t)data[0];
            this.productProcess = (ProductProcess_t)data[1];
            this.waterQnty = (data[2] | (data[3] << 8));
            this.beanHopper = (BeanHopper_t)data[4];
            this.cakeThickness = (double)((int)data[5] | ((int)data[6] << 8)) / 10.0;

            // Param 6-10
            this.powderPress = data[7];
            this.prebrewTime = (double)data[8] / 10.0;
            this.relaxTime = (double)data[9] / 10.0;
            this.pressAfter = (double)((sbyte)data[10]) / 10.0;
            this.milkQnty = Convert.ToDouble((int)data[11] | ((int)data[12] << 8)) / 10.0;

            // Param 11-15
            this.milkTemp = (MilkTemp_t)data[13];
            this.milkPercent = data[14];
            this.milkSequence = (MilkSequence_t)data[15];
            this.latteMacchTime = data[16];
            this.foamSequence = (FoamSequence_t)data[17];


            if (data.Length >= 30)
            {
                // Param 16-20
                this.steamTime = (int)((int)data[18] + ((int)data[19] << 8));
                this.steamTemp = (int)data[20];
                this.everfoamMode = (EverfoamMode_t)data[21];
                this.airStopTemp = (int)data[22];
                this.airStopTime = (double)data[23];

                // Param 21; reserved

                // Param 22-23
                this.pumpSpeedMilk = (int)((int)data[25] + ((int)data[26] << 8));
                this.pumpSpeedFoam = (int)((int)data[27] + ((int)data[28] << 8));

                // Param 24; reserved
            }
            else
            {
                // Param 16-20
                this.steamTime = 1;
                this.steamTemp = 30;
                this.everfoamMode = EverfoamMode_t.Time_e;
                this.airStopTemp = 0;
                this.airStopTime = 10;

                // Param 21; reserved

                // Param 22-23
                this.pumpSpeedMilk = 100;
                this.pumpSpeedFoam = 100;

                // Param 24; reserved
            }
        }

        /// <summary>Kopiert die Produktparameter in ein Array , damit diese via RS485 an die CPU gesendet werden können</summary>
        /// <returns>Array mit den Produktparametern</returns>
        public byte[] ToArrayRS485Cpu( )
        {
            byte[] prodData = new byte[DATA_LENGTH];

            // Adresse des Parametersatzes
            prodData[0] = ( byte )this.productId;

            // Allgemeine Produkt-Parameter
            prodData[1] = ( byte )this.productType;
            prodData[2] = ( byte )this.productProcess;
            prodData[3] = ( byte )this.productCyc;
            prodData[4] = ( byte )( this.productCyc >> 8 );
            prodData[5] = ( byte )this.productNbr;
            prodData[6] = ( byte )( this.productNbr >> 8 );
            prodData[7] = Convert.ToByte( this.doubleProduct );
            prodData[8] = Convert.ToByte( this.referenceKey );

            // Wasser Parameter
            prodData[9] = ( byte )this.waterQnty;
            prodData[10] = ( byte )( this.waterQnty >> 8 );
            prodData[11] = ( byte )this.hotWaterQnty;
            prodData[12] = ( byte )( this.hotWaterQnty >> 8 );

            // Kaffee-Parameter
            prodData[13] = ( byte )this.beanHopper;
            prodData[14] = ( byte )this.coffeeOutlet;
            prodData[15] = ( byte )( int )( this.cakeThickness * 10 );
            prodData[16] = ( byte )( ( int )( this.cakeThickness * 10 ) >> 8 );
            prodData[17] = ( byte )( this.powderPress );
            prodData[18] = ( byte )this.coffeeAroma;
            prodData[19] = ( byte )( this.prebrewTime * 10 );
            prodData[20] = ( byte )( this.relaxTime * 10 );
            prodData[21] = ( byte )( this.pressAfter * 10 );
            prodData[22] = ( byte )this.bypassQnty;
            prodData[23] = ( byte )this.refExtractTime;
            prodData[24] = ( byte )this.desiredExtractionTimeMin;
            prodData[25] = ( byte )this.desiredExtractionTimeMax;

            // Milch-Parameter
            prodData[26] = ( byte )this.milkSort;
            prodData[27] = ( byte )( this.milkQnty * 10 );
            prodData[28] = ( byte )( ( int )( this.milkQnty * 10 ) >> 8 );
            prodData[29] = ( byte )this.milkTemp;
            prodData[30] = ( byte )this.milkPercent;
            prodData[31] = ( byte )this.milkSequence;
            prodData[32] = ( byte )( int )( this.milkDelayTime * 10 );
            prodData[33] = ( byte )( ( int )( this.milkDelayTime * 10 ) >> 8 );
            prodData[34] = ( byte )this.latteMacchTime;
            prodData[35] = ( byte )this.foamSequence;

            // Dampf-Parameter
            prodData[36] = ( byte )( this.steamTime );
            prodData[37] = ( byte )( ( int )( this.steamTime ) >> 8 );
            prodData[38] = ( byte )this.steamTemp;

            if( this.everfoamMode == EverfoamMode_t.NTC_e )
            {
                // Muss null sein, damit die CPU weiss, dass die Luftpumpe via Airstop abgeschaltet werden soll
                prodData[39] = 0;       // AirStopTime
                prodData[40] = 0;       // AirStopTime
            }
            else if( this.everfoamMode == EverfoamMode_t.Time_e )
            {
                prodData[39] = ( byte )( this.steamTime );                          // AirStopTime
                prodData[40] = ( byte )( ( int )( this.steamTime ) >> 8 );          // AirStopTime
            }

            prodData[41] = ( byte )this.airStopTemp;    // AirStopTemp

            return prodData;
        }

        /// <summary>Kopiert die Produktparameter in ein Array , damit diese via RS485 an das andere Panel gesendet werden können</summary>
        /// <returns>Array mit den Produktparametern</returns>
        public byte[] ToArrayRS485Panel( )
        {
            byte[] prodName = Encoding.UTF8.GetBytes( this.prodName );
            byte[] prodData = this.ToArrayRS485Cpu( );
            int dataOffset = DATA_LENGTH;

            // Touch-Parameter addieren
            Array.Resize( ref prodData, dataOffset + prodName.Length + 4 );

            // Everfoam Mode
            prodData[dataOffset] = ( byte )this.everfoamMode;

            // Icon
            prodData[dataOffset + 1] = ( byte )this.iconNbr;
            prodData[dataOffset + 2] = ( byte )( ( int )this.iconNbr >> 8 );

            // Produkt-Name
            prodData[dataOffset + 3] = ( byte )prodName.Length;
            Array.Copy( prodName, 0, prodData, dataOffset + 4, prodName.Length );

            return prodData;
        }

        /// <summary>Kopiert die RS485 Daten in die Produktparameter-Struktur um.</summary>
        /// <param name="data">Daten-Buffer</param>
        public void FromArrayRS485( byte[] data )
        {
            try
            {
                // Adresse des Parametersatzes
                this.productId = ( byte )data[0];

                // Allgemeine Produkt-Parameter
                this.productType = ( ProductType_t )data[1];
                this.productProcess = ( ProductProcess_t )data[2];
                this.productCyc = ( ( int )data[3] | ( ( int )data[4] << 8 ) );
                this.productNbr = ( ( int )data[5] | ( ( int )data[6] << 8 ) );
                this.doubleProduct = Convert.ToBoolean( data[7] );
                this.referenceKey = Convert.ToBoolean( data[8] );

                // Wasser Parameter
                this.waterQnty = ( ( int )data[9] | ( ( int )data[10] << 8 ) );
                this.hotWaterQnty = ( ( int )data[11] | ( ( int )data[12] << 8 ) );

                // Kaffee-Parameter
                this.beanHopper = ( BeanHopper_t )data[13];
                this.coffeeOutlet = ( CoffeeOutlet_t )data[14];
                this.cakeThickness = ( double )( ( int )data[15] | ( ( int )data[16] << 8 ) ) / 10.0;
                this.powderPress = data[17];
                this.coffeeAroma = data[18];
                this.prebrewTime = ( double )data[19] / 10.0;
                this.relaxTime = ( double )data[20] / 10.0;
                this.pressAfter = Convert.ToDouble( ( sbyte )( data[21] ) ) / 10.0;
                this.bypassQnty = data[22];
                this.refExtractTime = ( PRD_RefExtractTime_t )data[23];
                this.desiredExtractionTimeMin = data[24];
                this.desiredExtractionTimeMax = data[25];

                // Milch-Parameter
                this.milkSort = ( MilkSort_t )data[26];
                this.milkQnty = Convert.ToDouble( ( int )data[27] | ( ( int )data[28] << 8 ) ) / 10.0;
                this.milkTemp = ( MilkTemp_t )data[29];
                this.milkPercent = data[30];
                this.milkSequence = ( MilkSequence_t )data[31];
                this.milkDelayTime = ( double )( ( int )data[32] | ( ( int )data[33] << 8 ) ) / 10.0;
                this.latteMacchTime = data[34];
                this.foamSequence = ( FoamSequence_t )data[35];

                // Dampf-Parameter
                this.steamTime = ( ( int )data[36] | ( ( int )data[37] << 8 ) );
                this.steamTemp = data[38];
                //this.airStopTime = ( ( int )data[39] | ( ( int )data[40] << 8 ) );
                //this.airStopTemp = data[41];
                this.airStopTemp = data[41];

                if( data.Length > ProductParameter_t.DATA_LENGTH )
                {
                    // Touch-Parameter addieren
                    int dataOffset = ProductParameter_t.DATA_LENGTH;

                    // Everfoam Mode
                    this.everfoamMode = ( EverfoamMode_t )data[dataOffset];

                    // Icon
                    this.iconNbr = ( ( int )data[dataOffset + 1] | ( ( int )data[dataOffset + 2] << 8 ) );

                    // 

                    // Produkt-Name
                    int prodNameLength = data[dataOffset + 3];
                    this.prodName = Encoding.UTF8.GetString( data, dataOffset + 4, prodNameLength );
                }
            }
            catch
            {
            }
        }
    }

    /// <summary>Prozess Dump-Daten</summary>
    public class ProcDump_t : ICloneable
    {
        public DAT_Date_t date;
        public DAT_Time_t time;
        public double cakePressBefore;
        public double cakePressAfter;
        public double cakePressFinal;
        public double cakePressHub;
        public ProcSide_t side;
        public double powderQty;
        public OnOffCtrl_t pwdrQntyCtrl;
        public double extractTime;
        public int waterQnty;
        public int waterTemp;           // Wassertemperatur Boiler-Auslauf, °C
        public int boilerTemp;          // Boilertemperatur Heizungs-Regelung, °C
        public ProductType_t prodType;
        public bool doubleProd;
        public int keyId;
        public BeanHopper_t beanHopp;
        public ProcSide_t outSide;
        public ProdAbortType_t prodAbort;
        public int grindAdjustLeft;
        public int grindAdjustRight;
        public int refExtractTime;
        public int milkTemp;           // Milchtemperatur in °C
        public int steamPress;         // Dampfdruck in bar/10
        public int milkTime;           // Milchauslaufzeit ganzes Produkt in sek/10
        public int rpmFoam;            // Drehzahl Schaum
        public int rpmMilk;            // Drehzahl Milch
        public int mctTempFoam;        // Temperatur Schaum in °C
        public int mctTempMilk;        // Temperatur Milch in °C
        public int mctTimeFoam;        // Zeit Schaum in sek/10
        public int mctTimeMilk;        // Zeit Milch in sek/10
        public int milkInputTemp;      // Milcheingangstemperatur in °C
        public int airQuantity;        // Luftmenge in %

        /// <summary>Kopiert die Prozess-Dump-Daten in eine neue Instanz</summary>
        /// <returns>Kopie</returns>
        public object Clone( )
        {
            return this.MemberwiseClone( );
        }

        /// <summary>Kopiert die RS485 Daten von Kaffee/Milch-Produkt in die Prozess-Dump-Daten-Struktur um.</summary>
        /// <param name="data">Daten-Buffer</param>
        public void FromArrayRS485Coffee( byte[] data )
        {
            try
            {
                this.date.day = data[0];
                this.date.month = data[1];
                this.date.year = data[2];
                this.time.second = data[3];
                this.time.minute = data[4];
                this.time.hour = data[5];
                this.cakePressBefore = ( ( int )data[6] | ( ( int )data[7] << 8 ) );
                this.cakePressAfter = ( ( int )data[8] | ( ( int )data[9] << 8 ) );
                this.cakePressFinal = ( ( int )data[10] | ( ( int )data[11] << 8 ) );
                this.cakePressHub = ( ( int )data[12] | ( ( int )data[13] << 8 ) );
                this.side = ( ProcSide_t )data[14];
                this.powderQty = data[15];
                this.pwdrQntyCtrl = ( OnOffCtrl_t )data[16];
                this.extractTime = ( ( int )data[17] | ( ( int )data[18] << 8 ) );
                this.waterQnty = ( ( int )data[19] | ( ( int )data[20] << 8 ) );
                this.waterTemp = ( ( int )data[21] | ( ( int )data[22] << 8 ) );
                this.boilerTemp = ( ( int )data[23] | ( ( int )data[24] << 8 ) );
                this.prodType = ( ProductType_t )data[25];
                this.doubleProd = Convert.ToBoolean( data[26] );
                this.keyId = ( int )data[27];
                this.beanHopp = ( BeanHopper_t )data[28];
                this.outSide = ( ProcSide_t )data[29];
                this.prodAbort = ( ProdAbortType_t )data[30];
                this.grindAdjustLeft = ( ( int )data[31] | ( ( int )data[32] << 8 ) );
                this.grindAdjustRight = ( ( int )data[33] | ( ( int )data[34] << 8 ) );
                this.refExtractTime = ( int )data[35];
                this.milkTemp = ( int )data[36];
                this.steamPress = ( int )data[37];
                this.milkTime = ( ( int )data[38] | ( ( int )data[39] << 8 ) );
                this.rpmFoam = ( ( int )data[40] | ( ( int )data[41] << 8 ) );
                this.rpmMilk = ( ( int )data[42] | ( ( int )data[43] << 8 ) );
                this.mctTempFoam = data[44];
                this.mctTempMilk = data[45];
                this.mctTimeFoam = ( ( int )data[46] | ( ( int )data[47] << 8 ) );
                this.mctTimeMilk = ( ( int )data[48] | ( ( int )data[49] << 8 ) );
                this.milkInputTemp = data[50];
                this.airQuantity = data[51];
            }
            catch( Exception ex )
            {
                //MessageBox.Show( ex.ToString( ) );
            }
        }

        /// <summary>Kopiert die RS485 Daten von Dampf/Heisswasser-Produkt in die Prozess-Dump-Daten-Struktur um.</summary>
        /// <param name="data">Daten-Buffer</param>
        public void FromArrayRS485Steam( byte[] data )
        {
            try
            {
                this.date.day = data[0];
                this.date.month = data[1];
                this.date.year = data[2];
                this.time.second = data[3];
                this.time.minute = data[4];
                this.time.hour = data[5];
                this.extractTime = ( ( int )data[6] | ( ( int )data[7] << 8 ) );
                this.waterQnty = ( ( int )data[8] | ( ( int )data[9] << 8 ) );
                this.waterTemp = ( ( int )data[10] | ( ( int )data[11] << 8 ) );
                this.prodType = ( ProductType_t )data[12];
                this.keyId = ( int )data[13];
                this.outSide = ( ProcSide_t )data[14];
                this.prodAbort = ( ProdAbortType_t )data[15];
                this.steamPress = ( int )data[16];
                this.airQuantity = data[17];

                // Die nicht verwendeten Parameter initialisieren
                this.cakePressBefore = 0;
                this.cakePressAfter = 0;
                this.cakePressFinal = 0;
                this.cakePressHub = 0;
                this.side = this.outSide;
                this.powderQty = 0;
                this.pwdrQntyCtrl = OnOffCtrl_t.Off_e;
                this.boilerTemp = 0;
                this.doubleProd = false;
                this.beanHopp = BeanHopper_t.Undef_e;
                this.grindAdjustLeft = 0;
                this.grindAdjustRight = 0;
                this.refExtractTime = 0;
                this.milkTemp = 0;
                this.milkTime = 0;
                this.rpmFoam = 0;
                this.rpmMilk = 0;
                this.mctTempFoam = 0;
                this.mctTempMilk = 0;
                this.mctTimeFoam = 0;
                this.mctTimeMilk = 0;
                this.milkInputTemp = 0;
            }
            catch
            {
            }
        }

        public byte[] ToArrayApi( )
        {
            byte[] data = new byte[52];

            data[0] = this.date.day;
            data[1] = this.date.month;
            data[2] = this.date.year;
            data[3] = this.time.second;
            data[4] = this.time.minute;
            data[5] = this.time.hour;
            data[6] = ( byte )this.cakePressBefore;
            data[7] = ( byte )( ( int )this.cakePressBefore >> 8 );
            data[8] = ( byte )this.cakePressAfter;
            data[9] = ( byte )( ( int )this.cakePressAfter >> 8 );
            data[10] = ( byte )this.cakePressFinal;
            data[11] = ( byte )( ( int )this.cakePressFinal >> 8 );
            data[12] = ( byte )this.cakePressHub;
            data[13] = ( byte )( ( int )this.cakePressHub >> 8 );
            data[14] = ( byte )this.side;
            data[15] = ( byte )this.powderQty;
            data[16] = ( byte )this.pwdrQntyCtrl;
            data[17] = ( byte )this.extractTime;
            data[18] = ( byte )( ( int )this.extractTime >> 8 );
            data[19] = ( byte )this.waterQnty;
            data[20] = ( byte )( this.waterQnty >> 8 );
            data[21] = ( byte )this.waterTemp;
            data[22] = ( byte )( this.waterTemp >> 8 );
            data[23] = ( byte )this.prodType;
            data[24] = Convert.ToByte( this.doubleProd );
            data[25] = ( byte )this.keyId;
            data[26] = ( byte )this.beanHopp;
            data[27] = ( byte )this.outSide;
            data[28] = ( byte )this.prodAbort;
            data[29] = ( byte )this.grindAdjustLeft;
            data[30] = ( byte )( this.grindAdjustLeft >> 8 );
            data[31] = ( byte )this.grindAdjustRight;
            data[32] = ( byte )( this.grindAdjustRight >> 8 );
            data[33] = ( byte )this.refExtractTime;
            data[34] = ( byte )this.milkTemp;
            data[35] = ( byte )this.steamPress;
            data[36] = ( byte )this.milkTime;
            data[37] = ( byte )( this.milkTime >> 8 );
            data[38] = ( byte )this.rpmFoam;
            data[39] = ( byte )( this.rpmFoam >> 8 );
            data[40] = ( byte )this.rpmMilk;
            data[41] = ( byte )( this.rpmMilk >> 8 );
            data[42] = ( byte )this.boilerTemp;
            data[43] = ( byte )( this.boilerTemp >> 8 );
            data[44] = ( byte )this.mctTempFoam;
            data[45] = ( byte )this.mctTempMilk;
            data[46] = ( byte )this.mctTimeFoam;
            data[47] = ( byte )( this.mctTimeFoam >> 8 );
            data[48] = ( byte )this.mctTimeMilk;
            data[49] = ( byte )( this.mctTimeMilk >> 8 );
            data[50] = ( byte )this.milkInputTemp;
            data[51] = ( byte )this.airQuantity;

            return data;
        }

        public void FromArrayApi( byte[] data )
        {
            this.date.day = data[0];
            this.date.month = data[1];
            this.date.year = data[2];
            this.time.second = data[3];
            this.time.minute = data[4];
            this.time.hour = data[5];
            this.cakePressBefore = ( ( int )data[6] | ( ( int )data[7] << 8 ) );
            this.cakePressAfter = ( ( int )data[8] | ( ( int )data[9] << 8 ) );
            this.cakePressFinal = ( ( int )data[10] | ( ( int )data[11] << 8 ) );
            this.cakePressHub = ( ( int )data[12] | ( ( int )data[13] << 8 ) );
            this.side = ( ProcSide_t )data[14];
            this.powderQty = data[15];
            this.pwdrQntyCtrl = ( OnOffCtrl_t )data[16];
            this.extractTime = ( ( int )data[17] | ( ( int )data[18] << 8 ) );
            this.waterQnty = ( ( int )data[19] | ( ( int )data[20] << 8 ) );
            this.waterTemp = ( ( int )data[21] | ( ( int )data[22] << 8 ) );
            this.prodType = ( ProductType_t )data[23];
            this.doubleProd = Convert.ToBoolean( data[24] );
            this.keyId = ( int )data[25];
            this.beanHopp = ( BeanHopper_t )data[26];
            this.outSide = ( ProcSide_t )data[27];
            this.prodAbort = ( ProdAbortType_t )data[28];
            this.grindAdjustLeft = ( ( int )data[29] | ( ( int )data[30] << 8 ) );
            this.grindAdjustRight = ( ( int )data[31] | ( ( int )data[32] << 8 ) );
            this.refExtractTime = ( int )data[33];
            this.milkTemp = ( int )data[34];
            this.steamPress = ( int )data[35];
            this.milkTime = ( ( int )data[36] | ( ( int )data[37] << 8 ) );
            this.rpmFoam = ( ( int )data[38] | ( ( int )data[39] << 8 ) );
            this.rpmMilk = ( ( int )data[40] | ( ( int )data[41] << 8 ) );
            this.boilerTemp = ( ( int )data[42] | ( ( int )data[43] << 8 ) );
            this.mctTempFoam = data[44];
            this.mctTempMilk = data[45];
            this.mctTimeFoam = ( ( int )data[46] | ( ( int )data[47] << 8 ) );
            this.mctTimeMilk = ( ( int )data[48] | ( ( int )data[49] << 8 ) );
            this.milkInputTemp = data[50];
            this.airQuantity = data[51];
        }
    }

    public struct DAT_DateTime_t
    {
        public short year;
        public byte month;                /* 1 .. 12 */
        public byte day;                  /* 1 .. 31 */
        public byte hour;                 /* 0 .. 23 */
        public byte minute;               /* 0 .. 59 */
        public byte second;               /* 0 .. 59 */
    }   /**< typ fuer Datum und Zeit */


    /// <summary>
    /// Zeit in Stunden, Minuten und Sekunden
    /// </summary>
    public struct DAT_Time_t
    {
        public byte second;
        public byte minute;
        public byte hour;

        public override string ToString( )
        {
            if( hour >= 1 && hour <= 24 )
                return hour.ToString( "00" ) + ":" + minute.ToString( "00" ) + ":" + second.ToString( "00" );
            else
                return "";
        }
    }

    /// <summary>
    /// Datum in Jahren (2-stellig), Monate und Tage
    /// Spezifikation Monat: Januar = 1, Februar = 2, usw.
    /// </summary>
    public struct DAT_Date_t
    {
        public byte day;
        public byte month;
        public byte year;

        public override string ToString( )
        {
            if( day >= 1 && day <= 31 )
                return day.ToString( "00" ) + "." + month.ToString( "00" ) + "." + ( year + 2000 ).ToString( "0000" );
            else
                return "";
        }
    }




    /** Milchregelung-Status */
    public enum MCT_Status_t
    {
        MilkCtrlStatusNone_e = 0,
        MilkCtrlStatusTempOk_e,
        MilkCtrlStatusAdjusted_e,
        MilkCtrlStatusLimit_e
    }


    #region enum HChangeNotifyEventID
    /// <summary>
    /// Describes the event that has occurred. 
    /// Typically, only one event is specified at a time. 
    /// If more than one event is specified, the values contained 
    /// in the <i>dwItem1</i> and <i>dwItem2</i> 
    /// parameters must be the same, respectively, for all specified events. 
    /// This parameter can be one or more of the following values. 
    /// </summary>
    /// <remarks>
    /// <para><b>Windows NT/2000/XP:</b> <i>dwItem2</i> contains the index 
    /// in the system image list that has changed. 
    /// <i>dwItem1</i> is not used and should be <see langword="null"/>.</para>
    /// <para><b>Windows 95/98:</b> <i>dwItem1</i> contains the index 
    /// in the system image list that has changed. 
    /// <i>dwItem2</i> is not used and should be <see langword="null"/>.</para>
    /// </remarks>
    [Flags]
    enum HChangeNotifyEventID
    {
        /// <summary>
        /// All events have occurred. 
        /// </summary>
        SHCNE_ALLEVENTS = 0x7FFFFFFF,

        /// <summary>
        /// A file type association has changed. <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> 
        /// must be specified in the <i>uFlags</i> parameter. 
        /// <i>dwItem1</i> and <i>dwItem2</i> are not used and must be <see langword="null"/>. 
        /// </summary>
        SHCNE_ASSOCCHANGED = 0x08000000,

        /// <summary>
        /// The attributes of an item or folder have changed. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the item or folder that has changed. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>.
        /// </summary>
        SHCNE_ATTRIBUTES = 0x00000800,

        /// <summary>
        /// A nonfolder item has been created. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the item that was created. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>.
        /// </summary>
        SHCNE_CREATE = 0x00000002,

        /// <summary>
        /// A nonfolder item has been deleted. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the item that was deleted. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_DELETE = 0x00000004,

        /// <summary>
        /// A drive has been added. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the root of the drive that was added. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_DRIVEADD = 0x00000100,

        /// <summary>
        /// A drive has been added and the Shell should create a new window for the drive. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the root of the drive that was added. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_DRIVEADDGUI = 0x00010000,

        /// <summary>
        /// A drive has been removed. <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the root of the drive that was removed.
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_DRIVEREMOVED = 0x00000080,

        /// <summary>
        /// Not currently used. 
        /// </summary>
        SHCNE_EXTENDED_EVENT = 0x04000000,

        /// <summary>
        /// The amount of free space on a drive has changed. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the root of the drive on which the free space changed.
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_FREESPACE = 0x00040000,

        /// <summary>
        /// Storage media has been inserted into a drive. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the root of the drive that contains the new media. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_MEDIAINSERTED = 0x00000020,

        /// <summary>
        /// Storage media has been removed from a drive. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the root of the drive from which the media was removed. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_MEDIAREMOVED = 0x00000040,

        /// <summary>
        /// A folder has been created. <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> 
        /// or <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the folder that was created. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_MKDIR = 0x00000008,

        /// <summary>
        /// A folder on the local computer is being shared via the network. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the folder that is being shared. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_NETSHARE = 0x00000200,

        /// <summary>
        /// A folder on the local computer is no longer being shared via the network. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the folder that is no longer being shared. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_NETUNSHARE = 0x00000400,

        /// <summary>
        /// The name of a folder has changed. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the previous pointer to an item identifier list (PIDL) or name of the folder. 
        /// <i>dwItem2</i> contains the new PIDL or name of the folder. 
        /// </summary>
        SHCNE_RENAMEFOLDER = 0x00020000,

        /// <summary>
        /// The name of a nonfolder item has changed. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the previous PIDL or name of the item. 
        /// <i>dwItem2</i> contains the new PIDL or name of the item. 
        /// </summary>
        SHCNE_RENAMEITEM = 0x00000001,

        /// <summary>
        /// A folder has been removed. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the folder that was removed. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_RMDIR = 0x00000010,

        /// <summary>
        /// The computer has disconnected from a server. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the server from which the computer was disconnected. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_SERVERDISCONNECT = 0x00004000,

        /// <summary>
        /// The contents of an existing folder have changed, 
        /// but the folder still exists and has not been renamed. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the folder that has changed. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// If a folder has been created, deleted, or renamed, use SHCNE_MKDIR, SHCNE_RMDIR, or 
        /// SHCNE_RENAMEFOLDER, respectively, instead. 
        /// </summary>
        SHCNE_UPDATEDIR = 0x00001000,

        /// <summary>
        /// An image in the system image list has changed. 
        /// <see cref="HChangeNotifyFlags.SHCNF_DWORD"/> must be specified in <i>uFlags</i>. 
        /// </summary>
        SHCNE_UPDATEIMAGE = 0x00008000,

    }
    #endregion // enum HChangeNotifyEventID



    #region public enum HChangeNotifyFlags
    /// <summary>
    /// Flags that indicate the meaning of the <i>dwItem1</i> and <i>dwItem2</i> parameters. 
    /// The uFlags parameter must be one of the following values.
    /// </summary>
    [Flags]
    public enum HChangeNotifyFlags
    {
        /// <summary>
        /// The <i>dwItem1</i> and <i>dwItem2</i> parameters are DWORD values. 
        /// </summary>
        SHCNF_DWORD = 0x0003,
        /// <summary>
        /// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of ITEMIDLIST structures that 
        /// represent the item(s) affected by the change. 
        /// Each ITEMIDLIST must be relative to the desktop folder. 
        /// </summary>
        SHCNF_IDLIST = 0x0000,
        /// <summary>
        /// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings of 
        /// maximum length MAX_PATH that contain the full path names 
        /// of the items affected by the change. 
        /// </summary>
        SHCNF_PATHA = 0x0001,
        /// <summary>
        /// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings of 
        /// maximum length MAX_PATH that contain the full path names 
        /// of the items affected by the change. 
        /// </summary>
        SHCNF_PATHW = 0x0005,
        /// <summary>
        /// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings that 
        /// represent the friendly names of the printer(s) affected by the change. 
        /// </summary>
        SHCNF_PRINTERA = 0x0002,
        /// <summary>
        /// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings that 
        /// represent the friendly names of the printer(s) affected by the change. 
        /// </summary>
        SHCNF_PRINTERW = 0x0006,
        /// <summary>
        /// The function should not return until the notification 
        /// has been delivered to all affected components. 
        /// As this flag modifies other data-type flags, it cannot by used by itself.
        /// </summary>
        SHCNF_FLUSH = 0x1000,
        /// <summary>
        /// The function should begin delivering notifications to all affected components 
        /// but should return as soon as the notification process has begun. 
        /// As this flag modifies other data-type flags, it cannot by used by itself.
        /// </summary>
        SHCNF_FLUSHNOWAIT = 0x2000
    }
    #endregion // enum HChangeNotifyFlags

}
