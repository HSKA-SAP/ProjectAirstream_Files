import time
from commands import *

import serial
from api_definitions import *
from telegramFormat import telegramFormat


# import GPIO
# from construct import *
# from threading import Timer
# import array
# import struct 

## Important ##
# - Make sure format of RPi is little endian - GOOD
# - When ran on Pi -  use serial.Serial('/dev/ttyS0',115200, timeout=None)

## Define devices
def MakeCoffee():
	port = InstantiatePort()
	seqNumber = 0
	# Check for ACK, Get Status, Check if all statuses are okay
	WaitTillReady(port,seqNumber,False)
    # telegram = DoRinseRight(seqNumber)
    # DoCommand(port,seqNumber,telegram)
	telegram = DoCoffeeLeft(seqNumber)
	DoCommand(port,seqNumber,telegram)

def InstantiatePort():
	port = serial.Serial('COM6',115200,timeout=None)
	return port
############# Execute as script ###############
if __name__ == "__main__":
	MakeCoffee()
