from SimpleCar import SimpleCar
from WebEP import WebEP
import time
import sys

def main():
    robot = SimpleCar()

    a = 0
    while a < 5:
        WebEP.BroadcastEP()
        time.sleep(1)
        a = a + 1
        
    serv = WebEP()
    serv.run(robot)

if __name__ == "__main__":
    sys.exit(int(main() or 0))