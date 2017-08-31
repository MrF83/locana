from AbstractRobot import AbstractRobot
from ev3dev.ev3 import *

class CableCar(AbstractRobot):
    """Representation of the CableCar Model"""
    
    def __init__(self):
        super().__init__()
        self.DriveMotor = LargeMotor("outA")
        self.VerticalMotor = LargeMotor("outB")
        self.HorizontalMotor = MediumMotor("outC")

    def Stop(self):
        self.DriveMotor.stop(stop_action = "coast")
        self.VerticalMotor.stop(stop_action = "coast")
        self.HorizontalMotor.stop(stop_action = "coast")

    def React(self, msg):

        decodedMsg = self.DecodeMsg(msg)
        self.SetSpeeds(decodedMsg)

        #TODO: Controller