using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorApp
{
    public interface ISimulatorModel : INotifyPropertyChanged
    {
        void Connect(string ip, int port);
        void Disconnect();
        void Start();
        void AddSetString(string name, double value);

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
    }
}
