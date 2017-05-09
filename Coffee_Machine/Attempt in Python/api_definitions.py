from enum import Enum 
## Special characters for stuffing and squeezing
class DPT_SpecialChar_t(Enum):
	SOH_e = 0x01 # Start of Header (begin of packet)
	ETB_e = 0x17 # End of Transmit Block (end of packet)
	DLE_e = 0x10 # Shift Character (next character has to be XORed)
	NUL_e = 0x00 # NULL char
	STX_e = 0x02 # Start of Text
	ETX_e = 0x03 # End of Text
	EOT_e = 0x04 # End of Transmission
	LF_e = 0x0A  # Line Feed
	CR_e = 0x0D  # Carriage Return
	ModemEsc_e = 0x2B # Standard Modem Escape Character
	ShiftXOR_e = 0x40 # Shift XOR Value

 
## API commands
class API_Command_t(Enum):
	Reserved_e = 0
	GetStatus_e = 1
	DoProduct_e = 2
	DoRinse_e = 3
	StartCleaning_e = 4
	Reserved1_e = 5
	Reserved2_e = 6
	Reserved3_e = 7
	ScreenRinse_e = 8
	Reserved4_e = 9
	Reserved5_e = 10
	Stop_e = 11
	GetRequests_e = 12
	GetInfoMessages_e = 13
	MilkOutletRinse_e = 14
	DisplayAction_e = 15
	GetProductDump_e = 16
	GetSensorValues_e = 17


## API requests 
class API_Request_t(Enum):
	OutletRinseLeft_e = 10,
	OutletRinseRight_e = 11,


#####################################################################################
#                      Commands specific  to the modules 							#
#####################################################################################

## Action status of each module

class Action_t(Enum):
	ActionIdle_e = 0, # No product process is running
	ActionQueued_e = 1 # Next product is already queued
	ActionSuspended_e = 2 # Product process interrupted and waiting for an action
	ActionEnding_e = 3# Next product can already be ordered
	ActionEndCyc_e = 4 # Only used for multi cyle products (end of cylce)
	ActionStoped_e = 5# Product has been stopped
	ActionStarted_e = 6# Product has been started
	ActionPumping_e = 7# Product is being dispensed
	ActionMilkInterrupt_e = 8 # Milk tank empty => Waiting for refilling
	ActionCycleAborted_e = 9 # Only used for multi cyle products (abortion of cylce)
	ActionPwdrChute_e = 10 # Only used for powder chute products
	ActionCleanTabs_e = 11# Cleaning tabs empty => Waiting for refilling

## Processes of each module
class Process_t(Enum):
	ProcessCoffee_e = 0,
	ProcessSteam_e = 1
	ProcessHotWater_e = 2
	ProcessLearnWaterQnty_e = 3
	ProcessClean_e = 4
	ProcessRinse_e = 5
	ProcessScreenRinse_e = 6
	ProcessServicePos_e = 7
	ProcessDePressurize_e = 8
	ProcessEmptyBoiler_e = 9
	ProcessAdjPumpPress_e = 10
	ProcessFlowMeterTest_e = 11
	ProcessGrinderSensorTest_e = 12
	ProcessMotIni_e = 13
	ProcessMotIniRebootAbort_e = 14
	ProcessBrewMoveTest_e = 15
	ProcessMilkClean_e = 16
	ProcessOutletRinse_e = 17
	ProcessEmptyCofBoiler_e = 18
	ProcessGrinderAdjustMenu_e = 19
	ProcessTestBallDispenser_e = 20
	ProcessTestMilkPump_e = 21
	ProcessMilkReactorWarmup_e = 22
	ProcessReducePressure_e = 23
	ProcessTestSecurityValve_e = 24
	ProcessDispenseBall_e = 25
	ProcessMilkDetectionTest_e = 26
	ProcessBrewTightnessTest_e = 27
	ProcessUndef_e = 0xFF


## Product types
class ProductType_t(Enum):
	None_e = 0
	Ristretto_e = 1
	Espresso_e = 2
	Coffee_e = 3
	FilterCoffee_e = 4
	Americano_e = 5
	CoffeePot_e = 6
	FilterCoffeePot_e = 7
	HotWater_e = 8
	ManualSteam_e = 9
	AutoSteam_e = 10
	Everfoam_e = 11
	MilkCoffee_e = 12
	Cappuccino_e = 13
	EspressoMacchiato_e = 14
	LatteMacchiato_e = 15
	Milk_e = 16
	MilkFoam_e = 17
	Max_e = 18
	Undef_e = 0xFF


## The different milk sequences
class MilkSequence_t(Enum):
	MilkSeqCofThenMilk_e = 0
	MilkSeqMilkThenCof_e = 1
	MilkSeqCofPlusMilk_e = 2
	MilkSeqMilkOnly_e = 3
	MilkSeqCofDelayedMilk_e = 4
	MilkSeqMax_e = 5
	MilkSeqUndef_e = 0xFF

## Display actions

class API_DisplayAction_t(Enum):
	GroundsBinEmptied_e = 0
	BeanHopperRefilled_e = 1
	MilkTankCleaned_e = 2
	SendContinue_e = 3
	MilkTankLeftRefilledAndFinishProduct_e = 4
	MilkTankLeftRefilledAndAbortProduct_e = 5
	MilkTankRightRefilledAndFinishProduct_e = 6
	MilkTankRightRefilledAndAbortProduct_e = 7
	RebootCpu_e = 8
	RestartDisplay_e = 9

class BeanHopper_t(Enum):
	Front_e = 0,
	Rear_e = 1
	Mix_e = 2
	PowderChute_e = 3
	Max_e = 4
	Undef_e = 0xFF


class ProdAbortType_t(Enum):
	ProdFinished_e = 0, # Product hasn’t been stopped
	ProdStopped_e = 1 # Product has been stopped (not used anymore)
	ProdAbortMachine_e = 2 # Product has been stopped automatically
	ProdAbortUser_e = 3 # Product has been stopped manually


class PacketTypes(Enum):
            ACK = 0x6A,
            NACK = 0x6B,
            COMMAND = 0x68,
            REQUEST = 0x6C,
            RESPONSE = 0x68,