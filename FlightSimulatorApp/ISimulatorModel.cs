using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorApp
{
    public interface ISimulatorModel : INotifyPropertyChanged
    {
        void connect(string ip, int port);
        void disconnect();
        void start();
        string Status { set; get; }

        double HeadingDeg { set; get; }
        double VerticalSpeed{ set; get; }
        double GroundSpeedKt{ set; get; }
        double IndicatedSpeedKt { set; get; }
        double GpsIndicatedAltitudeFt { set; get; }
        double RollDeg { set; get; }
        double PitchDeg { set; get; }
        double AltimeterIndicatedAltitudeFt { set; get; }
        double Latitude { set; get; }
        double Longitude { set; get; }
        double Angle { set; get; }
        Location PlaneLocation { set; get; }

        void addSetString(string name, double value);
    }

    

}
