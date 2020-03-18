using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp
{
    interface ISimulatorClient
    {
        void connect(string ip, int port);
        void send(string data);
        string recieve();
        void disconnect();
    }
}
