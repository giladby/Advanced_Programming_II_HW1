using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp
{
    public class PropertyChangedEventArgs : EventArgs
    {
        string propName;
        public PropertyChangedEventArgs(string property)
        {
            propName = property;
        }
        public string getPropName()
        {
            return propName;
        }

    }
}
