using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

                case "VM_Status":
                    VM_Status = model.Status;
                    break;
            }
        }

        public string VM_Status
        {
            get { return (string)GetValue(StatusProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Console.WriteLine("set status");
                    SetValue(StatusProperty, value);
                }));
            }
        }

        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("VM_Status", typeof(string), typeof(SimulatorDashboardViewModel));


        public double VM_Latitude
        {
            get { return (double)GetValue(LatitudeProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Console.WriteLine("set latitude");
                    SetValue(LatitudeProperty, value);
                }));
            }
        }
        public static readonly DependencyProperty LatitudeProperty =
            DependencyProperty.Register("VM_Latitude", typeof(double), typeof(SimulatorDashboardViewModel));

        public double VM_Longitude
        {
            get { return (double)GetValue(LongitudeProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Console.WriteLine("set longitude");
                    SetValue(LongitudeProperty, value);
                }));
            }
        }
        public static readonly DependencyProperty LongitudeProperty =
            DependencyProperty.Register("VM_Longitude", typeof(double), typeof(SimulatorDashboardViewModel));


    }
}
