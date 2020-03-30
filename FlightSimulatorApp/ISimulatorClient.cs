using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp
{
    public interface ISimulatorClient
    {
        string connect(string ip, int port);
        string send(string data);
        string recieve();
    }
}
