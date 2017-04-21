import time
import serial


## Define devices


raspberry = serial.Serial(
	port='COM1',
	timeout=None,
	baudrate=9600,
	parity=serial.PARITY_NONE,
	stopbits=serial.STOPBITS_ONE,
	bytesize=serial.EIGHTBITS
)



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