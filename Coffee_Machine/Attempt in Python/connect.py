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
	#DoCoffee(port,seqNumber)

	while True:
		cmResponse = port.read_all()  # read from Pi
		print("Read from Pi:", cmResponse)

def InstantiatePort():
	port = serial.Serial('COM1',115200,timeout=None)
	return port
############# Execute as script ###############
if __name__ == "__main__":
	MakeCoffee()
