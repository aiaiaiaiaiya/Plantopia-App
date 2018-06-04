import time
import RPi.GPIO as GPIO
import MySQLdb


GPIO.setmode(GPIO.BOARD)
GPIO.setup(13, GPIO.OUT)
GPIO.setup(15, GPIO.OUT)


		

try:
	while 1:
		#access database
		db = MySQLdb.connect(host="ec2-54-169-202-67.ap-southeast-1.compute.amazonaws.com",    # your host, usually localhost
                     user="plantopia_dev",         # your username
                     passwd="plantopiadevpass",  # your password 
                     db="plantopiadb")        # name of the data base
		cur = db.cursor()
		# Use all the SQL you like
		sql = "SELECT nutrientValve FROM user_control WHERE user_control.timestamp = (SELECT MAX(user_control.timestamp) FROM user_control WHERE nutrientValve IS NOT NULL)"
		cur.execute(sql)
		db.commit()
		# print all the first cell of all the rows
		nutrientValve = cur.fetchone()

		db.close()

		#print nutrientValve[0]

		if nutrientValve[0] == 1:
			#turn on food pumps for  2sec.
			GPIO.output(13, GPIO.HIGH)
			GPIO.output(15, GPIO.HIGH)

			time.sleep(2)

			GPIO.output(13, GPIO.LOW)
			GPIO.output(15, GPIO.LOW)

			#tell database that already done function
			#access database
			db = MySQLdb.connect(host="ec2-54-169-202-67.ap-southeast-1.compute.amazonaws.com",    # your host, usually localhost
	                     user="plantopia_dev",         # your username
	                     passwd="plantopiadevpass",  # your password 
	                     db="plantopiadb")        # name of the data base
			cur = db.cursor()
			# Use all the SQL you like
			sql = "INSERT INTO user_control VALUES (1, NULL, NULL, NULL, 0)"
			cur.execute(sql)
			db.commit()
			# print all the first cell of all the rows
			nutrentValve = cur.fetchone()

			db.close()

			#print "Done"

		time.sleep(3)

except KeyboardInterrupt:
    pass

GPIO.cleanup()