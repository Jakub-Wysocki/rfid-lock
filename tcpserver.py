#!/usr/bin/python3

import socket
from datetime import datetime

def saveToFile(data, status):
	file = open('data.txt', 'a')
	currentTime = datetime.now()
	file.write(currentTime.strftime("%d/%m/%Y %H:%M:%S") + "-> " + data.decode('utf-8') + " with status " + status + "\n" )
	file.close()
def verifyTag(data):
	databaseFile = open('database.txt', 'r')
	if data.decode('utf-8') == "4287253630":
		saveToFile(data, "true")
		databaseFile.close()
		return True

	saveToFile(data, "false")
	return False
	databaseFile.close()

try:
	sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM) #Choose family and type of socket (TCP)
except:
	print("Failed to create socket")
	quit()

print ("Socket created")

# Set host and port
Host = '210.10.10.57'
Port = 50000

#start the protocol
sock.bind((Host, Port))

#recive and wait for next transmission
while True:
	sock.listen()
	conn, addr = sock.accept()
	with conn:
		while True:
			data = conn.recv(1024)
			if not data: 
				print("Transmission finished")
				break
			res = verifyTag(data)
			if res == True:
				conn.send("Access granted".encode())
			else:
				conn.send("Access denied".encode())
			break;
