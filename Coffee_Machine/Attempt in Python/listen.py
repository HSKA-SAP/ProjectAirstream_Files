import time
import serial
from telegramFormat import telegramFormat
from commands import * 
from api_definitions import * 

## Define devices
## On the receiving end, the CRC checks from PIP to CRC 
##  i.e. SOH -->|PIP, PIE, PN, SA,DA, MI, MP, DL, DATA,CRC| <--EOT 

def Listen():
    port = InstantiatePorts()
    seqNum = 0
    WaitForPacket(port, seqNum)

def InstantiatePorts():
    seqNum = 0
    port = serial.Serial('/dev/ttyS0',115200, timeout=None)
    return port

def WaitForPacket(port,seqNum):
    received = False
    while (received == False):
        input_buff = readtilleol(port)
        port.reset_input_buffer()
        input_telegram = ResponseToTelegramArray(input_buff)
        print('Telegram read from input: ',input_telegram)
        if(input_buff[-1] == 4):
            crcValid = CheckForCRC(input_telegram)
            print(crcValid)
            if(input_telegram[2] == PacketTypes.COMMAND.value and (crcValid ==0)):
                SendACKPacket(port)
            elif(input_telegram[2] == PacketTypes.REQUEST.value and (crcValid ==0)):
                SendACKPacket(port)
                ProcessRequest(input_telegram, port)

def CheckForCRC(telegram):
    dataToCheck = bytes(telegram[1:-1])
    length = len(dataToCheck)
    return API_CalculateCRC(dataToCheck,length)

def SendACKPacket(port):
    ACK = CreateACK(0x00,0x6A,0x02,0x41,0x42)
    port.write(ACK)

def ProcessRequest(telegram, port):
    messageID = 6 #7th byte in telegram
    if(telegram[6] == API_Command_t.GetStatus_e.value):
        ##code for processing status 
        print("a")
        telegram = CreateTelegram(0x00,0x68,0x02,0x41,0x42,0x01,0x00,0x0006,True,[0,0,0,0,0,0])
        port.write(telegram)
    elif(telegram[6] == API_Command_t.GetRequests_e.value):
        #reponse
        print("b")
    elif(telegram[6] == API_Command_t.GetInfoMessages_e.value):
        #reponse
        print("c")
    elif(telegram[6] == API_Command_t.DisplayAction_e.value):
        #reponse
        print("d")
    elif(telegram[6] == API_Command_t.GetProductDump_e.value):
        #reponse
        print("e")
    elif(telegram[6] == API_Command_t.GetSensorValues_e.value):
        #reponse
        print("f")
    else:
        return 0

############# Execute as script ###############
if __name__ == "__main__":
    Listen()