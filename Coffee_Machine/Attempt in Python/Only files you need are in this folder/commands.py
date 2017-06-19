import time
#from telegramFormat import *
from api_definitions import *
import itertools
import struct
import io
import logging

logging.basicConfig(level=logging.DEBUG)
logger = logging.getLogger(__name__)

# #file handler for logs
# handler = logging.FileHandler('commands.log')
# handler.setLevel(logging.INFO)

# # create a logging format
# formatter = logging.Formatter('%(asctime)s - %(module)s - %(levelname)s - %(message)s')
# handler.setFormatter(formatter)
# logger.addHandler(handler)

#logger.disabled = True

# GLOBAL VARIABLES
MILK_OUTLET_DATA_SIZE = 3
MILK_TUBE_LENGTH = 350
SCREEN_RINSE_DATA_SIZE = 2
RIGHT_SIDE = 1
LEFT_SIDE = 0
MAX_ACK_RETRIES = 3


############################################# START OF CREATING AND READING TELEGRAMS ##########################
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
    if(dataPackets_p):
        for var in data_p:
            array.append(var)

    # call to CRC
    CRC = API_CalculateCRC(array, len(array))
    array.append(CRC & 0xff)
    array.append((CRC >> 8) & 0xff)
    ##Stuff and shift
    shifted = StuffAndShift(array)
    finalTelegram = [DPT_SpecialChar_t.SOH_e.value] + \
        shifted + [DPT_SpecialChar_t.EOT_e.value]
    
    logger.info("\nSent from Pi: " + str(finalTelegram))
    return bytes(finalTelegram)

def CreateACKPacket(PIP_p, PIE_p, PN_p, SA_p, DA_p):
    array = [
        PIP_p,
        PIE_p,
        PN_p,
        SA_p,
        DA_p,
    ]
    shifted = StuffAndShift(array)
    finalTelegram = [DPT_SpecialChar_t.SOH_e.value] + \
        shifted + [DPT_SpecialChar_t.EOT_e.value]
    #print("Telegram Created:", finalTelegram)
    return bytes(finalTelegram)

def StuffAndShift(arrayToShift):
    a = arrayToShift
    i = 0
    lengthA = len(a)
    while i < lengthA:
        for j in DPT_SpecialChar_t:
            if(a[i] == j.value):
                b = a[0:i]  # before
                c = a[i + 1:]  # after
                stuff = DPT_SpecialChar_t.DLE_e.value
                shift = a[i] ^ DPT_SpecialChar_t.ShiftXOR_e.value
                a = b + [stuff, shift] + c
                i = i + 1
                lengthA = len(a)
                break
        i += 1
    return a


def DeStuffAndShift(arrayToDeShift):
    i = 0
    while(i < len(arrayToDeShift)):
        if(arrayToDeShift[i] == DPT_SpecialChar_t.DLE_e.value):
            b = (arrayToDeShift[i + 1] ^ DPT_SpecialChar_t.ShiftXOR_e.value)
            c = arrayToDeShift[i + 2:]
            d = arrayToDeShift[0:i]
            arrayToDeShift = d + [b] + c
        i += 1
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


############################################# END OF CREATING AND READING TELEGRAMS ###########################

############################################# START OF SENDING PACKETS  #######################################


def WaitTillReady(port, seqNumber):
    ready = False
    readyCnt = 1
    while(ready == False):
        print("\n\n\n\n")
        logger.info("ATTEMPT" + str(readyCnt) + "\n\n\n")
        ready = CheckIfReady(port,seqNumber)
        seqNumber +=1 
        readyCnt +=1
def CheckIfReady(port,seqNumber):
    statuses = GetMachineStatus(port, seqNumber)[0]
    logger.info("\nReading machine status...")
    logger.debug(str(statuses))
    # print (statuses[1]['Just Reset']) # How to Access Machine status bits
    # print (statuses[0]['Coffee Left Process']) # How to Access General status bits 
    del statuses['Machine Status']
    statusCodes = list(val for key,val in statuses.items() if 'status' in key.lower())
    if all([ v == 0 for v in statusCodes ]) :
        logger.info("\Success")
        logger.info ("\n The coffee machine is ready")
        return True
    else:
        logger.error("\nSomething is wrong, one or more statuses indicate they are not ready")
        input("\n Please check the coffee machine for errors and press any key to try again..")
        return False

