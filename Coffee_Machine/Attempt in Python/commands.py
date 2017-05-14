import time
from telegramFormat import *
from api_definitions import *
import itertools
import struct

## GLOBALS
MILK_OUTLET_DATA_SIZE = 3
MILK_TUBE_LENGTH = 350
SCREEN_RINSE_DATA_SIZE = 2
RIGHT_SIDE = 1
LEFT_SIDE = 0

def CreateTelegram(PIP_p, PIE_p, PN_p, SA_p, DA_p, MI_p, MP_p, DL_p, dataPackets_p, data_p):
    array = [
        PIP_p,
        PIE_p,
        PN_p,
        SA_p,
        DA_p,
        MI_p,
        MP_p & 0xff, ## depends on whether RPi is little-endian
        ((MP_p >> 8) & 0xff),
        DL_p & 0xff,
        ((DL_p >> 8) & 0xff),
    ]
    if(dataPackets_p):
        for var in data_p:
            array.append(var)

    ## call to CRC
    CRC = API_CalculateCRC(array,len(array))
    array.append(CRC & 0xff)
    array.append((CRC >> 8) & 0xff)   
    ##Stuff and shift
    shifted = stuffAndShift(array)
    finalTelegram = [DPT_SpecialChar_t.SOH_e.value] + shifted + [DPT_SpecialChar_t.EOT_e.value]
    print("Telegram Created:", finalTelegram)
    return finalTelegram

def stuffAndShift(arrayToShift):
    a = arrayToShift
    i = 0
    lengthA = len(a)
    while i < lengthA:
        for j in DPT_SpecialChar_t:
            if(a[i] == j.value):
                b = a[0:i] # before
                c = a[i+1:] # after
                stuff = DPT_SpecialChar_t.DLE_e.value
                shift = a[i] ^ DPT_SpecialChar_t.ShiftXOR_e.value
                a = b+[stuff,shift] + c 
                i = i + 1
                lengthA = len(a)
                break
        i +=1
    return a

def deStuffAndShift(arrayToDeShift):
    i = 0
    while(i < len(arrayToDeShift)):
        if(arrayToDeShift[i] == DPT_SpecialChar_t.DLE_e.value):
            b = (arrayToDeShift[i+1] ^ DPT_SpecialChar_t.ShiftXOR_e.value)
            c = arrayToDeShift[i+2:]
            d = arrayToDeShift[0:i]
            arrayToDeShift = d+[b]+c
        i+=1
    return arrayToDeShift



def ByteToBitArray(num, size=8):
    """Format a number as binary with leading zeros"""
    bitArrayString = str(bin(num)[2:]).zfill(size)
    bitArray = list(map(int, bitArrayString))
    return bitArray

def LowNibble(bitArray):
    return bitArray & 0x0F

def HighNibble(bitArray):
    return bitArray >> 4


##########################################  COMMANDS #############################################


################################### SENDING PACKETS #############################

def DoCommand(ports,seqNumber,telegram):
	WaitTillReady(ports, seqNumber, telegram,False)
	return 0

def DoRequest(ports, seqNumber, telegram):
	WaitTillReady(ports, seqNumber, telegram,False)
	responseAcknowledged = False
	while(responseAcknowledged == False):
		cmResponse = ports['pi'].read_all() # Read from Raspberry Pi
		if(len(cmResponse) > 0):
			responseAcknowledged = True
    ## decrypt the message - i.e. get the status
    


def WaitTillReady(ports, seqNumber, telegram, ACKReceived):
	while(ACKReceived == False):
		ACKReceived = CheckForAck(ports, seqNumber, telegram)
		time.sleep(0.8)

def CheckForAck(ports,seqNumber,telegram):
    cmCommand = ports['cm'].write(telegram) ## send command to coffee machine 
    time.sleep(0.15)
    cmResponse = ports['pi'].read_all()  ##  read from Pi
    print("Read from Pi:",cmResponse)
    cmResponse = telegramToIntArray(cmResponse)
    cmResponseOriginal  = deStuffAndShift(cmResponse)
    print("PIE:",hex(cmResponseOriginal[2]))
    if(cmResponseOriginal[2] == PacketTypes.ACK.value): 
        print("ACK Packet Detected")
        return True
    else:
        print("Received no ACK packet from coffee machine...sending packet again")
        return False

def telegramToIntArray(telegram):
    array = []
    a = struct.unpack('B'*len(telegram),telegram)
    array.append(a)
    return list(itertools.chain.from_iterable(array))


