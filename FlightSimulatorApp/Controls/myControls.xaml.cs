using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FlightSimulatorApp.Controls
{
    /// <summary>
    /// Interaction logic for myControls.xaml
    /// </summary>
    public partial class myControls : UserControl
    {
        private bool clicked;

        public myControls()
        {
            InitializeComponent();
            clicked = false;
        }

        public double ElevatorValue
        {
            get { return (double)GetValue(ElevatorValueProperty); }
            set { SetValue(ElevatorValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ElevatorValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElevatorValueProperty =
            DependencyProperty.Register("ElevatorValue", typeof(double), typeof(myControls));

        public double RudderValue
        {
            get { return (double)GetValue(RudderValueProperty); }
            set { SetValue(RudderValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RudderValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RudderValueProperty =
            DependencyProperty.Register("RudderValue", typeof(double), typeof(myControls));

        public double ThrottleValue
        {
            get { return (double)GetValue(ThrottleValueProperty); }
            set { SetValue(ThrottleValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ElevatorValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThrottleValueProperty =
            DependencyProperty.Register("ThrottleValue", typeof(double), typeof(myControls));

        public double AileronValue
        {
            get { return (double)GetValue(AileronValueProperty); }
            set { SetValue(AileronValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RudderValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AileronValueProperty =
            DependencyProperty.Register("AileronValue", typeof(double), typeof(myControls));

        public double Rudder
        {
            get
            {
                return RudderValue;
            }
            set
            {
                if (RudderValue != value)
                {
                    RudderValue = value;
                }
            }
        }

        public double Elevator
        {
            get
            {
                return ElevatorValue;
            }
            set
            {
                if (ElevatorValue != value)
                {
                    ElevatorValue = value;
                }
            }
        }

        private void throttleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double value = Math.Round(throttleSlider.Value, 5);
            throttleLabel.Content = value.ToString();
        }

        private void aileronSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double value = Math.Round(aileronSlider.Value, 5);
            aileronLabel.Content = value.ToString();
        }

        private void myJoystick_MouseMove(object sender, MouseEventArgs e)
        {
            if (clicked)
            {
                double x = Math.Round(myJoystick.X, 5);
                rudderLabel.Content = x.ToString();
                double y = Math.Round(myJoystick.Y, 5);
                elevatorLabel.Content = y.ToString();
            }
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            clicked = false;
            rudderLabel.Content = 0;
            elevatorLabel.Content = 0;
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            clicked = true;
        }

        public void ConnectedMode()
        {
            throttleSlider.IsEnabled = true;
            aileronSlider.IsEnabled = true;
            myJoystick.IsEnabled = true;
        }
        
        public void DisconnectedMode()
        {
            throttleSlider.Value = 0;
            throttleSlider.IsEnabled = false;
            aileronSlider.Value = 0;
            aileronSlider.IsEnabled = false;
            myJoystick.IsEnabled = false;
        }
    }
}
