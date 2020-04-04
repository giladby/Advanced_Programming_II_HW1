using System;

namespace FlightSimulatorApp
{
    public class PropertyChangedEventArgs : EventArgs
    {
        string propName;

        public PropertyChangedEventArgs(string property)
        {
            propName = property;
        }

        public string GetPropName()
        {
            return propName;
        }
    }
}
