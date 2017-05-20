import time
import serial
from telegramFormat import telegramFormat
from commands import * 
from api_definitions import * 
# import GPIO
# from construct import *
# from threading import Timer
# import array
# import struct 

## Important ##
# - Make sure format of RPi is little endian - GOOD


## Define devices
def MakeCoffee():
	ports = InstantiatePorts()
	seqNumber = 0
	WaitTillReady(ports,seqNumber,False)


def InstantiatePorts():
	raspberry = serial.Serial('COM1',115200,timeout=None)
	coffee = serial.Serial('COM2',115200,timeout=None)
	portsList = {'pi':raspberry,'cm':coffee} 
	return portsList
############# Execute as script ###############
if __name__ == "__main__":
	MakeCoffee()
