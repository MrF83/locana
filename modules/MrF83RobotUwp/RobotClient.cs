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
                if (value == false)
                {
                    Gui_RobotOn(this);
                }
            }
        }

        private Dictionary<Windows.System.VirtualKey, KeyStatus> _keyHistory = new Dictionary<Windows.System.VirtualKey, KeyStatus>();

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
            _keyHistory.Add(Windows.System.VirtualKey.A, KeyStatus.released);
            _keyHistory.Add(Windows.System.VirtualKey.S, KeyStatus.released);
            _keyHistory.Add(Windows.System.VirtualKey.D, KeyStatus.released);
            _keyHistory.Add(Windows.System.VirtualKey.W, KeyStatus.released);
            _keyHistory.Add(Windows.System.VirtualKey.Q, KeyStatus.released);
            _keyHistory.Add(Windows.System.VirtualKey.E, KeyStatus.released);

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
        public async void KeyActionHandler(object sender, KeyEventArgs args, KeyStatus status)
        {
            if (robotAvailable)
            {
                if (status == _keyHistory[args.VirtualKey])
                {
                    return;
                }
                else
                {
                    switch (_keyHistory[args.VirtualKey])
                    {
                        case KeyStatus.pressed:
                            _keyHistory[args.VirtualKey] = KeyStatus.released;
                            break;
                        case KeyStatus.released:
                            _keyHistory[args.VirtualKey] = KeyStatus.pressed;
                            break;
                    }
                }

                string msg = GenerateMsg(status, args);
                if (msg == null)
                {
                    Debug.WriteLine("Message in null");
                    return;
                }
                bool res = await Send(msg);
                if ((!res) && robotAvailable)
                {
                    robotAvailable = false;
                }
            }
        }

        /// <summary>
        /// The Async Handler used to hook on the KeyUp and KeyDown event of the parent programm.
        /// </summary>
        /// <param name="sender">A reference to the object which fired the event.</param>
        /// <param name="args">The 
        /// <see cref="Windows.UI.Core.KeyEventArgs"/> 
        /// provided by the KeyUp or KeyDown event.</param>
        public async Task KeyActionHandlerAsync(object sender, KeyEventArgs args, KeyStatus status)
        {
            await Task.Run(() => KeyActionHandler(sender, args, status));
        }

        private string GenerateMsg(KeyStatus status, KeyEventArgs args)
        {
            string msg = "";

            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.A:
                    msg += "A";
                    break;
                case Windows.System.VirtualKey.S:
                    msg += "S";
                    break;
                case Windows.System.VirtualKey.D:
                    msg += "D";
                    break;
                case Windows.System.VirtualKey.W:
                    msg += "W";
                    break;
                case Windows.System.VirtualKey.Q:
                    msg += "Q";
                    break;
                case Windows.System.VirtualKey.E:
                    msg += "E";
                    break;
                default:
                    return null;
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
            // TODO?: client.Timeout = new TimeSpan(0, 0, 0, 0, 500);
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
