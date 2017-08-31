using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MrF83RobotUwp
{
    /// <summary>
    /// Provides the Gui used by the 
    /// <see cref="MrF83RobotUwp.RobotClient"/> 
    /// class.
    /// </summary>
    public sealed partial class RobotGui : UserControl
    {
        public delegate void RobotChangedEventHandler(object sender, RobotChangedEventArgs e);

        /// <summary>
        /// Occurs when the user choose a other robot model.
        /// </summary>
        public event RobotChangedEventHandler RobotChanged;

        public delegate void RobotValuesChangedEventHandler(object sender, RobotValuesChangedEventArgs e);

        /// <summary>
        /// Occurs when any Slinder changes its value.
        /// </summary>
        public event RobotValuesChangedEventHandler ValuesChanged;

        public delegate void RobotOnEventHandler(object sender);

        /// <summary>
        /// Occurs when the user changes the state of the ToggleButton to ON.
        /// </summary>
        public event RobotOnEventHandler RobotOn;

        public delegate void RobotOffEventHandler(object sender);

        /// <summary>
        /// Occurs when the user changes the state of the ToggleButton to OFF.
        /// </summary>
        public event RobotOffEventHandler RobotOff;

        private RobotType _CurrentRobot;
        private bool _isInitializing;

        private int _driveSpeed;

        /// <summary>
        /// The Value of the DriveSpeed Slider.
        /// </summary>
        public int DriveSpeed
        {
            get { return _driveSpeed; }
        }
        private int _innerMotorSpeed;

        /// <summary>
        /// The Value of the CurveInnerMotorSpeed Slider.
        /// </summary>
        public int CurveInnerMotorSpeed
        {
            get { return _innerMotorSpeed; }
        }

        private int _horizontalTurnSpeed;

        /// <summary>
        /// The Value of the HorizontalTurnSpeed Slider.
        /// </summary>
        public int HorizontalTurnSpeed
        {
            get { return _horizontalTurnSpeed; }
        }

        private int _verticalTurnSpeed;

        /// <summary>
        /// The Value of the VerticalTurnSpeed Slider.
        /// </summary>
        public int VerticalTurnSpeed
        {
            get { return _verticalTurnSpeed; }
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="MrF83RobotUwp.RobotGui"/> 
        /// .
        /// </summary>
        public RobotGui()
        {
            _isInitializing = true;
            this.InitializeComponent();
            robotSelection.Items.Add(RobotType.ChainRobot.ToString("g"));
            robotSelection.Items.Add(RobotType.CableCar.ToString("g"));
            robotSelection.Items.Add(RobotType.TwoAxisGimbal.ToString("g"));
            robotSelection.Items.Add(RobotType.SimpleCar.ToString("g"));
            robotSelection.SelectedIndex = 0;
            _CurrentRobot = (RobotType) Enum.Parse(typeof(RobotType), robotSelection.SelectedItem.ToString());
            robotSelection.SelectionChangedTrigger = ComboBoxSelectionChangedTrigger.Committed;
            driveSpeed.IsEnabled = false;
            innerMotorSpeed.IsEnabled = false;
            verticalTurnSpeed.IsEnabled = false;
            horizontalTurnSpeed.IsEnabled = false;
            robotSelection.IsEnabled = false;
            _isInitializing = false;
        }

        /// <summary>
        /// Triggers the 
        /// <see cref="Windows.UI.Xaml.Controls.ProgressRing"/> 
        /// on the Gui to show there is working something in the background.
        /// </summary>
        public async void StartWorking()
        {
            await robotProgressRing.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { robotProgressRing.IsActive = true; });
        }

        /// <summary>
        /// Shuts down the
        /// <see cref="Windows.UI.Xaml.Controls.ProgressRing"/> 
        /// on the Gui to show everything is ready.
        /// </summary>
        public async void EndWorking()
        {
            await robotProgressRing.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { robotProgressRing.IsActive = false; });
        }

        private void RobotToggle_Toggled(object sender, RoutedEventArgs e)
        {
            bool isOn = (sender as ToggleSwitch).IsOn;
            driveSpeed.IsEnabled = isOn;
            innerMotorSpeed.IsEnabled = isOn;
            verticalTurnSpeed.IsEnabled = isOn;
            horizontalTurnSpeed.IsEnabled = isOn;
            robotSelection.IsEnabled = isOn;
            if (isOn)
            {
                RobotOn?.Invoke(this);
                OnRobotChange(_CurrentRobot);
            }
            else
            {
                RobotOff?.Invoke(this);
            }
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var a = sender as Slider;
            if (!_isInitializing)
            {
                TextBlock txt = FindName(a.Name + "Tb") as TextBlock;
                txt.Text = e.NewValue.ToString();
                _driveSpeed = (int)driveSpeed.Value;
                _innerMotorSpeed = (int)innerMotorSpeed.Value;
                _horizontalTurnSpeed = (int)horizontalTurnSpeed.Value;
                _verticalTurnSpeed = (int)verticalTurnSpeed.Value;
                ValuesChanged?.Invoke(this, new RobotValuesChangedEventArgs(DriveSpeed, CurveInnerMotorSpeed, HorizontalTurnSpeed, VerticalTurnSpeed));
            }
        }

        private void RobotSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RobotType newRobot = (RobotType)Enum.Parse(typeof(RobotType), robotSelection.SelectedItem.ToString());
            if (_CurrentRobot == newRobot)
            {
                return;
            }
            else
            {
                OnRobotChange(newRobot);
            }
        }

        private void OnRobotChange(RobotType newRobot)
        {
            _CurrentRobot = newRobot;
            _isInitializing = true;
            switch (newRobot)
            {
                case RobotType.ChainRobot:
                    InitializeChainRobot();
                    break;
                case RobotType.CableCar:
                    InitializeCableCar();
                    break;
                case RobotType.TwoAxisGimbal:
                    InitializeTwoAxisGimbal();
                    break;
                case RobotType.SimpleCar:
                    InitializeSimpleCar();
                    break;
            }
            _isInitializing = false;
            RobotChanged?.Invoke(this, new RobotChangedEventArgs(newRobot));
            ValuesChanged?.Invoke(this, new RobotValuesChangedEventArgs((int)driveSpeed.Value, (int)innerMotorSpeed.Value, (int)horizontalTurnSpeed.Value, (int)verticalTurnSpeed.Value));
        }

        private void InitializeSimpleCar()
        {
            if (!driveSpeed.IsEnabled) { driveSpeed.IsEnabled = true; }
            if (innerMotorSpeed.IsEnabled) { innerMotorSpeed.IsEnabled = false; }
            if (verticalTurnSpeed.IsEnabled) { verticalTurnSpeed.IsEnabled = false; }
            if (horizontalTurnSpeed.IsEnabled) { horizontalTurnSpeed.IsEnabled = false; }
            driveSpeed.Value = 100;
        }

        private void InitializeChainRobot()
        {
            if (!driveSpeed.IsEnabled) { driveSpeed.IsEnabled = true; }
            if (!innerMotorSpeed.IsEnabled) { innerMotorSpeed.IsEnabled = true; }
            if (!verticalTurnSpeed.IsEnabled) { verticalTurnSpeed.IsEnabled = true; }
            if (horizontalTurnSpeed.IsEnabled) { horizontalTurnSpeed.IsEnabled = false; }
            driveSpeed.Value = 100;
            innerMotorSpeed.Value = 0;
            verticalTurnSpeed.Value = 100;
        }

        private void InitializeCableCar()
        {
            if (!driveSpeed.IsEnabled) { driveSpeed.IsEnabled = true; }
            if (innerMotorSpeed.IsEnabled) { innerMotorSpeed.IsEnabled = false; }
            if (!verticalTurnSpeed.IsEnabled) { verticalTurnSpeed.IsEnabled = true; }
            if (!horizontalTurnSpeed.IsEnabled) { horizontalTurnSpeed.IsEnabled = true; }
            driveSpeed.Value = 100;
            verticalTurnSpeed.Value = 100;
            horizontalTurnSpeed.Value = 100;
        }

        private void InitializeTwoAxisGimbal()
        {
            if (driveSpeed.IsEnabled) { driveSpeed.IsEnabled = false; }
            if (innerMotorSpeed.IsEnabled) { innerMotorSpeed.IsEnabled = false; }
            if (!verticalTurnSpeed.IsEnabled) { verticalTurnSpeed.IsEnabled = true; }
            if (!horizontalTurnSpeed.IsEnabled) { horizontalTurnSpeed.IsEnabled = true; }
            verticalTurnSpeed.Value = 100;
            horizontalTurnSpeed.Value = 100;
        }
    }
}
