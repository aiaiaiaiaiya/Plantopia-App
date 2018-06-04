import time
import RPi.GPIO as GPIO
import MySQLdb


GPIO.setmode(GPIO.BOARD)
GPIO.setup(13, GPIO.OUT)
GPIO.setup(15, GPIO.OUT)

#turn on food pumps for  2sec.
GPIO.output(13, GPIO.HIGH)
GPIO.output(15, GPIO.HIGH)

time.sleep(2)

GPIO.output(13, GPIO.LOW)
GPIO.output(15, GPIO.LOW)

GPIO.cleanup()