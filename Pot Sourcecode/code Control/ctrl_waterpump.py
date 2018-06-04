import time
import RPi.GPIO as GPIO
import MySQLdb

GPIO.setmode(GPIO.BOARD)
GPIO.setup(16, GPIO.OUT)
GPIO.setup(18, GPIO.OUT)

p = GPIO.PWM(16, 50)  # channel=16 frequency=0Hz
GPIO.output(18, GPIO.HIGH)
p.start(0)

try:
	while 1:
		#access database
		db = MySQLdb.connect(host="ec2-54-169-202-67.ap-southeast-1.compute.amazonaws.com",    # your host, usually localhost
                     user="plantopia_dev",         # your username
                     passwd="plantopiadevpass",  # your password 
                     db="plantopiadb")        # name of the data base
		cur = db.cursor()
		# Use all the SQL you like
		sql = "SELECT pumpSpeed FROM user_control WHERE user_control.timestamp = (SELECT MAX(user_control.timestamp) FROM user_control WHERE pumpSpeed IS NOT NULL)"
		cur.execute(sql)
		db.commit()
		# print all the first cell of all the rows
		PumpSpeed = cur.fetchone()

		db.close()

		#print PumpSpeed[0]

		p.ChangeDutyCycle(float(PumpSpeed[0]))

		time.sleep(3)


except KeyboardInterrupt:
    pass
p.stop()
GPIO.output(18, GPIO.LOW)
GPIO.cleanup()