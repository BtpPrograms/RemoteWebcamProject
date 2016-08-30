using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GUIClientWPF
{
    class UdpClient
    {
        Socket s;
        IPAddress broadcast;
        IPEndPoint ep;

        public UdpClient(String ip)
        {
            s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            ProtocolType.Udp);

            broadcast = IPAddress.Parse(ip);

            ep = new IPEndPoint(broadcast, 11000);
        }

        public void setIP(String ip)
        {
            broadcast = IPAddress.Parse(ip);
            ep = new IPEndPoint(broadcast, 11000);
        }

        public void sendMessage(String message)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            s.SendTo(bytes, ep);
        }


    }
}
