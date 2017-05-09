import time
import serial
from telegramFormat import telegramFormat
from commands import * 
from construct import *
from threading import Timer
from api_definitions import * 

##GLOBALS
MILK_OUTLET_DATA_SIZE = 3
MILK_TUBE_LENGTH = 350
SCREEN_RINSE_DATA_SIZE = 2
RIGHT_SIDE = 1
LEFT_SIDE = 0


## Define devices
def mainScript():
	ports = InstantiatePorts()
	## timer code here 
	telegram = CreateTelegram(0x00, 0x41, 0x41, 0x41, 0x41,0x41,0x41,0x41,False,[0])

def HelloWorld():
	print("Hello")	
	

def InstantiatePorts():
	raspberry = serial.Serial()
	raspberry.port = 'COM1'
	raspberry.timeout = None
	raspberry.baudrate = 9600
	raspberry.parity = serial.PARITY_NONE
	raspberry.stopbits = serial.STOPBITS_ONE
	raspberry.bytesize = serial.EIGHTBITS

	coffee = serial.Serial()
	coffee.port='COM2'
	coffee.timeout=None
	coffee.baudrate=9600
	coffee.parity=serial.PARITY_NONE
	coffee.stopbits=serial.STOPBITS_ONE
	coffee.bytesize=serial.EIGHTBITS
	
	if raspberry.is_open != False:
		raspberry.open()

	if coffee.is_open != False:
		raspberry.open()

	portsList = [raspberry,coffee] #0 - Raspberry
								   #1 - Coffee Machine
	return portsList


############# Execute as script ###############
if __name__ == "__main__":
	mainScript()
