import os
import glob

import sys
import Adafruit_DHT
import time
import csv

import smbus

import os
import MySQLdb

userID = 11

""" TSL2561 light """

bus = smbus.SMBus(1)
bus.write_byte_data(0x39, 0x00 | 0x80, 0x03)
bus.write_byte_data(0x39, 0x01 | 0x80, 0x02)

""" DTH22 temperature & humidity """
#humidity, temperature = Adafruit_DHT.read_retry(Adafruit_DHT.DHT22, 14)


""" DS15B20 temperature of water """

os.system('modprobe w1-gpio')
os.system('modprobe w1-therm')
 
base_dir = '/sys/bus/w1/devices/'
device_folder = glob.glob(base_dir + '28*')[0]
device_file = device_folder + '/w1_slave'
 
def read_temp_raw():
    f = open(device_file, 'r')
    lines = f.readlines()
    f.close()
    return lines
 
def read_temp():
    lines = read_temp_raw()
    while lines[0].strip()[-3:] != 'YES':
        time.sleep(0.2)
        lines = read_temp_raw()
    equals_pos = lines[1].find('t=')
    if equals_pos != -1:
        temp_string = lines[1][equals_pos+2:]
        temp_c = float(temp_string) / 1000.0
        #temp_f = temp_c * 9.0 / 5.0 + 32.0
        return temp_c #, temp_f

"""Database NAJA JUB JUB"""

db = MySQLdb.connect(host = "ec2-54-169-202-67.ap-southeast-1.compute.amazonaws.com", 
					user = "plantopia_reg", 
					passwd = "plantopiapass", 
					db = "plantopiadb")
cur = db.cursor()

""" Print NAJA JUB """
i = 1
while i:

	localtime = time.asctime( time.localtime(time.time()) )
	localtimeSplit = localtime.split(" ")
	carrDate = localtimeSplit[0]+" "+localtimeSplit[2]+" "+localtimeSplit[1]+" "+localtimeSplit[4]
	carrTime = localtimeSplit[3]
	#DHT22
	humidity, temperature = Adafruit_DHT.read_retry(Adafruit_DHT.DHT22, 4)
	#DS18B20
	waterTemp = read_temp()
	#TSL2561
	data = bus.read_i2c_block_data(0x39, 0x0C | 0x80, 2)
	data1 = bus.read_i2c_block_data(0x39, 0x0E | 0x80, 2)
	ch0 = data[1] * 256 + data[0]
	#ch1 = data1[1] * 256 + data1[0]
	
	print "Date: {0:>3}  Time:  {1:>3}".format(carrDate,carrTime)
	
	#DTH22
	print 'Air Temperature:      {0:0.1f} *C'.format(temperature)
	print 'Humidity:             {0:0.1f}%'.format(humidity)
	
	#DS18B20
	print 'Water Temperature:    {0:0.1f} *C'.format(waterTemp)
	
	#TSL2561
	print 'Light Intensity:      {0} lux'.format(ch0) #(IR + Visible)
	#print "Infrared Value :%d lux" %ch1
	#print "Visible Value :%d lux" %(ch0 - ch1)

	#Database
	sql = "INSERT INTO user_potInput(userID,light,waterTemp,temperature,diameter,pumpSpeed) VALUES (%s,%s,%s,%s,%s,%s)"
	data = (userID,ch0,waterTemp,temperature,4.0,6.0) #hard code plantID, diameter, pumpSpeed
	cur.execute(sql, data)
	db.commit()
	cur.execute("SELECT * FROM user_potInput")
	print '==Query Database=='
	for row in cur.fetchall() :
		print row
	print '-------------------------------------------------------------------------------------------'
	i = i+1         
	time.sleep(5)