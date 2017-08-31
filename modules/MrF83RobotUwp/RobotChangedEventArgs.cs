using System;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MrF83RobotUwp
{
    /// <summary>
    /// Contains the 
    /// <see cref="MrF83RobotUwp.RobotType"/> 
    /// returned by a 
    /// <see cref="MrF83RobotUwp.RobotGui.RobotChanged"/> 
    /// event.
    /// </summary>
    public class RobotChangedEventArgs : EventArgs
    {
        private RobotType _Robot;

        /// <summary>
        /// The current robot model.
        /// </summary>
        public RobotType Robot
        {
            get { return _Robot; }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MrF83RobotUwp.RobotChangedEventArgs"/>
        /// class to provide the robot model.
        /// </summary>
        /// <param name="robot"></param>
        public RobotChangedEventArgs(RobotType robot)
        {
            _Robot = robot;
        }
    }
}
