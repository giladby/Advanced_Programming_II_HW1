using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorApp
{
    class SimulatorMapViewModel : UserControl
    {
        private ISimulatorModel model;
        public SimulatorMapViewModel(ISimulatorModel m)
        {
            this.model = m;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                setProperty("VM_" + e.getPropName());
            };
        }

        private void setProperty(string propName)
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

        public double VM_Latitude
        {
            get { return (double)GetValue(LatitudeProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(LatitudeProperty, value);
                }));
            }
        }
        public static readonly DependencyProperty LatitudeProperty =
            DependencyProperty.Register("VM_Latitude", typeof(double), typeof(SimulatorMapViewModel));

        public double VM_Longitude
        {
            get { return (double)GetValue(LongitudeProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(LongitudeProperty, value);
                }));
            }
        }
        public static readonly DependencyProperty LongitudeProperty =
            DependencyProperty.Register("VM_Longitude", typeof(double), typeof(SimulatorMapViewModel));

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
        public static readonly DependencyProperty PlaneLocationProperty =
            DependencyProperty.Register("VM_PlaneLocation", typeof(Location), typeof(SimulatorMapViewModel));


    }
}
