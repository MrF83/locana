from AbstractRobot import AbstractRobot
from ev3dev.ev3 import *

class SimpleCar(AbstractRobot):
    """description of class"""

    def __init__(self):
        super().__init__()
        self.SteeringMotor = LargeMotor("outB")
        self.DriveMotor = MediumMotor("outC")
        self.SteeringMotor.reset()

    def Stop(self):
        self.SteeringMotor.stop(stop_action="hold")
        self.DriveMotor.stop(stop_action="coast")

    def React(self, msg):

        decodedMsg = self.DecodeMsg(msg)
        self.SetSpeeds(decodedMsg)

        if decodedMsg["State"]  == "P":
            if decodedMsg["Key"] == "A":
                self.SteeringMotor.run_to_abs_pos(position_sp=80, speed_sp=1000, stop_action="hold")

            elif decodedMsg["Key"] == "S":
                self.DriveMotor.run_forever(speed_sp = -self.DriveSpeed)

            elif decodedMsg["Key"] == "D":
                self.SteeringMotor.run_to_abs_pos(position_sp=-80, speed_sp=1000, stop_action="hold")


            elif decodedMsg["Key"] == "W":
                self.DriveMotor.run_forever(speed_sp = self.DriveSpeed)

            else:
                return

        elif decodedMsg["State"]  == "R":
            if decodedMsg["Key"] == "A" or decodedMsg["Key"] == "D":
                self.SteeringMotor.run_to_abs_pos(position_sp=0, speed_sp=1000, stop_action="hold")

            elif decodedMsg["Key"] == "S" or decodedMsg["Key"] == "W":
                self.DriveMotor.stop(stop_action="coast")

            else:
                return

        else:
            return