################################### SENDING TELEGRAMS ############################    
def DoRinse(MI, MP, DL, seqNum):
    telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, MI, MP, DL, False, [0])
    return telegram

def DoRinseLeft(seqNum):
    telegram = DoRinse(API_Command_t.DoRinse_e.value,LEFT_SIDE, 0,seqNum)
    return telegram
    
def DoRinseRight(seqNum):
    telegram = DoRinse(API_Command_t.DoRinse_e.value, RIGHT_SIDE, 0,seqNum)
    return telegram

def StartClean(seqNum):
    telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, API_Command_t.StartCleaning_e.value, 0, 0, False, [0] )
    return telegram
    
def GetRequest(seqNum):
    telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, API_Command_t.GetRequests_e.value,0, 0, False, [0] )
    return telegram
    
def StopProcess(module,seqNum):
    telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, API_Command_t.Stop_e.value, module, 0x00, False, [0] )
    return telegram
    

def StopAllProcess(seqNum):
    telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, API_Command_t.Stop_e.value, 0, 0x00, False, [0] )
    return telegram
    
def RinseMilkOutlet(rinseMode,  side,  seqNum):
    array[0]*MILK_OUTLET_DATA_SIZE
    array[0] = rinseMode
    array[1] =  MILK_TUBE_LENGTH & 0xff
    array[2] = ((MILK_TUBE_LENGTH >> 8) & 0xff)
    telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, API_Command_t.MilkOutletRinse_e.value, side, MILK_OUTLET_DATA_SIZE, True, array)
    return telegram
    
def RinseRightMilkOutlet(seqNum):
    telegram = RinseMilkOutlet(1, RIGHT_SIDE,seqNum)
    return telegram
    
def RinseLeftMilkOutlet(seqNum):
     telegram = RinseMilkOutlet(1, LEFT_SIDE,seqNum)
     return telegram
    
def RinseRightTubes(seqNum):
    telegram = RinseMilkOutlet(2, RIGHT_SIDE,seqNum)
    return telegram

def RinseLeftTubes(seqNum):
    telegram = RinseMilkOutlet(2, LEFT_SIDE,seqNum)
    return telegram
    
def RinseRightTubesAndOutlet(seqNum):
    telegram = RinseMilkOutlet(0, RIGHT_SIDE,seqNum)
    return telegram
    
def RinseLeftTubesAndOutlet(seqNum):
    telegram = RinseMilkOutlet(0, LEFT_SIDE,seqNum)
    return telegram
    
def DoScreenRinse(side, seqNum):
    array[0]*SCREEN_RINSE_DATA_SIZE
    array[0] = 3 # screen rinse cycles
    array[1] = 10 #repetitions
    telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, API_Command_t.ScreenRinse_e.value, side, SCREEN_RINSE_DATA_SIZE, True, array)
    return telegram
    

def DoRightScreenRinse(seqNum):
    telegram = DoScreenRinse(RIGHT_SIDE,seqNum)
    return telegram
    
def DoLeftScreenRinse(seqNum):
    telegram = DoScreenRinse(LEFT_SIDE,seqNum)
    return telegram
    

def GetInfoMessage(seqNum):
    array = [0]*3
    telegram = CreateTelegram(0x00, 0x68,seqNum, 0x42, 0x41, API_Command_t.GetInfoMessages_e.value, 0, 3, True, array)
    return telegram
    

def DisplayAction(action, seqNum):
    telegram = CreateTelegram(0x00, 0x68,seqNum, 0x42, 0x41, API_Command_t.DisplayAction_e.value, action, 0, False, [0] )
    return telegram
    

def GetProductDump(side, seqNum):
    telegram = CreateTelegram(0x00, 0x68,seqNum, 0x42, 0x41, API_Command_t.GetProductDump_e.value, side, 0, False, [0] )
    return telegram

def GetProductDumpRight(seqNum):
    telegram = GetProductDump(RIGHT_SIDE, seqNum)
    return telegram
    
def GetProductDumpLeft(seqNum):
    telegram = GetProductDump(LEFT_SIDE,seqNum)
    return telegram
    

def GetSensorValues(seqNum):
    telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, API_Command_t.GetSensorValues_e.value, 0, 0, False, [0] )
    return telegram
    
