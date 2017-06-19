#!/usr/bin/env python

import cgi
import cgitb; cgitb.enable()
import time
import connect 


print ("Content-Type: text/html")
print ("")

arguments = cgi.FieldStorage()

quantityCoffee=0
quantityEspresso=0

if arguments.has_key('coffee'):
    quantityCoffee=arguments.getvalue('coffee')
    for i in range(0,quantityCoffee):
        if(i == 0):
            connect.MakeCoffee(False)
        elif(i == 1):
            connect.MakeCoffee(True)
       
if arguments.has_key('water'):
   quantityEspresso=arguments.getvalue('water')
   for i in range(0,quantityEspresso):
        if(i == 0):
            connect.MakeEspresso(False)
        elif(i == 1):
            connect.MakeEspresso(True)
       


print ('Quantity coffee: %s<br>' % quantityCoffee)
print ('Quantity Espresso: %s' % quantityEspresso)

#Here we have to call the funtion for coffee order with parameters
#quantityCoffee and quantityWater


#Just for logs we write down to see wheter the order was correctly recognized
with open('log.txt', 'a') as f:
    f.write('Order successful: %s coffee and %s water at time %s\n' % (quantityCoffee,quantityWater,time.strftime("%d.%m.%Y %H:%M:%S")))