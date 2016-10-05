using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace InputConsole
{
    public class StateObject //Used for receiving data
    {
        public Socket workSocket = null; //Client socket
        public const int BufferSize = 256; //Size of buffer
        public byte[] buffer = new byte[BufferSize]; //Holds received data in byte form
        public StringBuilder sb = new StringBuilder(); //String from the bytes, used if data is larger than buffer
    }

    public class AsyncClient
    {
        private Socket client; //Socket for doing all of the sending
        private EndPoint serverEndpoint; //The target for the message, gets initialized later

        //Used as a signal that an event is complete
        private static ManualResetEvent connectDone; //Connection complete
        private static ManualResetEvent sendDone; //Send complete
        //private static ManualResetEvent receiveDone; //Receive complete

        public AsyncClient(String IP, int port)
        {
            connectDone = new ManualResetEvent(false); //Initializing ManualResetEvents
            sendDone = new ManualResetEvent(false);
            //receiveDone = new ManualResetEvent(false);

            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //Initialize socket for sending and receiving

            getServerEndpoint(IP, port); //Gets the endpoint for messages
        }

        private void getServerEndpoint(String IP, int port) //Gets the end point and saves it
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(IP); //Gets info about host from IP
            IPAddress ipAddress = ipHostInfo.AddressList[0]; //Pulls an IP out of that info
            serverEndpoint = new IPEndPoint(ipAddress, port); //Sets endpoint with found IP and port
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                client.EndConnect(ar); //Closes the connection

                Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString()); //Outputs that connection was made

                connectDone.Set(); //Signals connection is done
            }
            catch (Exception e) //If error
            {
                Console.WriteLine(e.ToString()); //Write error 
            }
        }

        public void Connect()
        {
            
            client.BeginConnect(serverEndpoint, new AsyncCallback(ConnectCallback), client);

            connectDone.WaitOne();
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                int bytesSent = client.EndSend(ar); //Finish sending, get amount sent
                Console.WriteLine("Sent data to server");

                sendDone.Set(); //Signals sending is done
            }
            catch(Exception e) //If error
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Send(String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data); //Converts string into bytes

            client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client); //Begins to send data
        }

        public void Disconnect()
        {
            client.Disconnect(true);
        }

        public void Release()
        {
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            AsyncClient asyncClient = new AsyncClient("192.168.1.91", 11000);
            asyncClient.Connect();
            asyncClient.Send("Right<EOF>");
            asyncClient.Send("Up<EOF>");
            /*asyncClient.Send("Left");
            asyncClient.Send("Down");
            asyncClient.Disconnect();
            Console.WriteLine("Disconnected");
            asyncClient.Connect();
            Console.WriteLine("Connected");
            asyncClient.Send("ZoomIn");
            asyncClient.Send("ZoomOut");*/

            asyncClient.Disconnect();
            asyncClient.Release();
            System.Console.In.ReadLine();
        }
    }
}
