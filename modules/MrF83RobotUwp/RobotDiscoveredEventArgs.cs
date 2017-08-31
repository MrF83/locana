using System;

namespace MrF83RobotUwp
{
    /// <summary>
    /// Contains the 
    /// <see cref="System.Uri"/> 
    /// to the robot returned by a 
    /// <see cref="MrF83RobotUwp.RobotDiscovery.RobotDiscovered"/> 
    /// event.
    /// </summary>
    public class RobotDiscoveredEventArgs : EventArgs
    {
        private Uri _robotEndpoint;

        /// <summary>
        /// The 
        /// <see cref="System.Uri"/> 
        /// to the robot.
        /// </summary>
        public Uri RobotEndpoint
        {
            get { return _robotEndpoint; }
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="MrF83RobotUwp.RobotDiscoveredEventArgs"/> 
        /// class based on the parsed 
        /// <see cref="System.Uri"/> 
        /// .
        /// </summary>
        /// <param name="robotEndpoint">The discovered 
        /// <see cref="System.Uri"/> 
        /// to the robot.</param>
        public RobotDiscoveredEventArgs(Uri robotEndpoint)
        {
            _robotEndpoint = robotEndpoint;
        }
    }
}