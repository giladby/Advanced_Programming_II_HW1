using System.Text;
using System.Net.Sockets;

namespace FlightSimulatorApp
{
    class MySimulatorClient : ISimulatorClient
    {
        Socket mySocket;

        public string Connect(string ip, int port)
        {
            mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { ReceiveTimeout = 10000 };
            try
            {
                mySocket.Connect(ip, port);
                return MyStatus.connectedStatus;
            } 
            catch
            {
                return MyStatus.connectionFailedStatus;
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

        public string Recieve()
        {
            byte[] rcvBuffer = new byte[1024];
            try
            {
                int numberOfBytes = mySocket.Receive(rcvBuffer);
                return Encoding.ASCII.GetString(rcvBuffer, 0, numberOfBytes);
            } 
            catch
            {
                if (!mySocket.Connected)
                {
                    return MyStatus.simulatorDisconnectedStatus;
                }
                return MyStatus.rcvErrorStatus;
            }
        }

        public string Send(string data)
        {
            byte[] msgToSend = Encoding.ASCII.GetBytes(data);
            try
            {
                mySocket.Send(msgToSend);
                return MyStatus.okStatus;
            }
            catch
            {
                if (!mySocket.Connected)
                {
                    return MyStatus.simulatorDisconnectedStatus;
                }
                return MyStatus.sendErrorStatus;
            }
        }
    }
}
