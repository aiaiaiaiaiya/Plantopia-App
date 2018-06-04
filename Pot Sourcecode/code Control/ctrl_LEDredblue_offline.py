import time
import RPi.GPIO as GPIO
GPIO.setmode(GPIO.BOARD)
GPIO.setup(40, GPIO.OUT)
GPIO.setup(38, GPIO.OUT)

p = GPIO.PWM(40, 50)  # channel=16 frequency=490Hz
GPIO.output(38, GPIO.HIGH)
p.start(80)

try:
    while 1:
        dc = raw_input("Change Pump speed: ")
        if dc == 's':
            print 'Stop Water Pump'
            break
        else:
            p.ChangeDutyCycle(float(dc))
            print dc
except KeyboardInterrupt:
    pass
p.stop()
GPIO.output(38, GPIO.LOW)
GPIO.cleanup()
