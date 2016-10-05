using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        String ipInput = "";

        Console.WriteLine("Input the IP that you want to connect to");
        ipInput = Console.In.ReadLine();

        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            ProtocolType.Udp);

        IPAddress broadcast = IPAddress.Parse(ipInput);

        IPEndPoint ep = new IPEndPoint(broadcast, 11000);

        while (true)
        {
            ConsoleKeyInfo info = Console.ReadKey();
                
            if (info.Key == ConsoleKey.LeftArrow)
            {
                s.SendTo(Encoding.ASCII.GetBytes("left"), ep);
                Console.WriteLine("left");
            }
            else if (info.Key == ConsoleKey.RightArrow)
            {
                s.SendTo(Encoding.ASCII.GetBytes("right"), ep);
                Console.WriteLine("right");
            }
            else if (info.Key == ConsoleKey.UpArrow)
            {
                s.SendTo(Encoding.ASCII.GetBytes("up"), ep);
                Console.WriteLine("up");
            }
            else if (info.Key == ConsoleKey.DownArrow)
            {
                s.SendTo(Encoding.ASCII.GetBytes("down"), ep);
                Console.WriteLine("down");
            }
            else if (info.Key == ConsoleKey.Home)
            {
                s.SendTo(Encoding.ASCII.GetBytes("zoomin"), ep);
                Console.WriteLine("zoomin");
            }
            else if (info.Key == ConsoleKey.End)
            {
                s.SendTo(Encoding.ASCII.GetBytes("zoomout"), ep);
                Console.WriteLine("zoomout");
            }
        }
    }
}
