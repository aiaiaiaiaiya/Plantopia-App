from neopixel import *
from datetime import datetime

import time
import MySQLdb
import argparse
import signal
import sys
import os
import RPi.GPIO as GPIO

#Set for Super Bright LED
GPIO.setmode(GPIO.BOARD)
GPIO.setup(40, GPIO.OUT)
GPIO.setup(38, GPIO.OUT)

spbLED = GPIO.PWM(40, 50)  # channel=40 frequency=0Hz
GPIO.output(38, GPIO.HIGH)
spbLED.start(0)

#Set dateTime format
date_format = '%Y-%m-%d'
time_format = '%H:%M:%S'

timeOn1 = '5'
timeOn2 = '05'
timeOff = '23'
flagCamera = 0
dimCount = 0
dimBrightness = 80
LED_BRIGHTNESS = dimBrightness
dimSpbLED = 50
spbLEDBrightness = dimSpbLED

def ctrlLED(strip,color,sRangeZone,eRangeZone):
		if color == 'violet':
			for i in range(sRangeZone,eRangeZone):
				if i%3 == 0 or i%3 == 1:
					strip.setPixelColor(i, RED)
				elif i%3 == 2:
					strip.setPixelColor(i, BLUE)
				else:
					strip.setPixelColor(i, RED)
		elif color =='white':
			for i in range(sRangeZone,eRangeZone):
				strip.setPixelColor(i, WHITE)
		else:
			for i in range(sRangeZone,eRangeZone):
				strip.setPixelColor(i, OFF)
		strip.show()
try:
	while True:
		db = MySQLdb.connect(host="ec2-54-169-202-67.ap-southeast-1.compute.amazonaws.com",    # your host, usually localhost
		                     user="plantopia_reg",         # your username
		                     passwd="plantopiapass",  # your password 
		                     db="plantopiadb")        # name of the data base
		cur = db.cursor()
		# Use all the SQL you like
		sql = "SELECT light, user_control.timestamp FROM user_control WHERE user_control.timestamp = (SELECT MAX(user_control.timestamp) FROM user_control WHERE light IS NOT null)"
		cur.execute(sql)
		db.commit()
		dataFromDB = cur.fetchone()
		db.close()

		lidTime = str(dataFromDB[1])
		lidTime = lidTime.split(" ")[0]+" "+lidTime.split(" ")[1].split(":")[0]+":"+lidTime.split(" ")[1].split(":")[1]

		refDateTime = datetime.now().strftime(date_format +' '+ time_format) 
		currHour = refDateTime.split(' ')[1].split(':')[0]
		timeSetFlag = currHour +':'+refDateTime.split(' ')[1].split(':')[1]
		timeSetDim = refDateTime.split(' ')[0]+" "+timeSetFlag

		# LED strip configuration:
		LED_COUNT      = 74     # Number of LED pixels.
		LED_PIN        = 18      # GPIO pin connected to the pixels (18 uses PWM!).
		#LED_PIN        = 10      # GPIO pin connected to the pixels (10 uses SPI /dev/spidev0.0).
		LED_FREQ_HZ    = 800000  # LED signal frequency in hertz (usually 800khz)
		LED_DMA        = 10      # DMA channel to use for generating signal (try 10)
		if dataFromDB[0] == '' or dataFromDB[0] is None:
			LED_BRIGHTNESS = 0
		elif (str(lidTime) != timeSetDim and (dimCount%10 == 9)): #after drag the sun in application 1:10 min LED will dim
			LED_BRIGHTNESS = dimBrightness
			spbLEDBrightness = dimSpbLED 
		elif (str(lidTime) == timeSetDim):
			LED_BRIGHTNESS = int(dataFromDB[0])
			spbLEDBrightness = 100
			dimCount = 0

		LED_INVERT     = False   # True to invert the signal (when using NPN transistor level shift)
		LED_CHANNEL    = 0       # set to '1' for GPIOs 13, 19, 41, 45 or 53
		LED_STRIP      = ws.WS2811_STRIP_GRB   # Strip type and colour ordering

		OFF = Color(0, 0, 0)
		RED = Color(255, 0, 0)
		BLUE = Color(0, 0, 255)
		WHITE = Color(255,255,255)
			
		cur = db.cursor()

		strip = Adafruit_NeoPixel(LED_COUNT, LED_PIN, LED_FREQ_HZ, LED_DMA, LED_INVERT, LED_BRIGHTNESS, LED_CHANNEL, LED_STRIP)
		strip.begin()
		
		if ((currHour == timeOn1) or (currHour == timeOn2)) and (flagCamera == 0): #turn on
			spbLED.ChangeDutyCycle(float(0))
			ctrlLED(strip,'white',0,LED_COUNT)
			os.system('sudo python /home/pi/codeControl/ctrl_camera.py')
			flagCamera = 1
			ctrlLED(strip,'violet',0,LED_COUNT)
			spbLED.ChangeDutyCycle(float(spbLEDBrightness))
		elif (currHour == timeOff) and (flagCamera == 0): #turn off
			spbLED.ChangeDutyCycle(float(0))
			ctrlLED(strip,'white',0,LED_COUNT)
			os.system('sudo python /home/pi/codeControl/ctrl_camera.py')
			flagCamera = 1
			ctrlLED(strip,'off',0,LED_COUNT)
		else:
			if (timeSetFlag == '4:59') or (timeSetFlag == '04:59'):
				flagCamera = 0
			elif timeSetFlag == '22:59':
				flagCamera = 0
			else:
				if(currHour in ['6','06','7','07','8','08','9','09','10','11','12','13','14','15','16','17','18','19','20','21','22']):
					ctrlLED(strip,'violet',0,LED_COUNT)
					spbLED.ChangeDutyCycle(float(spbLEDBrightness))
				else:
					ctrlLED(strip,'off',0,LED_COUNT)
					spbLED.ChangeDutyCycle(float(0))
		dimCount = dimCount + 1
		time.sleep(3)

except KeyboardInterrupt:
    pass
LED_COUNT      = 74     # Number of LED pixels.
LED_PIN        = 18      # GPIO pin connected to the pixels (18 uses PWM!).
#LED_PIN        = 10      # GPIO pin connected to the pixels (10 uses SPI /dev/spidev0.0).
LED_FREQ_HZ    = 800000  # LED signal frequency in hertz (usually 800khz)
LED_DMA        = 10      # DMA channel to use for generating signal (try 10)
LED_BRIGHTNESS = 0
LED_INVERT     = False   # True to invert the signal (when using NPN transistor level shift)
LED_CHANNEL    = 0       # set to '1' for GPIOs 13, 19, 41, 45 or 53
LED_STRIP      = ws.WS2811_STRIP_GRB   # Strip type and colour ordering
strip = Adafruit_NeoPixel(LED_COUNT, LED_PIN, LED_FREQ_HZ, LED_DMA, LED_INVERT, LED_BRIGHTNESS, LED_CHANNEL, LED_STRIP)
strip.begin()
ctrlLED(strip,'off',0,LED_COUNT)

spbLED.stop()
GPIO.output(38, GPIO.LOW)
GPIO.cleanup()