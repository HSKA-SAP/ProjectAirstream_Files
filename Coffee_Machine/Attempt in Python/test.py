import time
import serial
from telegramFormat import telegramFormat
from construct import *
from threading import Timer
from api_definitions import * 
import array
import struct 
from enum import *
import itertools

def stuffAndShift(arrayToShift):
    a = arrayToShift
    i = 0
    lengthA = len(a)
    while i < lengthA:
        for j in DPT_SpecialChar_t:
            if(a[i] == j.value):
                b = a[0:i] # before
                c = a[i+1:] # after
                stuff = DPT_SpecialChar_t.DLE_e.value
                shift = a[i] ^ DPT_SpecialChar_t.ShiftXOR_e.value
                a = b+[stuff,shift] + c 
                i = i + 1
                lengthA = len(a)
                break
        i +=1
    return a

def deStuffAndShift(arrayToDeShift):
    i = 0
    while(i < len(arrayToDeShift)):
        if(arrayToDeShift[i] == DPT_SpecialChar_t.DLE_e.value):
            b = (arrayToDeShift[i+1] ^ DPT_SpecialChar_t.ShiftXOR_e.value)
            c = arrayToDeShift[i+2:]
            d = arrayToDeShift[0:i]
            arrayToDeShift = d+[b]+c
        i+=1
    return arrayToDeShift

def toHex(array):
    b = [0]*len(array)
    for i in range(0,len(array)):
        b[i] = hex(array[i])
    return b

def telegramToIntArray(telegram):
    array = []
    a = struct.unpack('B'*len(telegram),telegram)
    array.append(a)
    return list(itertools.chain.from_iterable(array))


def ByteToBitArray(num, size=8):
    """Format a number as binary with leading zeros"""
    bitArrayString = str(bin(num)[2:]).zfill(size)
    bitArray = list(map(int, bitArrayString))
    return bitArray

def LowNibble(bitArray):
    return bitArray & 0x0F

def HighNibble(bitArray):
    return bitArray >> 4

a = ByteToBitArray(25)
b = LowNibble(25)
c = HighNibble(25)

print("25 as Byte:",a)
print(list(a))
print("Low Nibble:", b)
print("High Nibble:", c)
