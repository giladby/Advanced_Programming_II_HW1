using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace FlightSimulatorApp.Controls
{
    /// <summary>
    /// Interaction logic for Joystick.xaml
    /// </summary>
    public partial class Joystick : UserControl
    {
        private bool mousePressed;
        private double blackRadius;
        private double mouseX;
        private double mouseY;
        private Storyboard myStoryboard;

        public Joystick()
        {
            InitializeComponent();
            mousePressed = false;
            // Set the radius of the black inner circle.
            blackRadius = 60;
            myStoryboard = (Storyboard)Knob.FindResource("CenterKnob");
        }

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        // Using a DependencyProperty for Y in order to bind with the XAML.
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(Joystick));

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        // Using a DependencyProperty for X in order to bind with the XAML.
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(Joystick));

        private void centerKnob_Completed(object sender, EventArgs e)
        {
            myStoryboard.Stop();
            knobPosition.X = 0;
            knobPosition.Y = 0;
            SetValues();
        }

        // Set the X and Y values according to the Knob position.
        private void SetValues()
        {
            double x = knobPosition.X / blackRadius;
            double y = -1 * (knobPosition.Y / blackRadius);
            if (X != x)
            {
                X = x;
            }
            if (Y != y)
            {
                Y = y;
            }
        }

        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!mousePressed)
            {
                mousePressed = true;
                mouseX = e.GetPosition(this).X;
                mouseY = e.GetPosition(this).Y;
            }
        }

        // Move the Knob inside the inner circle according to the given x and y.
        private void SetKnobPosition(double x, double y)
        {
            double left = knobPosition.X + x;
            double top = knobPosition.Y + y;
            double length = Math.Sqrt(left * left + top * top);
            if (length > blackRadius)
            {
                return;
            }
            knobPosition.X += x;
            knobPosition.Y += y;
        }

        private void ReleaseMouse()
        {
            mousePressed = false;
            myStoryboard.Begin();
        }

        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            double currentX, offsetX, offsetY, currentY;
            if (mousePressed)
            {
                SetValues();
                currentX = e.GetPosition(this).X;
                currentY = e.GetPosition(this).Y;
                offsetX = currentX - mouseX;
                offsetY = currentY - mouseY;
                SetKnobPosition(offsetX, offsetY);
                mouseX = currentX;
                mouseY = currentY;
            }
        }

        private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouse();
        }

        private void Knob_MouseLeave(object sender, MouseEventArgs e)
        {
            if (mousePressed)
            {
                ReleaseMouse();
            }
        }
    }
}
