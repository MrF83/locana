from AbstractRobot import AbstractRobot
from ev3dev.ev3 import *

class ChainRobot(AbstractRobot):
    """Representation of the ChainRobot Model"""

    def __init__(self):
        super().__init__()
        self.DriveMotorL = LargeMotor("outA")
        self.DriveMotorR = LargeMotor("outB")
        self.VerticalMotor = MediumMotor("outC")

    def Stop(self):
        self.DriveMotorL.stop(stop_action="coast")
        self.DriveMotorR.stop(stop_action="coast")
        self.VerticalMotor.stop(stop_action="coast")

    def React(self, msg):

        decodedMsg = self.DecodeMsg(msg)
        self.SetSpeeds(decodedMsg)

        if decodedMsg["State"]  == "P":
            if decodedMsg["Key"] == "D":
                self.DriveMotorL.run_forever(speed_sp = self.CurveInnerMotorSpeed)
                self.DriveMotorR.run_forever(speed_sp = self.DriveSpeed)

            elif decodedMsg["Key"] == "W":
                self.DriveMotorL.run_forever(speed_sp = -self.DriveSpeed)
                self.DriveMotorR.run_forever(speed_sp = -self.DriveSpeed)

            elif decodedMsg["Key"] == "A":
                self.DriveMotorL.run_forever(speed_sp = self.DriveSpeed)
                self.DriveMotorR.run_forever(speed_sp = self.CurveInnerMotorSpeed)

            elif decodedMsg["Key"] == "S":
                self.DriveMotorL.run_forever(speed_sp = self.DriveSpeed)
                self.DriveMotorR.run_forever(speed_sp = self.DriveSpeed)

            elif decodedMsg["Key"] == "Q":
                self.VerticalMotor.run_forever(speed_sp = self.VerticalTurnSpeed)

            elif decodedMsg["Key"] == "E":
                self.VerticalMotor.run_forever(speed_sp = -self.VerticalTurnSpeed)

            else:
                return

        elif decodedMsg["State"]  == "R":
            if decodedMsg["Key"] == "A":
                self.DriveMotorL.stop(stop_action="coast")
                self.DriveMotorR.stop(stop_action="coast")

            elif decodedMsg["Key"] == "S":
                self.DriveMotorL.stop(stop_action="coast")
                self.DriveMotorR.stop(stop_action="coast")

            elif decodedMsg["Key"] == "D":
                self.DriveMotorL.stop(stop_action="coast")
                self.DriveMotorR.stop(stop_action="coast")

            elif decodedMsg["Key"] == "W":
                self.DriveMotorL.stop(stop_action="coast")
                self.DriveMotorR.stop(stop_action="coast")

            elif decodedMsg["Key"] == "Q":
                self.VerticalMotor.stop(stop_action="hold")

            elif decodedMsg["Key"] == "E":
                self.VerticalMotor.stop(stop_action="hold")

            else:
                return

        else:
            return
