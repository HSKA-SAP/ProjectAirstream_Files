using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAirstream
{
    class ControlDefinitions
    {

        public enum DPT_SpecialChar_t
        {
            SOH_e = 0x01, // Start of Header (begin of packet)
            ETB_e = 0x17, // End of Transmit Block (end of packet)
            DLE_e = 0x10, // Shift Character (next character has to be XORed)
            NUL_e = 0x00, // NULL char
            STX_e = 0x02, // Start of Text
            ETX_e = 0x03, // End of Text
            EOT_e = 0x04, // End of Transmission
            LF_e = 0x0A, // Line Feed
            CR_e = 0x0D, // Carriage Return
            ModemEsc_e = 0x2B, // Standard Modem Escape Character
            ShiftXOR_e = 0x40 // Shift XOR Value
        };


        public enum API_Command_t
        {
            Reserved_e = 0,
            GetStatus_e,
            DoProduct_e,
            DoRinse_e,
            StartCleaning_e,
            Reserved1_e,
            Reserved2_e,
            Reserved3_e,
            ScreenRinse_e,
            Reserved4_e,
            Reserved5_e,
            Stop_e,
            GetRequests_e,
            GetInfoMessages_e,
            MilkOutletRinse_e,
            DisplayAction_e,
            GetProductDump_e,
            GetSensorValues_e,
        }


        public enum ProdAbortType_t
        {
            ProdFinished_e = 0, // Product hasn’t been stopped
            ProdStopped_e, // Product has been stopped (not used anymore)
            ProdAbortMachine_e, // Product has been stopped automatically
            ProdAbortUser_e // Product has been stopped manually
        }


        public enum BeanHopper_t
        {
            Front_e = 0,
            Rear_e,
            Mix_e,
            PowderChute_e,
            Max_e,
            Undef_e = 0xFF
        }


        public enum API_DisplayAction_t
        {
            GroundsBinEmptied_e = 0,
            BeanHopperRefilled_e = 1,
            MilkTankCleaned_e = 2,
            SendContinue_e = 3,
            MilkTankLeftRefilledAndFinishProduct_e = 4,
            MilkTankLeftRefilledAndAbortProduct_e = 5,
            MilkTankRightRefilledAndFinishProduct_e = 6,
            MilkTankRightRefilledAndAbortProduct_e = 7,
            RebootCpu_e = 8,
            RestartDisplay_e = 9,
        }


        public enum API_Request_t
        {
            OutletRinseLeft_e = 10,
            OutletRinseRight_e = 11,
        }


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

        public enum Action_t
        {
            ActionIdle_e = 0, // No product process is running
            ActionQueued_e, // Next product is already queued
            ActionSuspended_e, // Product process interrupted and waiting for an action
            ActionEnding_e, // Next product can already be ordered
            ActionEndCyc_e, // Only used for multi cyle products (end of cylce)
            ActionStoped_e, // Product has been stopped
            ActionStarted_e, // Product has been started
            ActionPumping_e, // Product is being dispensed
            ActionMilkInterrupt_e, // Milk tank empty => Waiting for refilling
            ActionCycleAborted_e, // Only used for multi cyle products (abortion of cylce)
            ActionPwdrChute_e, // Only used for powder chute products
            ActionCleanTabs_e, // Cleaning tabs empty => Waiting for refilling
        }

    public enum Process_t
        {
            ProcessCoffee_e = 0,
            ProcessSteam_e,
            ProcessHotWater_e,
            ProcessLearnWaterQnty_e,
            ProcessPowderTest_e,
            ProcessClean_e,
            ProcessRinse_e,
            ProcessScreenRinse_e,
            ProcessServicePos_e,
            ProcessDePressurize_e,
            ProcessEmptyBoiler_e,
            ProcessAdjPumpPress_e,
            ProcessFlowMeterTest_e,
            ProcessGrinderSensorTest_e,
            ProcessMotIni_e,
            ProcessMotIniRebootAbort_e,
            ProcessBrewMoveTest_e,
            ProcessMilkClean_e,
            ProcessOutletRinse_e,
            ProcessEmptyCofBoiler_e,
            ProcessGrinderAdjustMenu_e,
            ProcessTestBallDispenser_e,
            ProcessTestMilkPump_e,
            ProcessMilkReactorWarmup_e,
            ProcessReducePressure_e,
            ProcessTestSecurityValve_e,
            ProcessDispenseBall_e,
            ProcessMilkDetectionTest_e,
            ProcessBrewTightnessTest_e,
            ProcessUndef_e = 0xFF
        }

        public enum ProductType_t
        {
            None_e = 0,
            Ristretto_e,
            Espresso_e,
            Coffee_e,
            FilterCoffee_e,
            Americano_e,
            CoffeePot_e,
            FilterCoffeePot_e,
            HotWater_e,
            ManualSteam_e,
            AutoSteam_e,
            Everfoam_e,
            MilkCoffee_e,
            Cappuccino_e,
            EspressoMacchiato_e,
            LatteMacchiato_e,
            Milk_e,
            MilkFoam_e,
            Max_e,
            Undef_e = 0xFF
        }



    }
}