def GetMachineStatus(port, seqNumber):
    # Get the request telegram
    logger.info("\n\n Sending a request to get the machine status...")
    telegram = GetStatusTelegram(seqNumber)

    # Get an ACK packet so the coffee machine is ready to send a response
    InitRequest(port,seqNumber,telegram)
    logger.info("\nResending the request to receive response...")
    # Change the sequence number for the new packet
    seqNumber += 1
    telegram = GetStatusTelegram(seqNumber)
    # Get the response data 
    responseData = DoRequest(port,seqNumber,telegram)  
    return GetStatusArrays(responseData)

def InitRequest(port,seqNumber,telegram):
    WaitTillACK(port, seqNumber, telegram)
    ClearPortContents(port)

def WaitTillACK(port, seqNumber, telegram):
    ACKReceived = False
    retryCnt = 0
    while(ACKReceived == False and retryCnt <= MAX_ACK_RETRIES):
        ACKReceived = CheckForAck(port, seqNumber, telegram)
        if(ACKReceived):
            break
        time.sleep(0.8) # might be problem
        seqNumber+=1
        retryCnt +=1 
def CheckForAck(port, seqNumber, telegram):
    # Send Command/Request
    port.write(bytes(telegram))  # send command to coffee machine
    time.sleep(0.1)
    # Read response
    cmResponse = port.read_all()  # read from Pi

    ##Convert response to telegram array
    telegram = ResponseToTelegramArray(cmResponse)
    logger.info("\n Received from coffee machine: " + str(telegram))
    if(len(telegram) > 0 ):
        if(telegram[2] == PacketTypes.ACK.value):
            logger.info("\nCoffee machine sends ACK to command/request.. message received")
            return True
    else:
        logger.info("\n Received no ACK packet from coffee machine...sending packet again")
        return False

# Check if machine is ready before sending any commands/requests
def DoCommand(port,seqNumber,telegram):
    WaitTillACK(port,seqNumber,telegram)
    ClearPortContents(port)
    port.write(bytes(telegram))
    return 0

def DoRequest(port,seqNumber, telegram):
    
    machineStatusReceived = False
    responseData = 0
    while machineStatusReceived == False:
        port.write(bytes(telegram))
        time.sleep(0.15)
        response = port.read_all()
        responseData = ResponseToTelegramArray(response)[7:]
        if(len(responseData) > 0):
            # decode the message to see if it is indeed a response
            if (responseData[2] == PacketTypes.RESPONSE.value):
                machineStatusReceived = True
                logger.info("\nResponse successfully received:")
                logger.info("\n Response received from coffee machine: " + str(responseData))
        else:
            return 0
            # Do something
    return responseData



def ClearPortContents(port):
    port.reset_input_buffer()
    port.reset_output_buffer()




    


def GetStatusArrays(telegram):
    startOfDataBits = 11 # SOH = 0, PIP = 1, PIE = 2 ... DL = (9+10) - 16 bit
    dataBits = {'Machine Status' : telegram[startOfDataBits], \
    'Coffee Left Action': HighNibble(telegram[startOfDataBits + 1]), \
    'Coffee Left Status': LowNibble(telegram[startOfDataBits + 1]), \

    'Coffee Right Action':  HighNibble(telegram[startOfDataBits + 2]), \
    'Coffee Right Status':  LowNibble(telegram[startOfDataBits + 2]), \

    'Steam Left Action':  HighNibble(telegram[startOfDataBits + 3]), \
    'Steam Left Status':  LowNibble(telegram[startOfDataBits + 3]), \

    'Steam Right Action': HighNibble(telegram[startOfDataBits + 4]), \
    'Steam Right Status': LowNibble(telegram[startOfDataBits + 4]), \

    'Water Action':  HighNibble(telegram[startOfDataBits + 5]), \
    'Water Status':  LowNibble(telegram[startOfDataBits + 5]), \

    'Coffee Left Process':  telegram[startOfDataBits + 6], \
    'Coffee Right Process':  telegram[startOfDataBits + 7], \
    'Steam Left Process':  telegram[startOfDataBits + 8], \
    'Steam Right Process':  telegram[startOfDataBits + 9], \
    'Water Process':  telegram[startOfDataBits + 10], \
    }

    ## Might need to flip if big-endian architecture
    machineStatus = ByteToBitArray(dataBits['Machine Status'])
    machineStatusData = {
    'Just Reset': machineStatus[0], \
    'Request Set': machineStatus[1], \
    'Info Message Set': machineStatus[2], \
    'Product Dump Left':  machineStatus[4], \
    'Product Dump Right':  machineStatus[5], \
    }
    return [dataBits, machineStatusData]







