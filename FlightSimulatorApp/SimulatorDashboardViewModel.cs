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

        // Using a DependencyProperty for VM_HeadingDeg in order to bind with the XAML.
        public static readonly DependencyProperty HeadingDegProperty =
            DependencyProperty.Register("VM_HeadingDeg", typeof(double), typeof(SimulatorDashboardViewModel));

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

        // Using a DependencyProperty for VM_VerticalSpeed in order to bind with the XAML.
        public static readonly DependencyProperty VerticalSpeedProperty =
            DependencyProperty.Register("VM_VerticalSpeed", typeof(double), typeof(SimulatorDashboardViewModel));

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

        // Using a DependencyProperty for VM_GroundSpeedKt in order to bind with the XAML.
        public static readonly DependencyProperty GroundSpeedKtProperty =
            DependencyProperty.Register("VM_GroundSpeedKt", typeof(double), typeof(SimulatorDashboardViewModel));

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

        // Using a DependencyProperty for VM_IndicatedSpeedKt in order to bind with the XAML.
        public static readonly DependencyProperty IndicatedSpeedKtProperty =
            DependencyProperty.Register("VM_IndicatedSpeedKt", typeof(double), typeof(SimulatorDashboardViewModel));

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

        // Using a DependencyProperty for VM_GpsIndicatedAltitudeFt in order to bind with the XAML.
        public static readonly DependencyProperty GpsIndicatedAltitudeFtProperty =
            DependencyProperty.Register("VM_GpsIndicatedAltitudeFt", typeof(double), typeof(SimulatorDashboardViewModel));

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

        // Using a DependencyProperty for VM_RollDeg in order to bind with the XAML.
        public static readonly DependencyProperty RollDegProperty =
            DependencyProperty.Register("VM_RollDeg", typeof(double), typeof(SimulatorDashboardViewModel));

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

        // Using a DependencyProperty for VM_PitchDeg in order to bind with the XAML.
        public static readonly DependencyProperty PitchDegProperty =
            DependencyProperty.Register("VM_PitchDeg", typeof(double), typeof(SimulatorDashboardViewModel));

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

        // Using a DependencyProperty for VM_AltimeterIndicatedAltitudeFt in order to bind with the XAML.
        public static readonly DependencyProperty AltimeterIndicatedAltitudeFtProperty =
            DependencyProperty.Register("VM_AltimeterIndicatedAltitudeFt", typeof(double), typeof(SimulatorDashboardViewModel));

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
