using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Maps.MapControl.WPF;
using System.ComponentModel;

namespace FlightSimulatorApp
{
    public class SimulatorMapViewModel : UserControl
    {
        private ISimulatorModel model;

        public SimulatorMapViewModel(ISimulatorModel m)
        {
            model = m;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                SetProperty("Vm" + e.PropertyName);
            };
        }

        private void SetProperty(string propName)
        {
            switch (propName)
            {
                case "VmLatitude":
                    VmLatitude = model.Latitude;
                    break;
                case "VmLongitude":
                    VmLongitude = model.Longitude;
                    break;
                case "VmAngle":
                    VmAngle = model.Angle;
                    break;
                case "VmPlaneLocation":
                    VmPlaneLocation = model.PlaneLocation;
                    break;
            }
        }

        public string VmLatitude
        {
            get { return (string)GetValue(LatitudeProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(LatitudeProperty, value);
                }));
            }
        }

        // Using a DependencyProperty for VmLatitude in order to bind with the XAML.
        public static readonly DependencyProperty LatitudeProperty =
            DependencyProperty.Register("VmLatitude", typeof(string), typeof(SimulatorMapViewModel));

        public string VmLongitude
        {
            get { return (string)GetValue(LongitudeProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(LongitudeProperty, value);
                }));
            }
        }

        // Using a DependencyProperty for VmLongitude in order to bind with the XAML.
        public static readonly DependencyProperty LongitudeProperty =
            DependencyProperty.Register("VmLongitude", typeof(string), typeof(SimulatorMapViewModel));

        public double VmAngle
        {
            get { return (double)GetValue(AngleProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(AngleProperty, value);
                }));
            }
        }

        // Using a DependencyProperty for VmAngle in order to bind with the XAML.
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("VmAngle", typeof(double), typeof(SimulatorMapViewModel));

        public Location VmPlaneLocation
        {
            get { return (Location)GetValue(PlaneLocationProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(PlaneLocationProperty, value);
                }));
            }
        }

        // Using a DependencyProperty for VmPlaneLocation in order to bind with the XAML.
        public static readonly DependencyProperty PlaneLocationProperty =
            DependencyProperty.Register("VmPlaneLocation", typeof(Location), typeof(SimulatorMapViewModel));
    }
}
