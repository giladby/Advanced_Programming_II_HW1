using System;
using System.Windows;
using System.Windows.Controls;

namespace FlightSimulatorApp
{
    public class SimulatorDashboardViewModel : UserControl
    {
        private ISimulatorModel model;

        public SimulatorDashboardViewModel(ISimulatorModel m)
        {
            model = m;
            VM_status = "";
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                SetProperty("VM_" + e.GetPropName());
            };
        }

        public string VM_HeadingDeg
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

        // Using a DependencyProperty for VM_HeadingDeg in order to bind with the XAML.
        public static readonly DependencyProperty HeadingDegProperty =
            DependencyProperty.Register("VM_HeadingDeg", typeof(string), typeof(SimulatorDashboardViewModel));

        public string VM_VerticalSpeed
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

        // Using a DependencyProperty for VM_VerticalSpeed in order to bind with the XAML.
        public static readonly DependencyProperty VerticalSpeedProperty =
            DependencyProperty.Register("VM_VerticalSpeed", typeof(string), typeof(SimulatorDashboardViewModel));

        public string VM_GroundSpeedKt
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

        // Using a DependencyProperty for VM_GroundSpeedKt in order to bind with the XAML.
        public static readonly DependencyProperty GroundSpeedKtProperty =
            DependencyProperty.Register("VM_GroundSpeedKt", typeof(string), typeof(SimulatorDashboardViewModel));

        public string VM_IndicatedSpeedKt
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

        // Using a DependencyProperty for VM_IndicatedSpeedKt in order to bind with the XAML.
        public static readonly DependencyProperty IndicatedSpeedKtProperty =
            DependencyProperty.Register("VM_IndicatedSpeedKt", typeof(string), typeof(SimulatorDashboardViewModel));

        public string VM_GpsIndicatedAltitudeFt
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

        // Using a DependencyProperty for VM_GpsIndicatedAltitudeFt in order to bind with the XAML.
        public static readonly DependencyProperty GpsIndicatedAltitudeFtProperty =
            DependencyProperty.Register("VM_GpsIndicatedAltitudeFt", typeof(string), typeof(SimulatorDashboardViewModel));

        public string VM_RollDeg
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

        // Using a DependencyProperty for VM_RollDeg in order to bind with the XAML.
        public static readonly DependencyProperty RollDegProperty =
            DependencyProperty.Register("VM_RollDeg", typeof(string), typeof(SimulatorDashboardViewModel));

        public string VM_PitchDeg
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

        // Using a DependencyProperty for VM_PitchDeg in order to bind with the XAML.
        public static readonly DependencyProperty PitchDegProperty =
            DependencyProperty.Register("VM_PitchDeg", typeof(string), typeof(SimulatorDashboardViewModel));

        public string VM_AltimeterIndicatedAltitudeFt
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

        // Using a DependencyProperty for VM_AltimeterIndicatedAltitudeFt in order to bind with the XAML.
        public static readonly DependencyProperty AltimeterIndicatedAltitudeFtProperty =
            DependencyProperty.Register("VM_AltimeterIndicatedAltitudeFt", typeof(string), typeof(SimulatorDashboardViewModel));

        private string VM_status;

        public string VM_Status
        {
            get { return (string)GetValue(StatusProperty); }
            set
            {
                if (VM_status != value)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        SetValue(StatusProperty, value);
                    }));
                    VM_status = value;
                }
            }
        }

        // Using a DependencyProperty for VM_Status in order to bind with the XAML.
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("VM_Status", typeof(string), typeof(SimulatorDashboardViewModel));

        private void SetProperty(string propName)
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
                case "VM_Status":
                    VM_Status = model.Status;
                    break;
            }
        }
    }
}
