using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChairReceiver
{
    public class StateObject
    {
        public Socket workSocket = null; //Socket for data
        public const int BufferSize = 1024; //Size of incoming data buffer
        public byte[] buffer = new byte[BufferSize]; //Holds incoming data
        public StringBuilder sb = new StringBuilder(); //Builds string from bytes
    }

    public class AsyncReceiver
    {
        private ManualResetEvent allDone; //Signals when done getting data
        private Socket receiver; //Socket for incoming info
        private IPEndPoint localEndPoint;

        public AsyncReceiver(int port)
        {
            allDone = new ManualResetEvent(false); //Initializes reset event

            receiver = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //Initializes receiver

            getLocalEndpoint(port); //Gets this server's info for receiver
        }

        private void getLocalEndpoint(int port) {
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            //IPAddress ipAddress = IPAddress.Loopback; //Gets this PC's IP
            localEndPoint = new IPEndPoint(ipAddress, port);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set(); //Signals that connection was made

            Socket handler = receiver.EndAccept(ar); //Handles incoming data

            StateObject state = new StateObject(); //Initializes state object
            state.workSocket = handler; //Sets work socket to handler
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty; //Holds received data

            StateObject state = (StateObject)ar.AsyncState; //Gets state object
            Socket handler = state.workSocket; //Gets socket from state object

            int bytesRead = handler.EndReceive(ar); //Ends receive, gets amount of data read

            if (bytesRead > 0)
            {
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead)); //Append any data already in buffer to stringbuilder

                content = state.sb.ToString(); //Put data in string

                if (content.IndexOf("<EOF>") > -1) //Check if the end of file tag has been received
                {
                    Console.WriteLine(content); //If it has been received then print the complete data
                }
                else
                {
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                    //Recursively goes back through to get more data, previous data retained through state object
                }
            }
        }
        public void Listen()
        {
            try
            {
                receiver.Bind(localEndPoint);
                receiver.Listen(100);

                while (true)
                {
                    allDone.Reset(); //Reset signal to nonsignal state

                    Console.WriteLine("Waiting for connection...");
                    receiver.BeginAccept(new AsyncCallback(AcceptCallback), receiver);

                    allDone.WaitOne(); //Wait until connection is made
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            AsyncReceiver asyncReceiver = new AsyncReceiver(11000);
            asyncReceiver.Listen();
            Console.ReadLine();
        }
    }
}
