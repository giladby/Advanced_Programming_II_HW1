using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace FlightSimulatorApp
{
    class MySimulatorClient : ISimulatorClient
    {
        Socket mySocket;

        public string connect(string ip, int port)
        {
            mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mySocket.ReceiveTimeout = 10000;
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

        public string recieve()
        {
            byte[] rcvBuffer = new byte[1024];
            try
            {
                int numberOfBytes = mySocket.Receive(rcvBuffer);
                return Encoding.ASCII.GetString(rcvBuffer, 0, numberOfBytes);
            } catch
            {
                if(!mySocket.Connected)
                {
                    return MyStatus.disconnectedStatus;
                }
                return MyStatus.rcvErrorStatus;
            }
        }

        public string send(string data)
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
                    return MyStatus.disconnectedStatus;
                }
                return MyStatus.sendErrorStatus;
            }
        }
    }
}