def GetStatus(seqNum):
    telegram = CreateTelegram(0x00,0x6C, seqNum, 0x42, 0x41, API_Command_t.GetStatus_e.value,0,0,False, [0] )
    return telegram

##########################################  CRC CALCULATE #############################################
crcPolynomTable = \
    [
    0x0000,0xc0c1,0xc181,0x0140,0xc301,0x03c0,0x0280,0xc241,
    0xc601,0x06c0,0x0780,0xc741,0x0500,0xc5c1,0xc481,0x0440,
    0xcc01,0x0cc0,0x0d80,0xcd41,0x0f00,0xcfc1,0xce81,0x0e40,
    0x0a00,0xcac1,0xcb81,0x0b40,0xc901,0x09c0,0x0880,0xc841,
    0xd801,0x18c0,0x1980,0xd941,0x1b00,0xdbc1,0xda81,0x1a40,
    0x1e00,0xdec1,0xdf81,0x1f40,0xdd01,0x1dc0,0x1c80,0xdc41,
    0x1400,0xd4c1,0xd581,0x1540,0xd701,0x17c0,0x1680,0xd641,
    0xd201,0x12c0,0x1380,0xd341,0x1100,0xd1c1,0xd081,0x1040,
    0xf001,0x30c0,0x3180,0xf141,0x3300,0xf3c1,0xf281,0x3240,
    0x3600,0xf6c1,0xf781,0x3740,0xf501,0x35c0,0x3480,0xf441,
    0x3c00,0xfcc1,0xfd81,0x3d40,0xff01,0x3fc0,0x3e80,0xfe41,
    0xfa01,0x3ac0,0x3b80,0xfb41,0x3900,0xf9c1,0xf881,0x3840,
    0x2800,0xe8c1,0xe981,0x2940,0xeb01,0x2bc0,0x2a80,0xea41,
    0xee01,0x2ec0,0x2f80,0xef41,0x2d00,0xedc1,0xec81,0x2c40,
    0xe401,0x24c0,0x2580,0xe541,0x2700,0xe7c1,0xe681,0x2640,
    0x2200,0xe2c1,0xe381,0x2340,0xe101,0x21c0,0x2080,0xe041,
    0xa001,0x60c0,0x6180,0xa141,0x6300,0xa3c1,0xa281,0x6240,
    0x6600,0xa6c1,0xa781,0x6740,0xa501,0x65c0,0x6480,0xa441,
    0x6c00,0xacc1,0xad81,0x6d40,0xaf01,0x6fc0,0x6e80,0xae41,
    0xaa01,0x6ac0,0x6b80,0xab41,0x6900,0xa9c1,0xa881,0x6840,
    0x7800,0xb8c1,0xb981,0x7940,0xbb01,0x7bc0,0x7a80,0xba41,
    0xbe01,0x7ec0,0x7f80,0xbf41,0x7d00,0xbdc1,0xbc81,0x7c40,
    0xb401,0x74c0,0x7580,0xb541,0x7700,0xb7c1,0xb681,0x7640,
    0x7200,0xb2c1,0xb381,0x7340,0xb101,0x71c0,0x7080,0xb041,
    0x5000,0x90c1,0x9181,0x5140,0x9301,0x53c0,0x5280,0x9241,
    0x9601,0x56c0,0x5780,0x9741,0x5500,0x95c1,0x9481,0x5440,
    0x9c01,0x5cc0,0x5d80,0x9d41,0x5f00,0x9fc1,0x9e81,0x5e40,
    0x5a00,0x9ac1,0x9b81,0x5b40,0x9901,0x59c0,0x5880,0x9841,
    0x8801,0x48c0,0x4980,0x8941,0x4b00,0x8bc1,0x8a81,0x4a40,
    0x4e00,0x8ec1,0x8f81,0x4f40,0x8d01,0x4dc0,0x4c80,0x8c41,
    0x4400,0x84c1,0x8581,0x4540,0x8701,0x47c0,0x4680,0x8641,
    0x8201,0x42c0,0x4380,0x8341,0x4100,0x81c1,0x8081,0x4040
    ]

def API_CalculateCRC(data_p, dataLength):
    nIndex = 0
    checkSum = 0
    INIT_CRC = 0xFFFF
    checkSum = INIT_CRC

    while(nIndex < dataLength):
        checkSum = ((checkSum >> 8) ^ crcPolynomTable[(checkSum ^ data_p[nIndex]) & 0xFF])
        nIndex+=1
        
    return checkSum