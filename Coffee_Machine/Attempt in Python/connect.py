import commands
import serial

## Important ##
# - Make sure format of RPi is little endian 
# - When run on Pi -  use serial.Serial('/dev/ttyS0',115200, timeout=None)

def MakeCoffee():
	while True:
		try:
			port = InstantiatePort()
			break
		except serial.serialutil.SerialException:
			input("Serial port is already in use.. please close the port and press <Enter> to try again")
	seqNumber = 0
	# Check for ACK, Get Status, Check if all statuses are okay
	print("[STATUS] Start script")
	print("\n[STATUS]Checking coffee machine status...")
	commands.WaitTillReady(port,seqNumber)
	# telegram = DoRinseRight(seqNumber)
	# DoCommand(port,seqNumber,telegram)#
	telegram = commands.DoCoffeeLeft(seqNumber)
	commands.DoCommand(port,seqNumber,telegram)
	close(port)

def close(port):
	port.close()
	input("script executed successfully, press any key to exit...")

def InstantiatePort():
	port = serial.Serial('COM6',115200,timeout=None)
	return port
############# Execute as script ###############
if __name__ == "__main__":
	MakeCoffee()
