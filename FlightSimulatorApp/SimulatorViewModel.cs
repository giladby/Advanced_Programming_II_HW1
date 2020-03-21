using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FlightSimulatorApp
{
    class SimulatorViewModel : UserControl
    {
        private ISimulatorModel model;
        public SimulatorViewModel(ISimulatorModel m)
        {
            this.model = m;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                setProperty("VM_" + e.getPropName());
            };
            
        }

        public void connect(string ip, int port)
        {
            model.connect(ip, port);
        }

        /*
        
        double VM_headingDeg;
        public double VM_HeadingDeg
        {
            get
            {
                return VM_headingDeg;
            }
            set
            {
                if (VM_headingDeg != value)
                {
                    VM_headingDeg = value;
                }
            }
        }
        double VM_verticalSpeed;
        public double VM_VerticalSpeed
        {
            get
            {
                return VM_verticalSpeed;
            }
            set
            {
                if (VM_verticalSpeed != value)
                {
                    VM_verticalSpeed = value;
                }
            }
        }
        double VM_groundSpeedKt;
        public double VM_GroundSpeedKt
        {
            get
            {
                return VM_groundSpeedKt;
            }
            set
            {
                if (VM_groundSpeedKt != value)
                {
                    VM_groundSpeedKt = value;
                }
            }
        }
        double VM_indicatedSpeedKt;
        public double VM_IndicatedSpeedKt
        {
            get
            {
                return VM_indicatedSpeedKt;
            }
            set
            {
                if (VM_indicatedSpeedKt != value)
                {
                    VM_indicatedSpeedKt = value;
                }
            }
        }
        double VM_gpsIndicatedAltitudeFt;
        public double VM_GpsIndicatedAltitudeFt
        {
            get
            {
                return VM_gpsIndicatedAltitudeFt;
            }
            set
            {
                if (VM_gpsIndicatedAltitudeFt != value)
                {
                    VM_gpsIndicatedAltitudeFt = value;
                }
            }
        }
        double VM_rollDeg;
        public double VM_RollDeg
        {
            get
            {
                return VM_rollDeg;
            }
            set
            {
                if (VM_rollDeg != value)
                {
                    Console.WriteLine("RollDeg = " + value);
                    VM_rollDeg = value;
                }
            }
        }
        double VM_pitchDeg;
        public double VM_PitchDeg
        {
            get
            {
                return VM_pitchDeg;
            }
            set
            {
                if (VM_pitchDeg != value)
                {
                    VM_pitchDeg = value;
                }
            }
        }
        double VM_altimeterIndicatedAltitudeFt;
        public double VM_AltimeterIndicatedAltitudeFt
        {
            get
            {
                return VM_altimeterIndicatedAltitudeFt;
            }
            set
            {
                if (VM_altimeterIndicatedAltitudeFt != value)
                {
                    VM_altimeterIndicatedAltitudeFt = value;
                }
            }
        }

        */



        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public double VM_HeadingDeg
        {
            get { return (double)GetValue(HeadingDegProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(HeadingDegProperty, value);
                }));
            }
        }
        public static readonly DependencyProperty HeadingDegProperty =
            DependencyProperty.Register("VM_HeadingDeg", typeof(double), typeof(SimulatorViewModel));

        public double VM_VerticalSpeed
        {
            get { return (double)GetValue(VerticalSpeedProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(VerticalSpeedProperty, value);
                }));
            }
        }
        public static readonly DependencyProperty VerticalSpeedProperty =
            DependencyProperty.Register("VM_VerticalSpeed", typeof(double), typeof(SimulatorViewModel));

        public double VM_GroundSpeedKt
        {
            get { return (double)GetValue(GroundSpeedKtProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(GroundSpeedKtProperty, value);
                }));
            }
        }
        public static readonly DependencyProperty GroundSpeedKtProperty =
            DependencyProperty.Register("VM_GroundSpeedKt", typeof(double), typeof(SimulatorViewModel));

        public double VM_IndicatedSpeedKt
        {
            get { return (double)GetValue(IndicatedSpeedKtProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(IndicatedSpeedKtProperty, value);
                }));
            }
        }
        public static readonly DependencyProperty IndicatedSpeedKtProperty =
            DependencyProperty.Register("VM_IndicatedSpeedKt", typeof(double), typeof(SimulatorViewModel));

        public double VM_GpsIndicatedAltitudeFt
        {
            get { return (double)GetValue(GpsIndicatedAltitudeFtProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(GpsIndicatedAltitudeFtProperty, value);
                }));
            }
        }
        public static readonly DependencyProperty GpsIndicatedAltitudeFtProperty =
            DependencyProperty.Register("VM_GpsIndicatedAltitudeFt", typeof(double), typeof(SimulatorViewModel));

        public double VM_RollDeg
        {
            get { return (double)GetValue(RollDegProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(RollDegProperty, value);
                }));
            }
        }
        public static readonly DependencyProperty RollDegProperty =
            DependencyProperty.Register("VM_RollDeg", typeof(double), typeof(SimulatorViewModel));

        public double VM_PitchDeg
        {
            get { return (double)GetValue(PitchDegProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(PitchDegProperty, value);
                }));
            }
        }
        public static readonly DependencyProperty PitchDegProperty =
            DependencyProperty.Register("VM_PitchDeg", typeof(double), typeof(SimulatorViewModel));

        public double VM_AltimeterIndicatedAltitudeFt
        {
            get { return (double)GetValue(AltimeterIndicatedAltitudeFtProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(AltimeterIndicatedAltitudeFtProperty, value);
                }));
            }
        }
        public static readonly DependencyProperty AltimeterIndicatedAltitudeFtProperty =
            DependencyProperty.Register("VM_AltimeterIndicatedAltitudeFt", typeof(double), typeof(SimulatorViewModel));

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
            DependencyProperty.Register("VM_Latitude", typeof(double), typeof(SimulatorViewModel));

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
            DependencyProperty.Register("VM_Longitude", typeof(double), typeof(SimulatorViewModel));

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        public string VM_Status
        {
            get { return (string)GetValue(StatusProperty); }
            set 
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(StatusProperty, value);
                })); 
            }
        }

        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("VM_Status", typeof(string), typeof(SimulatorViewModel));


        private void setProperty(string propName)
        {
            switch(propName)
            {
                case "VM_HeadingDeg":
                    VM_HeadingDeg = model.HeadingDeg;
                    break;
                case "VM_VerticalSpeed":
                    VM_VerticalSpeed = model.VerticalSpeed;
                    break;
                case "VM_GroundSpeedKt":
                    VM_GroundSpeedKt = model.GroundSpeedKt;
                    break;
                case "VM_IndicatedSpeedKt":
                    VM_IndicatedSpeedKt = model.IndicatedSpeedKt;
                    break;
                case "VM_GpsIndicatedAltitudeFt":
                    VM_GpsIndicatedAltitudeFt = model.GpsIndicatedAltitudeFt;
                    break;
                case "VM_RollDeg":
                    VM_RollDeg = model.RollDeg;
                    break;
                case "VM_PitchDeg":
                    VM_PitchDeg = model.PitchDeg;
                    break;
                case "VM_AltimeterIndicatedAltitudeFt":
                    VM_AltimeterIndicatedAltitudeFt = model.AltimeterIndicatedAltitudeFt;
                    break;
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
        double VM_elevator;
        public double VM_Elevator
        {
            get
            {
                return VM_elevator;
            }
            set
            {
                if (VM_elevator != value)
                {
                    VM_elevator = value;
                    model.addSetString("/controls/flight/elevator", VM_elevator);
                }
            }
        }
        double VM_rudder;
        public double VM_Rudder
        {
            get
            {
                return VM_rudder;
            }
            set
            {
                if (VM_rudder != value)
                {
                    VM_rudder = value;
                    model.addSetString("/controls/flight/rudder", VM_rudder);
                }
            }
        }
        double VM_aileron;
        public double VM_Aileron
        {
            get
            {
                return VM_aileron;
            }
            set
            {
                if (VM_aileron != value)
                {
                    VM_aileron = value;
                    model.addSetString("/controls/flight/aileron", VM_aileron);
                }
            }
        }
        double VM_throttle;
        public double VM_Throttle
        {
            get
            {
                return VM_throttle;
            }
            set
            {
                if (VM_throttle != value)
                {
                    VM_throttle = value;
                    model.addSetString("/controls/engines/engine/throttle", VM_throttle);
                }
            }
        }
    }
}