def ResponseToTelegramArray(response):
    telegram = TelegramToIntArray(response)
    telegram = DeStuffAndShift(telegram)
    return telegram

def TelegramToIntArray(telegram):
    array = []
    a = struct.unpack('B' * len(telegram), telegram)
    array.append(a)
    return list(itertools.chain.from_iterable(array))


def readtilleol(port):
    eol = b'\x04'
    leneol = len(eol)
    line = bytearray()
    while True:
        c = port.read(1)
        if c:
            line += c
            if line[-leneol:] == eol:
                break
        else:
            break
    return bytes(line)


############################################# END OF SENDING PACKETS  #########################################


############################################# START OF PRODUCTS  ##############################################
def DoProduct(side,dataDict,seqNum):

    data = [dataDict["Product Type: "],dataDict["Product Process: "],dataDict["Water Quantity: "] & 0xff, \
            ((dataDict["Water Quantity: "] >> 8) & 0xff),dataDict["Bean Hopper: "], \
             dataDict["Cake Thickness: "] & 0xff, ((dataDict["Cake Thickness: "] >> 8) & 0xff), \
             dataDict["Tamping: "], dataDict["Pre-Infusion: "],dataDict["Relax Time: "], dataDict["Second Tamping: "], \
             dataDict["Milk Qty: "] & 0xff, ((dataDict["Milk Qty: "] >> 8) & 0xff), \
             dataDict["Milk Temperature: "], dataDict["Milk Percent: "], dataDict["Milk Seq: "], dataDict["Latte Macchiato Time: "], \
             dataDict["Foam Sequence: "], \
             dataDict["Steam Time: "] & 0xff, ((dataDict["Steam Time: "] >> 8) & 0xff), \
             dataDict["Steam Temperature: "], dataDict["Everfoam Mode: "], dataDict["Air Stop Temperature: "], \
             dataDict["Air Stop Time: "] & 0xff, ((dataDict["Air Stop Time: "] >> 8) & 0xff), \
             dataDict["Pump Speed Milk: "] & 0xff, ((dataDict["Pump Speed Milk: "] >> 8) & 0xff), \
             dataDict["Pump Speed Foam: "] & 0xff, ((dataDict["Pump Speed Foam: "] >> 8) & 0xff), \
             dataDict["param 23: "], \
             dataDict["Milk/Coffee Delay: "] & 0xff, ((dataDict["Milk/Coffee Delay: "] >> 8) & 0xff)]
    
    #print(len(data))
    
    telegram =  CreateTelegram(0x00,PacketTypes.COMMAND.value,seqNum,0x42,0x41,0x02,side,len(data),True,data)
    #print([hex(x) for x in TelegramToIntArray(telegram)])
    #print(len(TelegramToIntArray(telegram)))
    
    return telegram

# Product 1 Coffee
def DoCoffee(side,seqNum):
    dataDict = {"Product Type: ":ProductType_t.Coffee_e.value, "Product Process: ":0, \
            "Water Quantity: ":135 , "Bean Hopper: ":1, "Cake Thickness: ":140, \
            "Tamping: ":64, "Pre-Infusion: ": 0, "Relax Time: ": 0, "Second Tamping: ":0, \
            "Milk Qty: ":10, "Milk Temperature: ":255,"Milk Percent: ":1,"Milk Seq: ":MilkSequence_t.MilkSeqUndef_e.value,\
            "Latte Macchiato Time: ":1, "Foam Sequence: ":0, "Steam Time: ":1,\
            "Steam Temperature: ":30, "Everfoam Mode: ":0, "Air Stop Temperature: ":0, "Air Stop Time: ":10, \
            "Pump Speed Milk: ":1500, "Pump Speed Foam: ":3000, "param 23: ":0, "Milk/Coffee Delay: ":5}
    telegram = DoProduct(side,dataDict,seqNum)
    return telegram



