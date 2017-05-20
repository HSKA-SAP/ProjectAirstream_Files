    # telegram = telegramFormat
    # builtTelegram = telegram.build(
    #     dict(
    #         PIP = PIP_p,
    #         PIE = 0x68,
    #         PN = 3,
    #         SA = 4,
    #         DA = 5,
    #         commandType = 6,
    #         messageParam = MP_p,
    #         dataLen = DL_p,
    #         data = data_p
    #     )
    # )


#     import time
# import serial
# ## Define devices

# coffee = serial.Serial(
# 	port='COM2',
# 	timeout=None,
# 	baudrate=9600,
# 	parity=serial.PARITY_NONE,
# 	stopbits=serial.STOPBITS_ONE,
# 	bytesize=serial.EIGHTBITS,
# )



# print ("Listening from coffee machine ... ")

# while 1 :
# 	# get keyboard input

# 	out = b''
# 	time.sleep(1)
# 	while coffee.in_waiting > 0:
# 		out += coffee.read(1)
# 		if coffee.in_waiting == 0:
# 			print(out)

# ###########################################################
# #					   RESPONSES					      #	
# ###########################################################
# 			if out == b"Make some coffee":
# 				print("Making coffee")