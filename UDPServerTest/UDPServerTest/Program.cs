using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using PTZ;

public class UDPListener
{
    private const int listenPort = 11000;

    private static void StartListener()
    {
        bool done = false;

        //var p = PTZDevice.GetDevice("BCC950 ConferenceCam", PTZType.Relative);
        UdpClient listener = new UdpClient(listenPort);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

        try
        {
            while (!done)
            {
                Console.WriteLine("Waiting for broadcast");
                byte[] bytes = listener.Receive(ref groupEP);

                Console.WriteLine("Received broadcast from {0} :\n {1}\n",
                    groupEP.ToString(),
                    Encoding.ASCII.GetString(bytes, 0, bytes.Length));
                switch (Encoding.ASCII.GetString(bytes, 0, bytes.Length))
                {
                    case "up":
                        //p.Move(0, 1);
                        Console.WriteLine("up");
                        break;
                    case "down":
                        //p.Move(0, -1);
                        Console.WriteLine("down");
                        break;
                    case "left":
                        //p.Move(-1, 0);
                        Console.WriteLine("left");
                        break;
                    case "right":
                        //p.Move(1, 0);
                        Console.WriteLine("right");
                        break;
                    case "zoomin":
                        //p.Zoom(1);
                        Console.WriteLine("zoomin");
                        break;
                    case "zoomout":
                        //p.Zoom(-1);
                        Console.WriteLine("zoomout");
                        break;
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        finally
        {
            listener.Close();
        }
    }

    public static int Main()
    {
        StartListener();

        return 0;
    }
}