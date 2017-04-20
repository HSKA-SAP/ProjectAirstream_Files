import time
import serial


## Define devices


raspberry = serial.Serial(
	port='COM1',
	baudrate=9600,
	parity=serial.PARITY_NONE,
	stopbits=serial.STOPBITS_ONE,
	bytesize=serial.EIGHTBITS
)



coffee = serial.Serial(
	port='COM2',
	baudrate=9600,
	parity=serial.PARITY_NONE,
	stopbits=serial.STOPBITS_ONE,
	bytesize=serial.EIGHTBITS
)


print ("Listening from coffee machine ... ")

while 1 :
	# get keyboard input
	input_string = input(">> ")	
	if input_string == 'exit':
		coffee.close()
		exit()
	else:
		out = ''
		while coffee.in_waiting > 0:
			out += coffee.read(10)
		print(">>" + out)