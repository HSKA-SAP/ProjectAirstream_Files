import time
import serial
from construct import *

## CRC Format

## Telegram format


##### Example code ##################################################################


# >>> raw = message_format.build(Container(
# ...         msg_id=0x1234,
# ...         dest_addr=0xacba,
# ...         command_type='RESTART',
# ...         flags=Container(on=1, cache=0, status=4),
# ...         datalen=4,
# ...         data=[0x1, 0xff, 0xff, 0xdd],
# ...         crc=0x12345678))
# >>> raw.encode('hex')
##################################################################################





## Construct message format

telegramFormat = Struct('telegramFormat',
	'PIP'/Int8ub,
	'PIP'/Int8ub,
	'PN'/Int8ub,
	'SA'/Int8ub,
	'DA'/Int8ub,
	'commandType'/Int8ub,
	'messageParam'/Int16ub,
	'dataLen'/Int16ub,
)

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
# print(telegramFormat)
input_string =1

while 1 :
	# get keyboard input
	inputString = input(">> ")	
	if inputString == 'exit':
		raspberry.close()
		exit()
	else:
		# send the character to the device
		raspberry.write(bytearray(inputString,'utf8'))


# ushort API_CalculateCRC( ref byte[] data_p, int dataLength )
# {
# int nIndex;
# ushort checkSum;
# const ushort INIT_CRC = 0xFFFF;
# checkSum = INIT_CRC;
# for( nIndex = 0; nIndex < dataLength; nIndex++ )
# {
# checkSum = ( ushort )( ( checkSum >> 8 ) ^ crcPolynomTable[( checkSum ^ data_p[nIndex] ) & 0xFF] );
# }
# return checkSum;
# }

def calculateCRC():


