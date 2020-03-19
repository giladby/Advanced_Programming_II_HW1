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
        int time;
        public MySimulatorClient()
        {
            string y = "y1";
            int x = 5;
            mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            time = 10000;
            mySocket.ReceiveTimeout = time;
        }

        public string connect(string ip, int port)
        {
            try
            {
                mySocket.Connect(ip, port);
                return "Connected to simulator.";
            } 
            catch
            {
                return "Failed connecting to simulator.";
            }
            
            
            
            
        }

        public string recieve()
        {
            byte[] rcvBuffer = new byte[256];
            try
            {
                int numberOfBytes = mySocket.Receive(rcvBuffer);
                return Encoding.ASCII.GetString(rcvBuffer, 0, numberOfBytes);
            }
            catch
            {
                if(!mySocket.Connected)
                {
                    return "Disconnected from simulator.";
                }
                return "Error trying to recieve data from simulator.";
            }
        }

        public string send(string data)
        {
            byte[] msgToSend = Encoding.ASCII.GetBytes(data + "\n");
            try
            {
                mySocket.Send(msgToSend);
                return "OK";
            }
            catch
            {
                if (!mySocket.Connected)
                {
                    return "Disconnected from simulator.";
                }
                return "Error trying to send data from simulator.";
            }
        }
    }
}
