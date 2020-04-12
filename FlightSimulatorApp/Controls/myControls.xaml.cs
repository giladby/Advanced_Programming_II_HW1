using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FlightSimulatorApp.Controls
{
    /// <summary>
    /// Interaction logic for myControls.xaml
    /// </summary>
    public partial class MyControls : UserControl
    {
        private bool clicked;

        public MyControls()
        {
            InitializeComponent();
            clicked = false;
        }

        private void ThrottleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double value = Math.Round(throttleSlider.Value, 5);
            throttleLabel.Content = value.ToString();
        }

        private void AileronSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double value = Math.Round(aileronSlider.Value, 5);
            aileronLabel.Content = value.ToString();
        }

        private void MyJoystick_MouseMove(object sender, MouseEventArgs e)
        {
            if (clicked)
            {
                double x = Math.Round(Double.Parse(myJoystick.X), 5);
                rudderLabel.Content = x.ToString();
                double y = Math.Round(Double.Parse(myJoystick.Y), 5);
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
            // Unlock the controls.
            throttleSlider.IsEnabled = true;
            aileronSlider.IsEnabled = true;
            myJoystick.IsEnabled = true;
        }
        
        public void DisconnectedMode()
        {
            // Set the throttle and aileron values to 0 as default, and lock the controls.
            throttleSlider.Value = 0;
            throttleSlider.IsEnabled = false;
            aileronSlider.Value = 0;
            aileronSlider.IsEnabled = false;
            myJoystick.IsEnabled = false;
        }
    }
}
