using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorApp
{
    public interface ISimulatorModel : INotifyPropertyChanged
    {
        void Connect(string ip, int port);
        void Disconnect();
        void Start();
        void AddSetString(string name, string value);

        string Status { set; get; }

        string HeadingDeg { set; get; }

        string VerticalSpeed { set; get; }

        string GroundSpeedKt { set; get; }

        string IndicatedSpeedKt { set; get; }

        string GpsIndicatedAltitudeFt { set; get; }

        string RollDeg { set; get; }

        string PitchDeg { set; get; }

        string AltimeterIndicatedAltitudeFt { set; get; }

        string Latitude { set; get; }

        string Longitude { set; get; }

        double Angle { set; get; }
        
        Location PlaneLocation { set; get; }
    }
}
