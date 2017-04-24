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



print('Enter message from raspberry: ')

input_string =1

while 1 :
	# get keyboard input
	input_string = input(">> ")	
	if input_string == 'exit':
		raspberry.close()
		exit()
	else:
		# send the character to the device
		raspberry.write(bytearray(input_string,'utf8'))