def DoCoffeeLeftTelegram(seqNum):
    telegram = DoCoffee(LEFT_SIDE,seqNum)
    return telegram

def DoCoffeeRightTelegram(seqNum):
    telegram = DoCoffee(RIGHT_SIDE,seqNum)
    return telegram


# Product 2 Espresso
def DoEspresso(side,seqNum):
    
    dataDict = {"Product Type: ":ProductType_t.Espresso_e.value, "Product Process: ":0, \
            "Water Quantity: ":50 , "Bean Hopper: ":0, "Cake Thickness: ":140, \
            "Tamping: ":64, "Pre-Infusion: ": 8, "Relax Time: ": 20, "Second Tamping: ":20, \
            "Milk Qty: ":10, "Milk Temperature: ":255,"Milk Percent: ":1,"Milk Seq: ":MilkSequence_t.MilkSeqUndef_e.value,\
            "Latte Macchiato Time: ":1, "Foam Sequence: ":0, "Steam Time: ":1,\
            "Steam Temperature: ":30, "Everfoam Mode: ":0, "Air Stop Temperature: ":0, "Air Stop Time: ":10, \
            "Pump Speed Milk: ":1500, "Pump Speed Foam: ":3000, "param 23: ":0, "Milk/Coffee Delay: ":5}
    telegram = DoProduct(side,dataDict,seqNum)
    return telegram

def DoEspressoLeftTelegram(seqNum):
    telegram = DoEspresso(LEFT_SIDE,seqNum)
    return telegram

def DoEspressoRightTelegram(seqNum):
    telegram = DoEspresso(RIGHT_SIDE,seqNum)
    return telegram

# Product 3 Hot water

def DoHotWater(side,seqNum):
    
    dataDict = {"Product Type: ":ProductType_t.HotWater_e.value, "Product Process: ":2, \
            "Water Quantity: ":100 , "Bean Hopper: ":0, "Cake Thickness: ":140, \
            "Tamping: ":64, "Pre-Infusion: ": 8, "Relax Time: ": 20, "Second Tamping: ":20, \
            "Milk Qty: ":10, "Milk Temperature: ":255,"Milk Percent: ":1,"Milk Seq: ":MilkSequence_t.MilkSeqUndef_e.value,\
            "Latte Macchiato Time: ":1, "Foam Sequence: ":0, "Steam Time: ":1,\
            "Steam Temperature: ":30, "Everfoam Mode: ":0, "Air Stop Temperature: ":0, "Air Stop Time: ":10, \
            "Pump Speed Milk: ":1500, "Pump Speed Foam: ":3000, "param 23: ":0, "Milk/Coffee Delay: ":5}
    telegram = DoProduct(side,dataDict,seqNum)
    return telegram

def DoHotWaterLeftTelegram(seqNum):
    telegram = DoHotWater(LEFT_SIDE,seqNum)
    return telegram

def DoHotWaterRightTelegram(seqNum):
    telegram = DoHotWater(RIGHT_SIDE,seqNum)
    return telegram


############################################# END OF PRODUCTS  ################################################

####################################### START OF TELEGRAMS ####################################################

def DoRinseTelegram(MI, MP, DL, seqNum):
    telegram = CreateTelegram(0x00, PacketTypes.COMMAND.value, seqNum, 0x42,
                              0x41, MI, MP, DL, False, [0])
    return telegram


def DoRinseLeftTelegram(seqNum):
    telegram = DoRinseTelegram(API_Command_t.DoRinse_e.value, LEFT_SIDE, 0, seqNum)
    return telegram


def DoRinseRightTelegram(seqNum):
    telegram = DoRinseTelegram(API_Command_t.DoRinse_e.value, RIGHT_SIDE, 0, seqNum)
    return telegram


