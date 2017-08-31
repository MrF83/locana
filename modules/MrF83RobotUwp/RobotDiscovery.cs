using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Windows.Networking.Sockets;
using Windows.Networking;

namespace MrF83RobotUwp
{
    /// <summary>
    /// Provides a class to easy discover a robot in the network over Udp-Broadcast.
    /// </summary>
    public class RobotDiscovery
    {
        public delegate void RobotDiscoveredEventHandler(object sender, RobotDiscoveredEventArgs e);

        /// <summary>
        /// Is fired when the 
        /// <see cref="System.Uri"/> 
        /// to the robot is discovered and correctly parsed.
        /// </summary>
        public event RobotDiscoveredEventHandler RobotDiscovered;

        private DatagramSocket _listener;

        private int _port;

        /// <summary>
        /// Readonly local Port to listen for Udp-Broadcast Packets.
        /// </summary>
        public int LocalUdpPort
        {
            get { return _port; }
        }


        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="MrF83RobotUwp.RobotDiscovery"/> 
        /// class. The class holds mainly the 
        /// <see cref="Windows.Networking.Sockets.DatagramSocket"/> 
        /// which listens for the Udp-Broadcast Packets.
        /// </summary>
        /// <param name="localUdpPort">The local Port to listen for Udp-Broadcast Packets.</param>
        public RobotDiscovery(int localUdpPort)
        {
            _port = localUdpPort;
        }

        /// <summary>
        /// Starts the listening for Udp-Broadcast Packets.
        /// </summary>
        public async void GetRobotUriAsync()
        {
            _listener = new DatagramSocket();
            _listener.Control.InboundBufferSizeInBytes = 100;
            _listener.MessageReceived += Listener_MessageReceived;
            await _listener.BindServiceNameAsync(_port.ToString());
        }

        private void Listener_MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            _listener.MessageReceived -= Listener_MessageReceived;
            byte[] answer = new byte[33];
            args.GetDataReader().ReadBytes(answer);
            Uri tmpUri;
            try
            {
                string uriStr = "http://" + ExtractUri(null, answer) + "/";
                tmpUri = new Uri(uriStr);
            }
            catch (Exception)
            {
                _listener.Dispose();
                _listener = null;
                GetRobotUriAsync();
                return;
            }
            _listener.Dispose();
            _listener = null;
            RobotDiscovered?.Invoke(this, new RobotDiscoveredEventArgs(tmpUri));
        }

        private string ExtractUri(IPEndPoint endpoint, byte[] buffer)
        {
            string msg = Encoding.UTF8.GetString(buffer);
            string uriStr;
            if (msg.StartsWith("RobotIsOn") && msg.EndsWith("RobotIsOn"))
            {
                uriStr = msg.Replace("RobotIsOn", "");
                return uriStr;
            }
            else
            {
                return null;
            }
        }
    }
}
