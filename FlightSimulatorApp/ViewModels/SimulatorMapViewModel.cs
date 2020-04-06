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
                SetProperty("VM_" + e.PropertyName);
            };
        }

        private void SetProperty(string propName)
        {
            switch (propName)
            {
                case "VM_Latitude":
                    VM_Latitude = model.Latitude;
                    break;
                case "VM_Longitude":
                    VM_Longitude = model.Longitude;
                    break;
                case "VM_Angle":
                    VM_Angle = model.Angle;
                    break;
                case "VM_PlaneLocation":
                    VM_PlaneLocation = model.PlaneLocation;
                    break;
            }
        }

        public string VM_Latitude
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

        // Using a DependencyProperty for VM_Latitude in order to bind with the XAML.
        public static readonly DependencyProperty LatitudeProperty =
            DependencyProperty.Register("VM_Latitude", typeof(string), typeof(SimulatorMapViewModel));

        public string VM_Longitude
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

        // Using a DependencyProperty for VM_Longitude in order to bind with the XAML.
        public static readonly DependencyProperty LongitudeProperty =
            DependencyProperty.Register("VM_Longitude", typeof(string), typeof(SimulatorMapViewModel));

        public double VM_Angle
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

        // Using a DependencyProperty for VM_Angle in order to bind with the XAML.
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("VM_Angle", typeof(double), typeof(SimulatorMapViewModel));

        public Location VM_PlaneLocation
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

        // Using a DependencyProperty for VM_PlaneLocation in order to bind with the XAML.
        public static readonly DependencyProperty PlaneLocationProperty =
            DependencyProperty.Register("VM_PlaneLocation", typeof(Location), typeof(SimulatorMapViewModel));
    }
}