def StartCleanTelegram(seqNum):
    telegram = CreateTelegram(0x00, PacketTypes.COMMAND.value, seqNum, 0x42, 0x41,
                              API_Command_t.StartCleaning_e.value, 0, 0, False, [0])
    return telegram

def StopProcessTelegram(module, seqNum):
    telegram = CreateTelegram(0x00, PacketTypes.COMMAND.value, seqNum, 0x42, 0x41,
                              API_Command_t.Stop_e.value, module, 0x00, False, [0])
    return telegram


def StopAllProcessTelegram(seqNum):
    telegram = CreateTelegram(
        0x00, PacketTypes.COMMAND.value, seqNum, 0x42, 0x41, API_Command_t.Stop_e.value, 0, 0x00, False, [0])
    return telegram


def RinseMilkOutletTelegram(rinseMode,  side,  seqNum):
    array[0] * MILK_OUTLET_DATA_SIZE
    array[0] = rinseMode
    array[1] = MILK_TUBE_LENGTH & 0xff
    array[2] = ((MILK_TUBE_LENGTH >> 8) & 0xff)
    telegram = CreateTelegram(0x00, PacketTypes.COMMAND.value, seqNum, 0x42, 0x41,
                              API_Command_t.MilkOutletRinse_e.value, side, MILK_OUTLET_DATA_SIZE, True, array)
    return telegram


def RinseRightMilkOutletTelegram(seqNum):
    telegram = RinseMilkOutletTelegram(1, RIGHT_SIDE, seqNum)
    return telegram


def RinseLeftMilkOutletTelegram(seqNum):
    telegram = RinseMilkOutletTelegram(1, LEFT_SIDE, seqNum)
    return telegram


def RinseRightTubesTelegram(seqNum):
    telegram = RinseMilkOutletTelegram(2, RIGHT_SIDE, seqNum)
    return telegram


def RinseLeftTubesTelegram(seqNum):
    telegram = RinseMilkOutletTelegram(2, LEFT_SIDE, seqNum)
    return telegram


def RinseRightTubesAndOutletTelegram(seqNum):
    telegram = RinseMilkOutletTelegram(0, RIGHT_SIDE, seqNum)
    return telegram


def RinseLeftTubesAndOutletTelegram(seqNum):
    telegram = RinseMilkOutletTelegram(0, LEFT_SIDE, seqNum)
    return telegram


def DoScreenRinseTelegram(side, seqNum):
    array[0] * SCREEN_RINSE_DATA_SIZE
    array[0] = 3  # screen rinse cycles
    array[1] = 10  # repetitions
    telegram = CreateTelegram(0x00, PacketTypes.COMMAND.value, seqNum, 0x42, 0x41,
                              API_Command_t.ScreenRinse_e.value, side, SCREEN_RINSE_DATA_SIZE, True, array)
    return telegram


def DoRightScreenRinseTelegram(seqNum):
    telegram = DoScreenRinseTelegram(RIGHT_SIDE, seqNum)
    return telegram


def DoLeftScreenRinseTelegram(seqNum):
    telegram = DoScreenRinseTelegram(LEFT_SIDE, seqNum)
    return telegram
def GetStatusTelegram(seqNum):
    telegram = CreateTelegram(0x00, PacketTypes.REQUEST.value, seqNum, 0x42, 0x41,
                              API_Command_t.GetStatus_e.value, 0, 0, False, [0])
    return telegram

####################################### END OF TELEGRAMS ######################################################

####################################### START OF REQUESTS #####################################################
def GetRequest(seqNum,port):
    telegram = CreateTelegram(0x00, PacketTypes.REQUEST.value, seqNum, 0x42, 0x41,
                              API_Command_t.GetRequests_e.value, 0, 0, False, [0])

    # Get the request telegram
    logger.info("\n\n Sending a request to get the set requests...")

    # Get an ACK packet so the coffee machine is ready to send a response
    InitRequest(port,seqNum,telegram)
    logger.info("\nResending the request to receive response...")
    # Change the sequence number for the new packet
    seqNum += 1
    telegram = CreateTelegram(0x00, PacketTypes.REQUEST.value, seqNum, 0x42, 0x41,
                              API_Command_t.GetRequests_e.value, 0, 0, False, [0])
    # Get the response data 
    responseData = DoRequest(port,seqNum,telegram)  
    
    return responseData
    #print([hex(x) for x in responseData])

