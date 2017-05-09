from telegramFormat import *
from api_definitions import *
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
        MP_p & 0xff,
        ((MP_p >> 8) & 0xff),
        DL_p & 0xff,
        ((DL_p >> 8) & 0xff),
    ]
    for var in data_p:
        array.append(var)

    ## call to CRC
    CRC = 42
    array.append(CRC & 0xff)
    array.append((CRC >> 8) & 0xff)   
    ##Stuff and shift
    array = [25,25,0] ##problem
    size = (len(array))*2
    shifted =[0]*size
    TransformSerializedArray(array,shifted)
    

def TransformSerializedArray(originalArray,shiftedArray):
    positions = [0]*2
    finalIndex= 0
    i = 0
    while(i <= (len(originalArray) -1)):
        positions = RecursiveStuffAndShift(positions,originalArray[i],\
        originalArray,shiftedArray,positions[1],positions[0],0)
        CopyArrays(shiftedArray,positions[0],originalArray,positions[1])
        if(positions[1] >= (len(originalArray) - 1)):
            finalIndex = positions[0]
            break
        i = positions[1]
        positions[0]+=1
        positions[1]+=1
        i+=1
    print(originalArray)
    print(shiftedArray)
    return shiftedArray

def RecursiveStuffAndShift(positions,currentByte, originalArray,shiftedArray,\
                            cpyFromPosInOriginal,cpyToPosInShifted, recursionCnt):
    specialChar = CheckIfSpecial(currentByte)

    if(specialChar):
        ##stuff
        StuffArray(shiftedArray,cpyToPosInShifted)

        cpyToPosInShifted+=1

        ##shift
        ShiftArray(shiftedArray,cpyToPosInShifted,currentByte)

        cpyToPosInShifted+=1
        ##increment copy from position
        if(cpyFromPosInOriginal+1 < len(originalArray)):
            currentByte = originalArray[cpyFromPosInOriginal + 1]
        else:
            currentByte = 99 ##set to a non special character
        
        cpyFromPosInOriginal+=1
        positions[0] = cpyToPosInShifted
        positions[1] = cpyFromPosInOriginal
        newRecursion = recursionCnt+1        # might cause problem
        RecursiveStuffAndShift(positions,currentByte,originalArray,shiftedArray,cpyFromPosInOriginal,cpyToPosInShifted,newRecursion)
        return positions
    else:
        return positions

def ShiftArray(stuffAndShiftedArray,cpyToPosInShifted,refByte):
    stuffAndShiftedArray[cpyToPosInShifted] = refByte ^ DPT_SpecialChar_t.ShiftXOR_e.value
    return cpyToPosInShifted + 2

def StuffArray(stuffAndShiftedArray,cpyToPosInShifted):
    stuffAndShiftedArray[cpyToPosInShifted] = DPT_SpecialChar_t.DLE_e.value
    return 0  

def CopyArrays(stuffAndShiftedArray,shiftedArrayIndex,originalArray,refIndex):
    if(refIndex < len(originalArray)):
        stuffAndShiftedArray[shiftedArrayIndex] = originalArray[refIndex]
    return 0  

def CheckIfSpecial(currentByte):
    for val in DPT_SpecialChar_t:
        if(currentByte == val.value):
            return True
    return False

##########################################  COMMANDS ############################
def DoRinse(MI, MP, DL, seqNum):
    telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, MI, MP, DL, False, [0])
    return telegram

def DoRinseLeft(seqNum):
    telegram = DoRinse(API_Command_t.DoRinse_e, LEFT_SIDE, 0,seqNum)
    return telegram
    
def DoRinseRight(seqNum):
    telegram = DoRinse(API_Command_t.DoRinse_e, RIGHT_SIDE, 0,seqNum)
    return telegram

def StartClean(seqNum):
    telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, API_Command_t.StartCleaning_e, 0, 0, False, [0] )
    return telegram
    
def GetRequest(seqNum):
    telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, API_Command_t.GetRequests_e, 0, 0, False, [0] )
    return telegram
    
def StopProcess(module,seqNum):
    telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, API_Command_t.Stop_e, module, 0x00, False, [0] )
    return telegram
    

def StopAllProcess(seqNum):
    telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, API_Command_t.Stop_e, 0, 0x00, False, [0] )
    return telegram
    
def RinseMilkOutlet(rinseMode,  side,  seqNum):
    array[0]*MILK_OUTLET_DATA_SIZE
    array[0] = rinseMode
    array[1] =  MILK_TUBE_LENGTH & 0xff
    array[2] = ((MILK_TUBE_LENGTH >> 8) & 0xff)
    telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, API_Command_t.MilkOutletRinse_e, side, MILK_OUTLET_DATA_SIZE, True, array)
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
    telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, API_Command_t.ScreenRinse_e, side, SCREEN_RINSE_DATA_SIZE, True, array)
    return telegram
    

def DoRightScreenRinse(seqNum):
    telegram = DoScreenRinse(RIGHT_SIDE,seqNum)
    return telegram
    
def DoLeftScreenRinse(seqNum):
    telegram = DoScreenRinse(LEFT_SIDE,seqNum)
    return telegram
    

def GetInfoMessage(seqNum):
    array = [0]*3
    telegram = CreateTelegram(0x00, 0x68,seqNum, 0x42, 0x41, API_Command_t.GetInfoMessages_e, 0, 3, True, array)
    return telegram
    

def DisplayAction(action, seqNum):
    telegram = CreateTelegram(0x00, 0x68,seqNum, 0x42, 0x41, API_Command_t.DisplayAction_e, action, 0, False, [0] )
    return telegram
    

def GetProductDump(side, seqNum):
    telegram = CreateTelegram(0x00, 0x68,seqNum, 0x42, 0x41, API_Command_t.GetProductDump_e, side, 0, False, [0] )
    return telegram

def GetProductDumpRight(seqNum):
    telegram = GetProductDump(RIGHT_SIDE, seqNum)
    return telegram
    
def GetProductDumpLeft(seqNum):
    telegram = GetProductDump(LEFT_SIDE,seqNum)
    return telegram
    

def GetSensorValues(seqNum):
    telegram = CreateTelegram(0x00, 0x68, seqNum, 0x42, 0x41, API_Command_t.GetSensorValues_e, 0, 0, False, [0] )
    return telegram
    
def GetStatus(seqNum):
    telegram = CreateTelegram(0x00,0x6C, seqNum, 0x42, 0x41, API_Command_t.GetStatus_e,0,0, False, [0] )
    return telegram

        

