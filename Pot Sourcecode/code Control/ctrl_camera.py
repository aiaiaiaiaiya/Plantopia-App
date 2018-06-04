import os
import picamera
import time
import MySQLdb
import os

# """Database NAJA JUB JUB"""
db = MySQLdb.connect(host = "ec2-54-169-202-67.ap-southeast-1.compute.amazonaws.com", 
					user = "plantopia_reg", 
					passwd = "plantopiapass", 
					db = "plantopiadb")
cur = db.cursor()

sql = 'SELECT part FROM part_image_in_server WHERE part_image_in_server.timestamp = (SELECT MAX(part_image_in_server.timestamp) FROM part_image_in_server)'
cur.execute(sql)
db.commit()
currNo = '1'
part = cur.fetchone()

if part is not None:	#Database have part
	previousNo = part[0].split("/")[5].split("_")[1].split(".")[0]
	currNo = int(previousNo) + 1
	
sql = 'INSERT INTO part_image_in_server(userID,plantID,part) VALUES(%s,%s,%s)'
partToDB = '/home/ubuntu/picture/user1/11_'+str(currNo).zfill(2)+'.jpg'
data = ('11','1',partToDB)
cur.execute(sql,data)
db.commit()
db.close()

filename = '/home/pi/codeControl/picture/user1/11_'+str(currNo).zfill(2)+'.jpg'

# os.system('sudo python /home/pi/codeControl/ctrl_LED.py white')

camera = picamera.PiCamera()
camera.brightness = 50
camera.capture(filename)

#upload image to server
permission = 'chmod 400 plantopia.pem'
os.system(permission)
textToInsertImage = 'scp -i plantopia.pem '+filename+' ubuntu@54.169.202.67:/home/ubuntu/picture/user1/'
os.system(textToInsertImage)