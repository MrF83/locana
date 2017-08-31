class AbstractRobot(object):
    """Representation of a Robot"""

    def __init__(self):
        super().__init__()
        self.DriveSpeed = 1000
        self.CurveInnerMotorSpeed = 1000
        self.HorizontalTurnSpeed = 1000
        self.VerticalTurnSpeed = 1000

    def DecodeMsg(self, msg):
        return {"Key" : msg[0],
                "State" : msg[1],
                "DriveSpeed" : int(msg[2:5])*10,
                "CurveInnerMotorSpeed" : int(msg[5:8])*10,
                "HorizontalTurnSpeed" : int(msg[8:11])*10,
                "VerticalTurnSpeed" : int(msg[11:14])*10}
    def SetSpeeds(self, decodedMsg):
        self.DriveSpeed = decodedMsg["DriveSpeed"]
        self.CurveInnerMotorSpeed = decodedMsg["CurveInnerMotorSpeed"]
        self.HorizontalTurnSpeed = decodedMsg["HorizontalTurnSpeed"]
        self.VerticalTurnSpeed = decodedMsg["VerticalTurnSpeed"]