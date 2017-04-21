import time
import serial
from msvcrt import getch
## Define devices

coffee = serial.Serial(
	port='COM2',
	timeout=None,
	baudrate=9600,
	parity=serial.PARITY_NONE,
	stopbits=serial.STOPBITS_ONE,
	bytesize=serial.EIGHTBITS,
)



print ("Listening from coffee machine ... ")

while 1 :
	# get keyboard input

	out = b''
	time.sleep(1)
	while coffee.in_waiting > 0:
		out += coffee.read(1)
		if coffee.in_waiting == 0:
			print(out)