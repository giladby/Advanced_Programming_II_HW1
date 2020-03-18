using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp
{
    class MySimulatorClient : ISimulatorClient
    {
        public void connect(string ip, int port)
        {
            throw new NotImplementedException();
        }

        public void disconnect()
        {
            throw new NotImplementedException();
        }

        public string recieve()
        {
            throw new NotImplementedException();
        }

        public void send(string data)
        {
            throw new NotImplementedException();
        }
    }
}
