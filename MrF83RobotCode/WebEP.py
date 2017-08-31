from http.server import BaseHTTPRequestHandler, HTTPServer
from MyHandler import MyHandler
import socket

class S(HTTPServer):
    def serve_forever(self, robot):
        self.RequestHandlerClass.robot = robot
        HTTPServer.serve_forever(self)

class WebEP():
    """description of class"""

    def __init__(self):
        super().__init__()
        #self.robot = robot
        self.ip = socket.gethostbyname(socket.gethostname())
        print(self.ip)
        self.port = 80

    def BroadcastEP():
        ip = socket.gethostbyname(socket.gethostname())
        msg = "RobotIsOn" + ip + "RobotIsOn"
        sender = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        sender.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        sender.setsockopt(socket.SOL_SOCKET, socket.SO_BROADCAST, 1)
        sender.sendto(msg.encode("utf-8"), ('255.255.255.255', 54545))
        print("posted: ", msg, " on ", ip)
        
    def run(self, robot):
        ip = socket.gethostbyname(socket.gethostname())
        server_address = (ip , 80)
        httpd = S(server_address, MyHandler, True)
        #httpd.handle()
        print("run server")
        httpd.serve_forever(robot)