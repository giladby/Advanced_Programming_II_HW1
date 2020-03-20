﻿using System;
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
            mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            time = 10000;
            mySocket.ReceiveTimeout = time;
        }

        public string connect(string ip, int port)
        {
            try
            {
                mySocket.Connect(ip, port);
                return MainWindow.connectedStatus;
            } 
            catch
            {
                return MainWindow.connectionFailedStatus;
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
                    return MainWindow.disconnectedStatus;
                }
                return MainWindow.rcvErrorStatus;
            }
        }

        public string send(string data)
        {
            byte[] msgToSend = Encoding.ASCII.GetBytes(data + "\n");
            try
            {
                mySocket.Send(msgToSend);
                return MainWindow.okStatus;
            }
            catch
            {
                if (!mySocket.Connected)
                {
                    return MainWindow.disconnectedStatus;
                }
                Console.WriteLine("trying to send " + data + " ,connected, but fail");
                return MainWindow.sendErrorStatus;
            }
        }
    }
}
