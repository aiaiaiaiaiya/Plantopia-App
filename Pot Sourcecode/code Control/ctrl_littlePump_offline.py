import RPi.GPIO as GPIO
GPIO.setmode(GPIO.BOARD)
GPIO.setup(15, GPIO.OUT)

GPIO.output(15, GPIO.HIGH)

"""
try:
	while 1:
    	#GPIO.output(22, GPIO.HIGH)





except KeyboardInterrupt:
    GPIO.output(22, GPIO.LOW)

"""
try:
    while 1:
    	#GPIO.output(22, GPIO.HIGH)
        dc = raw_input("Enter [s] to stop: ")
        if dc == 's' or dc == 'S':
            print 'Stop Water Pump'
            break
except KeyboardInterrupt:
    pass

GPIO.output(15, GPIO.LOW)
GPIO.cleanup()