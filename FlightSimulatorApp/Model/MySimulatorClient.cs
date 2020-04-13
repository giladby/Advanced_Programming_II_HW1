using System.Text;
using System.Net.Sockets;
using System;

namespace FlightSimulatorApp
{
    class MySimulatorClient : ISimulatorClient
    {
        private Socket mySocket;

        public string Connect(string ip, int port)
        {
            // Make a new socket with a 10 seconds timeout.
            mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { ReceiveTimeout = 500 };
            try
            {
                mySocket.Connect(ip, port);
                return MyStatus.ConnectedStatus;
            } 
            catch
            {
                return MyStatus.ConnectionFailedStatus;
            }
        }

        public void Disconnect()
        {
            try
            {
                mySocket.Shutdown(SocketShutdown.Both);
            }
            finally
            {
                mySocket.Close();
            }
        }

        public string Receive()
        {
            byte[] rcvBuffer = new byte[1024];
            // Trying to receive data.
            try
            {
                int numberOfBytes = mySocket.Receive(rcvBuffer);
                return Encoding.ASCII.GetString(rcvBuffer, 0, numberOfBytes);
            }
            catch (Exception e) 
            {
                try
                {
                    SocketException se = (SocketException)e;
                    if (se.SocketErrorCode == SocketError.TimedOut)
                    {
                        return MyStatus.TimeoutErrorStatus;
                    }
                }
                catch
                {

                }
                Console.WriteLine("pass");
                if (!mySocket.Connected)
                {
                    return MyStatus.SimulatorDisconnectedStatus;
                }
                return MyStatus.RcvErrorStatus;
            }
        }

        public string Send(string data)
        {
            byte[] msgToSend = Encoding.ASCII.GetBytes(data);
            // Trying to send data.
            try
            {
                mySocket.Send(msgToSend);
                return MyStatus.OkStatus;
            }
            catch
            {
                if (!mySocket.Connected)
                {
                    return MyStatus.SimulatorDisconnectedStatus;
                }
                return MyStatus.SendErrorStatus;
            }
        }
    }
}
