from datetime import datetime

import time
import MySQLdb
import argparse
import signal
import sys
import os
import RPi.GPIO as GPIO

userID = '11'

timeOn = 5 
timeOff = 23
#timeOff = 18
flagCamera = 0
dimCount = 0
dimRBLED = 30
rbLEDBrightness = dimRBLED

#Set dateTime format
date_format = '%Y-%m-%d'
time_format = '%H:%M:%S'

#Set for Super Bright LED
GPIO.setmode(GPIO.BOARD)
GPIO.setup(40, GPIO.OUT)
GPIO.setup(38, GPIO.OUT)
GPIO.setup(22, GPIO.OUT)

rbLED = GPIO.PWM(40, 80)  # channel=40 frequency=0Hz
GPIO.output(38, GPIO.HIGH)
GPIO.output(22, GPIO.LOW)
rbLED.start(0)

try:
	while True:
		db = MySQLdb.connect(host="ec2-54-169-202-67.ap-southeast-1.compute.amazonaws.com",    # your host, usually localhost
		                     user="plantopia_reg",         # your username
		                     passwd="plantopiapass",  # your password 
		                     db="plantopiadb")        # name of the data base
		cur = db.cursor()
		# Use all the SQL you like
		sql = 'SELECT light, user_control.timestamp FROM user_control WHERE user_control.timestamp = (SELECT MAX(user_control.timestamp) FROM user_control WHERE light IS NOT null AND userID = ' + userID +')'
		cur.execute(sql)
		db.commit()
		dataFromDB = cur.fetchone()
		db.close()

		print 'dataFromDB = '+str(dataFromDB[0])

		lidTime = str(dataFromDB[1])
		lidTime = lidTime.split(" ")[0]+" "+lidTime.split(" ")[1].split(":")[0]+":"+lidTime.split(" ")[1].split(":")[1]

		refDateTime = datetime.now().strftime(date_format +' '+ time_format) 
		currHour = refDateTime.split(' ')[1].split(':')[0]
		timeSetFlag = currHour +':'+refDateTime.split(' ')[1].split(':')[1]
		print 'timeSetFlag = '+timeSetFlag
		timeSetDim = refDateTime.split(' ')[0]+" "+timeSetFlag
		print 'timeSetDim = '+timeSetDim
		print "currHour = "+str(currHour)

		if dataFromDB[0] == '' or dataFromDB[0] is None:
			rbLEDBrightness = dimRBLED 
			print "1. dataFromDB[0]"
		# elif (str(lidTime) != timeSetDim and (dimCount%10 == 5)): #after drag the sun in application 1:10 min LED will dim
		# 	rbLEDBrightness = dimRBLED 
		# 	print "2. str(lidTime) != timeSetDim and (dimCount%10 == 9)"
		elif (str(lidTime) == timeSetDim):
			rbLEDBrightness = 100
			dimCount = 0
			print "2. str(lidTime) == timeSetDim"
		else:
			rbLEDBrightness = dimRBLED 
			print "3. else"
			
		cur = db.cursor()
		
		if (int(currHour) == timeOn) and (flagCamera == 0): #capture before turn on red and blue LED at 05:00 o'clock
			#rbLED.ChangeDutyCycle(float(0))
			#ctrlLED(strip,'white',0,LED_COUNT)
			GPIO.output(38, GPIO.LOW)
			GPIO.output(22, GPIO.HIGH)
			os.system('sudo python /home/pi/codeControl/ctrl_camera.py')
			flagCamera = 1
			GPIO.output(22, GPIO.LOW)
			#ctrlLED(strip,'violet',0,LED_COUNT)
			GPIO.output(38, GPIO.HIGH)
			rbLED.ChangeDutyCycle(float(rbLEDBrightness))
			print "4. timeOn"
		elif (int(currHour) == timeOff) and (flagCamera == 0): #capture after turn off red and blue LED at 23:00 o'clock
			#rbLED.ChangeDutyCycle(float(0))
			GPIO.output(38, GPIO.LOW)
			#ctrlLED(strip,'white',0,LED_COUNT)
			GPIO.output(22, GPIO.HIGH)
			os.system('sudo python /home/pi/codeControl/ctrl_camera.py')
			flagCamera = 1
			#ctrlLED(strip,'off',0,LED_COUNT)
			GPIO.output(22, GPIO.LOW)
			print "5. timeOff"
		else:
			if (timeSetFlag == '4:59') or (timeSetFlag == '04:59') or (timeSetFlag == '22:59'):
				flagCamera = 0
			else:
				if(int(currHour) in range(6,22)):
					GPIO.output(38, GPIO.HIGH)
					rbLED.ChangeDutyCycle(float(rbLEDBrightness))
					print "6. if1"
				else:
					#rbLED.ChangeDutyCycle(float(0))
					print "7. else"
					GPIO.output(38, GPIO.LOW)
		dimCount = dimCount + 1
		print "rbLEDBrightness = "+str(rbLEDBrightness)
		time.sleep(3)
		print "flagCamera : "+str(flagCamera)
except KeyboardInterrupt:
    pass

spbLED.stop()
GPIO.output(38, GPIO.LOW)
GPIO.output(22, GPIO.LOW)
GPIO.cleanup()