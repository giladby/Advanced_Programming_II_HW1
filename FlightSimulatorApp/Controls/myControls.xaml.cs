using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            //DataContext = (Application.Current as App).controlsVM;
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





        //double aileron;
        //public double Aileron
        //{
        //    get
        //    {
        //        return aileron;
        //    }
        //    set
        //    {
        //        if (aileron != value)
        //        {
        //            aileron = value;
        //        }
        //    }
        //}
        //double throttle;
        //public double Throttle
        //{
        //    get
        //    {
        //        return throttle;
        //    }
        //    set
        //    {
        //        if (throttle != value)
        //        {
        //            throttle = value;
        //        }
        //    }
        //}




        //public double Elevator
        //{
        //    get { return (double)GetValue(ElevatorProperty); }
        //    set { ElevatorValue = value; }
        //}

        //// Using a DependencyProperty as the backing store for ElevatorValue.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ElevatorProperty =
        //    DependencyProperty.Register("Elevator", typeof(double), typeof(myControls));


        //public double Rudder
        //{
        //    get { return (double)GetValue(RudderProperty); }
        //    set { RudderValue = value; }
        //}

        //// Using a DependencyProperty as the backing store for RudderValue.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty RudderProperty =
        //    DependencyProperty.Register("Rudder", typeof(double), typeof(myControls));






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
                    //rudder = value;
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
                    //elevator = value;
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
                double x = Math.Round(myJoystick.xValue, 5);
                rudderLabel.Content = x.ToString();
                double y = Math.Round(myJoystick.yValue, 5);
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

        public void connectedMode()
        {
            throttleSlider.IsEnabled = true;
            aileronSlider.IsEnabled = true;
            myJoystick.IsEnabled = true;
        }
        
        public void disconnectedMode()
        {
            throttleSlider.Value = 0;
            throttleSlider.IsEnabled = false;
            aileronSlider.Value = 0;
            aileronSlider.IsEnabled = false;
            myJoystick.IsEnabled = false;
        }
    }
}
