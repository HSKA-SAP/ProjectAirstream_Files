import commands
import serial
import logging
logging.basicConfig(level=logging.DEBUG)
logger = logging.getLogger(__name__)

#file handler for logs
handler = logging.FileHandler('connect.log')
handler.setLevel(logging.INFO)

# create a logging format
formatter = logging.Formatter('%(asctime)s - %(module)s - %(levelname)s - %(message)s')
handler.setFormatter(formatter)
logger.addHandler(handler)


## Important ##
# - Make sure format of RPi is little endian 
# - When run on Pi -  use serial.Serial('/dev/ttyS0',115200, timeout=None)

def MakeCoffee(moreThanOneProduct):
	logger.disabled = True
	port = OpenThePort()
	seqNumber = 0
	# Check for ACK, Get Status, Check if all statuses are okay
	logger.info("Start script")
	logger.info("Checking coffee machine status...")
	commands.WaitTillReady(port,seqNumber)
	# telegram = DoRinseRight(seqNumber)
	# DoCommand(port,seqNumber,telegram)#
	#print(commands.GetProductDumpLeft(seqNumber,port))
	logger.info("Making a coffee ...")
	if(moreThanOneProduct): telegram = commands.DoCoffeeLeftTelegram(seqNumber)
	else: telegram = commands.DoCoffeeRightTelegram(seqNumber)
	commands.DoCommand(port,seqNumber,telegram)
	CloseThePort(port)


def MakeEspresso(moreThanOneProduct):
	logger.disabled = True
	port = OpenThePort()
	seqNumber = 0
	# Check for ACK, Get Status, Check if all statuses are okay
	logger.info("Start script")
	logger.info("Checking coffee machine status...")
	commands.WaitTillReady(port,seqNumber)
	# telegram = DoRinseRight(seqNumber)
	# DoCommand(port,seqNumber,telegram)#
	#print(commands.GetProductDumpLeft(seqNumber,port))
	logger.info("Making an espresso ...")
	if(moreThanOneProduct):telegram = commands.DoEspressoRightTelegram(seqNumber)
	else: telegram = commands.DoEspressoLeftTelegram(seqNumber)
	commands.DoCommand(port,seqNumber,telegram)
	CloseThePort(port)

def OpenThePort():
	while True:
		try:
			port = InstantiatePort()
			break
		except serial.serialutil.SerialException as e:
			if("FileNotFoundError" in str(e)):
				logger.error("No serial connected detected")
				input("Serial port is not connected to anything.. please connect the port and press any key to try again...")
			elif("PermissionError" in str(e)):
				logger.error("Serial port is being used by another application")
				input("Serial port is being used by another application..please close the application and press any key to try again..")
	return port
def CloseThePort(port):
	port.close()
	input("script executed successfully, press any key to exit...")

def InstantiatePort():
	port = serial.Serial('COM6',115200,timeout=None)
	return port
############# Execute as script ###############
if __name__ == "__main__":
	MakeCoffee()
