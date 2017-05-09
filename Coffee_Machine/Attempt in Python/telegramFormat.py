from construct import *
## Construct message format


telegramFormat = Struct(
	'PIP'/Int8ub,
	'PIE'/Int8ub,
	'PN'/Int8ub,
	'SA'/Int8ub,
	'DA'/Int8ub,
	'commandType'/Int8ub,
	'messageParam'/Int16ub,
	'dataLen'/Int16ub,
    'data'/Array(1,Byte)
)

class telegramStruct:
    PIP = 0
    PIE = 0
    PN = 0
    SA = 0
    DA = 0
    commandType = 0
    messageParam = 0
    dataLen = 0
    data = 0
