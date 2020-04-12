using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace FlightSimulatorApp
{
    public class SimulatorDashboardViewModel : UserControl
    {
        private ISimulatorModel model;

        public SimulatorDashboardViewModel(ISimulatorModel m)
        {
            model = m;
            vmStatus = "";
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                SetProperty("Vm" + e.PropertyName);
            };
        }

        public string VmHeadingDeg
        {
            get { return (string)GetValue(HeadingDegProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(HeadingDegProperty, value);
                }));
            }
        }

        // Using a DependencyProperty for VmHeadingDeg in order to bind with the XAML.
        public static readonly DependencyProperty HeadingDegProperty =
            DependencyProperty.Register("VmHeadingDeg", typeof(string), typeof(SimulatorDashboardViewModel));

        public string VmVerticalSpeed
        {
            get { return (string)GetValue(VerticalSpeedProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(VerticalSpeedProperty, value);
                }));
            }
        }

        // Using a DependencyProperty for VmVerticalSpeed in order to bind with the XAML.
        public static readonly DependencyProperty VerticalSpeedProperty =
            DependencyProperty.Register("VmVerticalSpeed", typeof(string), typeof(SimulatorDashboardViewModel));

        public string VmGroundSpeedKt
        {
            get { return (string)GetValue(GroundSpeedKtProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(GroundSpeedKtProperty, value);
                }));
            }
        }

        // Using a DependencyProperty for VmGroundSpeedKt in order to bind with the XAML.
        public static readonly DependencyProperty GroundSpeedKtProperty =
            DependencyProperty.Register("VmGroundSpeedKt", typeof(string), typeof(SimulatorDashboardViewModel));

        public string VmIndicatedSpeedKt
        {
            get { return (string)GetValue(IndicatedSpeedKtProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(IndicatedSpeedKtProperty, value);
                }));
            }
        }

        // Using a DependencyProperty for VmIndicatedSpeedKt in order to bind with the XAML.
        public static readonly DependencyProperty IndicatedSpeedKtProperty =
            DependencyProperty.Register("VmIndicatedSpeedKt", typeof(string), typeof(SimulatorDashboardViewModel));

        public string VmGpsIndicatedAltitudeFt
        {
            get { return (string)GetValue(GpsIndicatedAltitudeFtProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(GpsIndicatedAltitudeFtProperty, value);
                }));
            }
        }

        // Using a DependencyProperty for VmGpsIndicatedAltitudeFt in order to bind with the XAML.
        public static readonly DependencyProperty GpsIndicatedAltitudeFtProperty =
            DependencyProperty.Register("VmGpsIndicatedAltitudeFt", typeof(string), typeof(SimulatorDashboardViewModel));

        public string VmRollDeg
        {
            get { return (string)GetValue(RollDegProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(RollDegProperty, value);
                }));
            }
        }

        // Using a DependencyProperty for VmRollDeg in order to bind with the XAML.
        public static readonly DependencyProperty RollDegProperty =
            DependencyProperty.Register("VmRollDeg", typeof(string), typeof(SimulatorDashboardViewModel));

        public string VmPitchDeg
        {
            get { return (string)GetValue(PitchDegProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(PitchDegProperty, value);
                }));
            }
        }

        // Using a DependencyProperty for VmPitchDeg in order to bind with the XAML.
        public static readonly DependencyProperty PitchDegProperty =
            DependencyProperty.Register("VmPitchDeg", typeof(string), typeof(SimulatorDashboardViewModel));

        public string VmAltimeterIndicatedAltitudeFt
        {
            get { return (string)GetValue(AltimeterIndicatedAltitudeFtProperty); }
            set
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SetValue(AltimeterIndicatedAltitudeFtProperty, value);
                }));
            }
        }

        // Using a DependencyProperty for VmAltimeterIndicatedAltitudeFt in order to bind with the XAML.
        public static readonly DependencyProperty AltimeterIndicatedAltitudeFtProperty =
            DependencyProperty.Register("VmAltimeterIndicatedAltitudeFt", typeof(string), typeof(SimulatorDashboardViewModel));

        private string vmStatus;

        public string VmStatus
        {
            get { return (string)GetValue(StatusProperty); }
            set
            {
                if (vmStatus != value)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        SetValue(StatusProperty, value);
                    }));
                    vmStatus = value;
                }
            }
        }

        // Using a DependencyProperty for VmStatus in order to bind with the XAML.
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("VmStatus", typeof(string), typeof(SimulatorDashboardViewModel));

        private void SetProperty(string propName)
        {
            switch(propName)
            {
                case "VmHeadingDeg":
                    VmHeadingDeg = model.HeadingDeg;
                    break;
                case "VmVerticalSpeed":
                    VmVerticalSpeed = model.VerticalSpeed;
                    break;
                case "VmGroundSpeedKt":
                    VmGroundSpeedKt = model.GroundSpeedKt;
                    break;
                case "VmIndicatedSpeedKt":
                    VmIndicatedSpeedKt = model.IndicatedSpeedKt;
                    break;
                case "VmGpsIndicatedAltitudeFt":
                    VmGpsIndicatedAltitudeFt = model.GpsIndicatedAltitudeFt;
                    break;
                case "VmRollDeg":
                    VmRollDeg = model.RollDeg;
                    break;
                case "VmPitchDeg":
                    VmPitchDeg = model.PitchDeg;
                    break;
                case "VmAltimeterIndicatedAltitudeFt":
                    VmAltimeterIndicatedAltitudeFt = model.AltimeterIndicatedAltitudeFt;
                    break;
                case "VmStatus":
                    VmStatus = model.Status;
                    break;
            }
        }
    }
}
