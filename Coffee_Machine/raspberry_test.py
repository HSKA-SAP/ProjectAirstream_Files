import time
import serial


## Define devices


raspberry = serial.Serial()
raspberry.port = 'COM1'
raspberry.timeout = None
raspberry.baudrate = 9600
raspberry.parity = serial.PARITY_NONE
raspberry.stopbits = serial.STOPBITS_ONE
raspberry.bytesize = serial.EIGHTBITS

if raspberry.is_open != False:
	raspberry.open()

coffee = serial.Serial()
coffee.port='COM2'
coffee.timeout=None
coffee.baudrate=9600
coffee.parity=serial.PARITY_NONE
coffee.stopbits=serial.STOPBITS_ONE
coffee.bytesize=serial.EIGHTBITS




print('Enter message from raspberry: ')

input_string =1

while 1 :
	# get keyboard input
	input_string = input(">> ")	
	if input_string == 'exit':
		coffee.close()
		exit()
	else:
		# send the character to the device
		coffee.write(bytearray(input_string,'utf8'))