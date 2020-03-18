using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp
{
    interface ISimulatorModel : INotifyPropertyChanged
    {
        void connect(string ip, int port);
        void disconnect();
        void start();

        double HeadingDeg { set; get; }
        double VerticalSpeed{ set; get; }
        double GroundSpeedKt{ set; get; }
        double IndicatedSpeedKt { set; get; }
        double GpsIndicatedAltitudeFt { set; get; }
        double RollDeg { set; get; }
        double PitchDeg { set; get; }
        double AltimeterIndicatedAltitudeFt { set; get; }

        void addSetString(string name, double value);
    }

    

}
