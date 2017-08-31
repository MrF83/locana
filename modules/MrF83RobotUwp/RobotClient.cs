using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace MrF83RobotUwp
{
    /// <summary>
    /// Provides a class for interacting with robots.
    /// </summary>
    public class RobotClient
    {
        private RobotGui _gui;

        private bool _robotAvailable;

        private bool robotAvailable
        {
            get { return _robotAvailable; }
            set { _robotAvailable = value;
                if (value==false)
                {
                    Gui_RobotOn(this);
                }
            }
        }

        private bool _wasKeysetWSDown = false;
        private bool _wasKeysetADDown = false;
        private bool _wasKeysetQEDown = false;

        /// <summary>
        /// Holds the instance of the 
        /// <see cref="MrF83RobotUwp.RobotGui"/> 
        /// used by this 
        /// <see cref="MrF83RobotUwp.RobotClient"/> 
        /// .
        /// </summary>
        public RobotGui Gui
        {
            get { return _gui; }
        }

        private Uri _remoteUri;

        /// <summary>
        /// Holds the 
        /// <see cref="System.Uri"/> 
        /// to the robot.
        /// </summary>
        public Uri RemoteUri
        {
            get { return _remoteUri; }
        }

        private RobotDiscovery _discovery;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="MrF83RobotUwp.RobotClient"/> 
        /// class including an instance of the 
        /// <see cref="MrF83RobotUwp.RobotGui"/> 
        /// class stored in the 
        /// <see cref="MrF83RobotUwp.RobotClient.Gui"/> 
        /// proprty.
        /// </summary>
        public RobotClient()
        {
            _robotAvailable = false;
            _discovery = new RobotDiscovery(54545);
            _gui = new RobotGui();
            _gui.RobotOff += Gui_RobotOff;
            _gui.RobotOn += Gui_RobotOn;
        }

        private void Gui_RobotOff(object sender)
        {
            _robotAvailable = false;
        }

        private void Discovery_RobotDiscovered(object sender, RobotDiscoveredEventArgs e)
        {
            _discovery.RobotDiscovered -= Discovery_RobotDiscovered;
            _remoteUri = e.RobotEndpoint;
            robotAvailable = true;
            _gui.EndWorking();
        }

        private void Gui_RobotOn(object sender)
        {
            _gui.StartWorking();
            _discovery.RobotDiscovered += Discovery_RobotDiscovered;
            _discovery.GetRobotUriAsync();
        }

        /// <summary>
        /// The Handler used to hook on the KeyUp and KeyDown event of the parent programm.
        /// </summary>
        /// <param name="sender">A reference to the object which fired the event.</param>
        /// <param name="args">The 
        /// <see cref="Windows.UI.Core.KeyEventArgs"/> 
        /// provided by the KeyUp or KeyDown event.</param>
        public async void KeyActionHandler(object sender, KeyEventArgs args)
        {
            if (robotAvailable)
            {
                KeyStatus status;
                Key k;

                if (args.KeyStatus.WasKeyDown && !_wasKeysetWSDown)
                {
                    _wasKeysetWSDown = true;
                    status = KeyStatus.pressed;
                }
                else if (args.KeyStatus.WasKeyDown && !_wasKeysetADDown)
                {
                    _wasKeysetADDown = true;
                    status = KeyStatus.pressed;
                }
                else if (args.KeyStatus.WasKeyDown && !_wasKeysetQEDown)
                {
                    _wasKeysetQEDown = true;
                    status = KeyStatus.pressed;
                }
                else if (args.KeyStatus.IsKeyReleased && _wasKeysetWSDown)
                {
                    _wasKeysetWSDown = false;
                    status = KeyStatus.released;
                }
                else if (args.KeyStatus.IsKeyReleased && _wasKeysetADDown)
                {
                    _wasKeysetADDown = false;
                    status = KeyStatus.released;
                }
                else if (args.KeyStatus.IsKeyReleased && _wasKeysetQEDown)
                {
                    _wasKeysetQEDown = false;
                    status = KeyStatus.released;
                }
                else
                {
                    return;
                }

                switch (args.VirtualKey)
                {
                    case Windows.System.VirtualKey.S:
                        k = Key.S;
                        break;
                    case Windows.System.VirtualKey.A:
                        k = Key.A;
                        break;
                    case Windows.System.VirtualKey.D:
                        k = Key.D;
                        break;
                    case Windows.System.VirtualKey.E:
                        k = Key.E;
                        break;
                    case Windows.System.VirtualKey.Q:
                        k = Key.Q;
                        break;
                    case Windows.System.VirtualKey.W:
                        k = Key.W;
                        break;
                    default:
                        return;

                }

                string msg = GenerateMsg(status, k);
                bool res = await Send(msg);
                if (!res)
                {
                    robotAvailable = false;
                }
            }
        }

        private string GenerateMsg(KeyStatus status, Key k)
        {
            string msg = "";

            switch (k)
            {
                case Key.A:
                    msg += "A";
                    break;
                case Key.S:
                    msg += "S";
                    break;
                case Key.D:
                    msg += "D";
                    break;
                case Key.W:
                    msg += "W";
                    break;
                case Key.Q:
                    msg += "Q";
                    break;
                case Key.E:
                    msg += "E";
                    break;
            }

            switch (status)
            {
                case KeyStatus.pressed:
                    msg += "P";
                    break;
                case KeyStatus.released:
                    msg += "R";
                    break;
            }

            msg += PrepareValue(_gui.DriveSpeed);
            msg += PrepareValue(_gui.CurveInnerMotorSpeed);
            msg += PrepareValue(_gui.HorizontalTurnSpeed);
            msg += PrepareValue(_gui.VerticalTurnSpeed);

            Debug.WriteLine(msg);
            return msg;
        }

        private string PrepareValue(double value)
        {
            return value.ToString().PadLeft(3, '0');
        }

        private async Task<bool> Send(string msg)
        {
            Uri getUri = new Uri(RemoteUri, "function/?id=" + msg);
            HttpClient client = new HttpClient();
            string response;
            try
            {
                response = await client.GetStringAsync(getUri.AbsoluteUri);
            }
            catch (Exception)
            {
                return false;
            }
            if (response == "OK")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
