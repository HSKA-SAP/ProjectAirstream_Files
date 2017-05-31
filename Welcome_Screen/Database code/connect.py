from MySQLdb import *
from numpy import *
from database import *


def Connect():
	db = Database()

# db = MySQLdb.connect(host="localhost",    # your host, usually localhost
#                      user="root",         # your username
#                      passwd="MrMiller7",  # your password
#                      db="rfid_db")        # name of the data base

# # you must create a Cursor object. It will let
# #  you execute all the queries you need
# cur = db.cursor()

# # Use all the SQL you like
# q = 
# cur.execute("SELECT * FROM YOUR_TABLE_NAME")

# # print all the first cell of all the rows
# for row in cur.fetchall():
#     print row[0]

# db.close()


############# Execute as script ###############
if __name__ == "__main__":
	Connect()
