from AbstractRobot import AbstractRobot

class TwoAxisGimbal(AbstractRobot):
    """Representation of the TwoAxisGimbal Model"""

    def __init__(self):
        super().__init__()
        self.VerticalMotor = LargeMotor("outA")
        self.HorizontalMotor = LargeMotor("outB")

    def Stop(self):
        self.VerticalMotor.stop(stop_action = "hold")
        self.HorizontalMotor.stop(stop_action = "hold")

    def React(self, msg):

        decodedMsg = self.DecodeMsg(msg)
        self.SetSpeeds(decodedMsg)

        #TODO: Controller