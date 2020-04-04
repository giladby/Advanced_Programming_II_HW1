using System;

namespace FlightSimulatorApp
{
    public interface INotifyPropertyChanged
    {
        event PropertyChangedEventHandler PropertyChanged;
    }

    public delegate void PropertyChangedEventHandler(Object sender, PropertyChangedEventArgs e);
}
