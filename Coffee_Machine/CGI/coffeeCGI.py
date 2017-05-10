#!/usr/bin/env python

import cgi
import cgitb; cgitb.enable()
import time

print "Content-Type: text/html"
print ""

arguments = cgi.FieldStorage()

quantityCoffee=0
quantityWater=0

if arguments.has_key('coffee'):
   quantityCoffee=arguments.getvalue('coffee')

if arguments.has_key('water'):
   quantityWater=arguments.getvalue('water')


print 'Quantity coffee: %s<br>' % quantityCoffee
print 'Quantity water: %s' % quantityWater

#Here we have to call the funtion for coffee order with parameters
#quantityCoffee and quantityWater


#Just for logs we write down to see wheter the order was correctly recognized
with open('log.txt', 'a') as f:
    f.write('Order successful: %s coffee and %s water at time %s\n' % (quantityCoffee,quantityWater,time.strftime("%d.%m.%Y %H:%M:%S")))


