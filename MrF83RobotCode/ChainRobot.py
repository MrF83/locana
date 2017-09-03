from AbstractRobot import AbstractRobot
from ev3dev.ev3 import *

class ChainRobot(AbstractRobot):
    """Representation of the ChainRobot Model"""

    def __init__(self):
        super().__init__()
        self.DriveMotorL = LargeMotor("outA")
        self.DriveMotorR = LargeMotor("outB")
        self.VerticalMotor = MediumMotor("outC")
        self.running = [False, ""]

    def Stop(self):
        self.DriveMotorL.stop(stop_action="coast")
        self.DriveMotorR.stop(stop_action="coast")
        self.VerticalMotor.stop(stop_action="hold")

    def React(self, msg):

        decodedMsg = self.DecodeMsg(msg)
        self.SetSpeeds(decodedMsg)

        if decodedMsg["State"]  == "P":
            if decodedMsg["Key"] == "A":
                if self.running[1] == "W" or self.running[1] == "":
                    self.DriveMotorL.run_forever(speed_sp = -self.CurveInnerMotorSpeed)
                    self.DriveMotorR.run_forever(speed_sp = -self.DriveSpeed)
                elif self.running[1] == "S":
                    self.DriveMotorR.run_forever(speed_sp = self.CurveInnerMotorSpeed)
                    self.DriveMotorL.run_forever(speed_sp = self.DriveSpeed)
                else:
                    return

            elif decodedMsg["Key"] == "D":
                if self.running[1] == "W" or self.running[1] == "":
                    self.DriveMotorL.run_forever(speed_sp = -self.DriveSpeed)
                    self.DriveMotorR.run_forever(speed_sp = -self.CurveInnerMotorSpeed)
                elif self.running[1] == "S":
                    self.DriveMotorR.run_forever(speed_sp = self.DriveSpeed)
                    self.DriveMotorL.run_forever(speed_sp = self.CurveInnerMotorSpeed)
                else:
                    return

            elif decodedMsg["Key"] == "W":
                self.DriveMotorL.run_forever(speed_sp = -self.DriveSpeed)
                self.DriveMotorR.run_forever(speed_sp = -self.DriveSpeed)
                self.running = [True, "W"]

            elif decodedMsg["Key"] == "S":
                self.DriveMotorL.run_forever(speed_sp = self.DriveSpeed)
                self.DriveMotorR.run_forever(speed_sp = self.DriveSpeed)
                self.running = [True, "S"]

            elif decodedMsg["Key"] == "Q":
                self.VerticalMotor.run_forever(speed_sp = self.VerticalTurnSpeed)

            elif decodedMsg["Key"] == "E":
                self.VerticalMotor.run_forever(speed_sp = -self.VerticalTurnSpeed)

            else:
                return

        elif decodedMsg["State"]  == "R":
            if decodedMsg["Key"] == "A":
                if self.running[0] and self.running[1] == "W":
                    self.DriveMotorL.run_forever(speed_sp = -self.DriveSpeed)
                    self.DriveMotorR.run_forever(speed_sp = -self.DriveSpeed)
                elif self.running[0] and self.running[1] == "S":
                    self.DriveMotorL.run_forever(speed_sp = self.DriveSpeed)
                    self.DriveMotorR.run_forever(speed_sp = self.DriveSpeed)
                else:
                    self.DriveMotorL.stop(stop_action="coast")
                    self.DriveMotorR.stop(stop_action="coast")

            elif decodedMsg["Key"] == "D":
                if self.running[0] and self.running[1] == "W":
                    self.DriveMotorL.run_forever(speed_sp = -self.DriveSpeed)
                    self.DriveMotorR.run_forever(speed_sp = -self.DriveSpeed)
                elif self.running[0] and self.running[1] == "S":
                    self.DriveMotorL.run_forever(speed_sp = self.DriveSpeed)
                    self.DriveMotorR.run_forever(speed_sp = self.DriveSpeed)
                else:
                    self.DriveMotorL.stop(stop_action="coast")
                    self.DriveMotorR.stop(stop_action="coast")
                
            elif decodedMsg["Key"] == "S":
                self.DriveMotorL.stop(stop_action="coast")
                self.DriveMotorR.stop(stop_action="coast")
                self.running = [False, ""]

            elif decodedMsg["Key"] == "W":
                self.DriveMotorL.stop(stop_action="coast")
                self.DriveMotorR.stop(stop_action="coast")
                self.running = [False, ""]

            elif decodedMsg["Key"] == "Q":
                self.VerticalMotor.stop(stop_action="hold")

            elif decodedMsg["Key"] == "E":
                self.VerticalMotor.stop(stop_action="hold")

            else:
                return

        else:
            return
