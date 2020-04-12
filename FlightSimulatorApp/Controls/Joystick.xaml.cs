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
        private UIElement el;

        public Joystick()
        {
            InitializeComponent();
            mousePressed = false;
            // Set the radius of the black inner circle.
            blackRadius = 130;
            myStoryboard = (Storyboard)Knob.FindResource("CenterKnob");
        }

        public string Y
        {
            get { return (string)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        // Using a DependencyProperty for Y in order to bind with the XAML.
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(string), typeof(Joystick));

        public string X
        {
            get { return (string)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        // Using a DependencyProperty for X in order to bind with the XAML.
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(string), typeof(Joystick));

        private void CenterKnob_Completed(object sender, EventArgs e)
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
            if (X != x.ToString())
            {
                X = x.ToString();
            }
            if (Y != y.ToString())
            {
                Y = y.ToString();
            }
        }

        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            

            
            if (!mousePressed)
            {
                el = (UIElement)sender;
                el.CaptureMouse();
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
            double length = Math.Sqrt((left * left) + (top * top));
            if (length > blackRadius)
            {
                return;
            }
            knobPosition.X += x;
            knobPosition.Y += y;
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
            if (mousePressed)
            {
                el.ReleaseMouseCapture();
                mousePressed = false;
                myStoryboard.Begin();
            }
        }
    }
}