def GetInfoMessage(seqNum,port):
    array = [0] * 3
    telegram = CreateTelegram(0x00, PacketTypes.REQUEST.value, seqNum, 0x42, 0x41,
                              API_Command_t.GetInfoMessages_e.value, 0, 3, True, array)

                              
    # Get the request telegram
    logger.info("\n\n Sending a request to get the info messages...")

    # Get an ACK packet so the coffee machine is ready to send a response
    InitRequest(port,seqNum,telegram)
    logger.info("\nResending the request to receive response...")
    # Change the sequence number for the new packet
    seqNum += 1
    telegram = CreateTelegram(0x00, PacketTypes.REQUEST.value, seqNum, 0x42, 0x41,
                              API_Command_t.GetInfoMessages_e.value, 0, 3, True, array)
    # Get the response data 
    responseData = DoRequest(port,seqNum,telegram)  
    return responseData


def DisplayAction(action, seqNum):
    telegram = CreateTelegram(0x00, PacketTypes.REQUEST.value, seqNum, 0x42, 0x41,
                              API_Command_t.DisplayAction_e.value, action, 0, False, [0])
    return telegram


def GetProductDump(side, seqNum,port):
    telegram = CreateTelegram(0x00, PacketTypes.REQUEST.value, seqNum, 0x42, 0x41,
                              API_Command_t.GetProductDump_e.value, side, 0, False, [0])

    # Get the request telegram
    logger.info("\n\n Sending a request to get the info messages...")

    # Get an ACK packet so the coffee machine is ready to send a response
    InitRequest(port,seqNum,telegram)
    logger.info("\nResending the request to receive response...")
    # Change the sequence number for the new packet
    seqNum += 1
    telegram = CreateTelegram(0x00, PacketTypes.REQUEST.value, seqNum, 0x42, 0x41,
                              API_Command_t.GetProductDump_e.value, side, 0, False, [0])
    # Get the response data 
    responseData = DoRequest(port,seqNum,telegram)  
    return responseData

def GetProductDumpRight(seqNum,port):
    response = GetProductDump(RIGHT_SIDE, seqNum,port)
    return response


def GetProductDumpLeft(seqNum,port):
    response = GetProductDump(LEFT_SIDE, seqNum,port)
    return response


def GetSensorValues(seqNum,port):
    telegram = CreateTelegram(0x00, PacketTypes.REQUEST.value, seqNum, 0x42, 0x41,
                              API_Command_t.GetSensorValues_e.value, 0, 0, False, [0])
    # Get the request telegram
    logger.info("\n\n Sending a request to get the sensor values...")

    # Get an ACK packet so the coffee machine is ready to send a response
    InitRequest(port,seqNum,telegram)
    logger.info("\nResending the request to receive response...")
    # Change the sequence number for the new packet
    seqNum += 1
    telegram = CreateTelegram(0x00, PacketTypes.REQUEST.value, seqNum, 0x42, 0x41,
                              API_Command_t.GetSensorValues_e.value, 0, 0, False, [0])
    # Get the response data 
    responseData = DoRequest(port,seqNum,telegram)  
    return responseData


####################################### ENF OF REQUESTS ######################################################

