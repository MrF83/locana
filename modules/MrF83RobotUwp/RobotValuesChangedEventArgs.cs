using System;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MrF83RobotUwp
{
    /// <summary>
    /// Contains all parameters returned by a 
    /// <see cref="MrF83RobotUwp.RobotGui.ValuesChanged"/> 
    /// event.
    /// </summary>
    public class RobotValuesChangedEventArgs : EventArgs
    {
        private int _driveSpeed;

        /// <summary>
        /// The Value of the DriveSpeed Slider of the sender class which is likely of type
        /// <see cref="MrF83RobotUwp.RobotGui"/>
        /// .
        /// </summary>
        public int DriveSpeed
        {
            get { return _driveSpeed; }
        }

        private int _innerSpeed;

        /// <summary>
        /// The Value of the CurveInnerMotorSpeed Slider of the sender class which is likely of type
        /// <see cref="MrF83RobotUwp.RobotGui"/>
        /// .
        /// </summary>
        public int CurveInnerMotorSpeed
        {
            get { return _innerSpeed; }
        }

        private int _horizontalSpeed;

        /// <summary>
        /// The Value of the HorizontalTurnSpeed Slider of the sender class which is likely of type
        /// <see cref="MrF83RobotUwp.RobotGui"/>
        /// .
        /// </summary>
        public int HorizontalSpeed
        {
            get { return _horizontalSpeed; }
        }

        private int _verticalSpeed;

        /// <summary>
        /// The Value of the VerticalTurnSpeed Slider of the sender class which is likely of type
        /// <see cref="MrF83RobotUwp.RobotGui"/>
        /// .
        /// </summary>
        public int VerticalSpeed
        {
            get { return _verticalSpeed; }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MrF83RobotUwp.RobotValuesChangedEventArgs"/>
        /// class based on the specific parameters found in the
        /// <see cref="MrF83RobotUwp.RobotGui"/>
        /// class.
        /// </summary>
        /// <param name="driveSpeed">Value of the DriveSpeed Slider.</param>
        /// <param name="innerSpeed">Value of the CurveInnerMotorSpeed Slider.</param>
        /// <param name="horizontalSpeed">Value of the HorizontalTurnSpeed Slider.</param>
        /// <param name="verticalSpeed">Value of the VerticalTurnSpeed Slider.</param>
        public RobotValuesChangedEventArgs(int driveSpeed, int innerSpeed, int horizontalSpeed, int verticalSpeed)
        {
            _driveSpeed = driveSpeed;
            _innerSpeed = innerSpeed;
            _horizontalSpeed = horizontalSpeed;
            _verticalSpeed = verticalSpeed;
        }
    }

}