####################################### START OF CRC CALC ####################################################
crcPolynomTable = \
    [
        0x0000, 0xc0c1, 0xc181, 0x0140, 0xc301, 0x03c0, 0x0280, 0xc241,
        0xc601, 0x06c0, 0x0780, 0xc741, 0x0500, 0xc5c1, 0xc481, 0x0440,
        0xcc01, 0x0cc0, 0x0d80, 0xcd41, 0x0f00, 0xcfc1, 0xce81, 0x0e40,
        0x0a00, 0xcac1, 0xcb81, 0x0b40, 0xc901, 0x09c0, 0x0880, 0xc841,
        0xd801, 0x18c0, 0x1980, 0xd941, 0x1b00, 0xdbc1, 0xda81, 0x1a40,
        0x1e00, 0xdec1, 0xdf81, 0x1f40, 0xdd01, 0x1dc0, 0x1c80, 0xdc41,
        0x1400, 0xd4c1, 0xd581, 0x1540, 0xd701, 0x17c0, 0x1680, 0xd641,
        0xd201, 0x12c0, 0x1380, 0xd341, 0x1100, 0xd1c1, 0xd081, 0x1040,
        0xf001, 0x30c0, 0x3180, 0xf141, 0x3300, 0xf3c1, 0xf281, 0x3240,
        0x3600, 0xf6c1, 0xf781, 0x3740, 0xf501, 0x35c0, 0x3480, 0xf441,
        0x3c00, 0xfcc1, 0xfd81, 0x3d40, 0xff01, 0x3fc0, 0x3e80, 0xfe41,
        0xfa01, 0x3ac0, 0x3b80, 0xfb41, 0x3900, 0xf9c1, 0xf881, 0x3840,
        0x2800, 0xe8c1, 0xe981, 0x2940, 0xeb01, 0x2bc0, 0x2a80, 0xea41,
        0xee01, 0x2ec0, 0x2f80, 0xef41, 0x2d00, 0xedc1, 0xec81, 0x2c40,
        0xe401, 0x24c0, 0x2580, 0xe541, 0x2700, 0xe7c1, 0xe681, 0x2640,
        0x2200, 0xe2c1, 0xe381, 0x2340, 0xe101, 0x21c0, 0x2080, 0xe041,
        0xa001, 0x60c0, 0x6180, 0xa141, 0x6300, 0xa3c1, 0xa281, 0x6240,
        0x6600, 0xa6c1, 0xa781, 0x6740, 0xa501, 0x65c0, 0x6480, 0xa441,
        0x6c00, 0xacc1, 0xad81, 0x6d40, 0xaf01, 0x6fc0, 0x6e80, 0xae41,
        0xaa01, 0x6ac0, 0x6b80, 0xab41, 0x6900, 0xa9c1, 0xa881, 0x6840,
        0x7800, 0xb8c1, 0xb981, 0x7940, 0xbb01, 0x7bc0, 0x7a80, 0xba41,
        0xbe01, 0x7ec0, 0x7f80, 0xbf41, 0x7d00, 0xbdc1, 0xbc81, 0x7c40,
        0xb401, 0x74c0, 0x7580, 0xb541, 0x7700, 0xb7c1, 0xb681, 0x7640,
        0x7200, 0xb2c1, 0xb381, 0x7340, 0xb101, 0x71c0, 0x7080, 0xb041,
        0x5000, 0x90c1, 0x9181, 0x5140, 0x9301, 0x53c0, 0x5280, 0x9241,
        0x9601, 0x56c0, 0x5780, 0x9741, 0x5500, 0x95c1, 0x9481, 0x5440,
        0x9c01, 0x5cc0, 0x5d80, 0x9d41, 0x5f00, 0x9fc1, 0x9e81, 0x5e40,
        0x5a00, 0x9ac1, 0x9b81, 0x5b40, 0x9901, 0x59c0, 0x5880, 0x9841,
        0x8801, 0x48c0, 0x4980, 0x8941, 0x4b00, 0x8bc1, 0x8a81, 0x4a40,
        0x4e00, 0x8ec1, 0x8f81, 0x4f40, 0x8d01, 0x4dc0, 0x4c80, 0x8c41,
        0x4400, 0x84c1, 0x8581, 0x4540, 0x8701, 0x47c0, 0x4680, 0x8641,
        0x8201, 0x42c0, 0x4380, 0x8341, 0x4100, 0x81c1, 0x8081, 0x4040
    ]


def API_CalculateCRC(data_p, dataLength):
    nIndex = 0
    checkSum = 0
    INIT_CRC = 0xFFFF
    checkSum = INIT_CRC

    while(nIndex < dataLength):
        checkSum = ((checkSum >> 8) ^ crcPolynomTable[(
            checkSum ^ data_p[nIndex]) & 0xFF])
        nIndex += 1

    return checkSum
####################################### END OF CRC CALC ######################################################