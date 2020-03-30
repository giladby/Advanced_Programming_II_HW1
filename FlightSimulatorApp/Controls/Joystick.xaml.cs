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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightSimulatorApp.Controls
{
    public partial class Joystick : UserControl
    {
        private bool mousePressed;
        private double blackRadius;
        private double mouseX;
        private double mouseY;
        private Storyboard myStoryboard;

        public double yValue
        {
            get { return (double)GetValue(yValueProperty); }
            set { SetValue(yValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ElevatorValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty yValueProperty =
            DependencyProperty.Register("yValue", typeof(double), typeof(Joystick));


        public double xValue
        {
            get { return (double)GetValue(xValueProperty); }
            set { SetValue(xValueProperty, value); }
        }

        //public AnimationTimeline CenterKnob { get; private set; }

        // Using a DependencyProperty as the backing store for RudderValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty xValueProperty =
            DependencyProperty.Register("xValue", typeof(double), typeof(Joystick));


        
        public Joystick()
        {
            InitializeComponent();
            mousePressed = false;
            blackRadius = 60;

            myStoryboard = (Storyboard)Knob.FindResource("CenterKnob");
        }
        private void centerKnob_Completed(object sender, EventArgs e)
        {
            myStoryboard.Stop();
            knobPosition.X = 0;
            knobPosition.Y = 0;
            setSimulator();
        }

        private void setSimulator()
        {
            /*
            if(RudderValue != Knob.Margin.Left / blackRadius)
            {
                RudderValue = Knob.Margin.Left / blackRadius;
            }
            
            if(ElevatorValue != -1 * (Knob.Margin.Top / blackRadius))
            {
                ElevatorValue = -1 * (Knob.Margin.Top / blackRadius);
            }
            */

            double x = knobPosition.X / blackRadius;
            double y = -1 * (knobPosition.Y / blackRadius);

            if (xValue != x)
            {
                xValue = x;
            }

            if (yValue != y)
            {
                yValue = y;
            }
        }

        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(!mousePressed)
            {
                mousePressed = true;
                mouseX = e.GetPosition(this).X;
                mouseY = e.GetPosition(this).Y;
            }
        }

        private void setKnobPosition(double x, double y)
        {
            //double left = Knob.Margin.Left + x;
            //double top = Knob.Margin.Top + y;

            double left = knobPosition.X + x;
            double top = knobPosition.Y + y;

            double length = Math.Sqrt(left * left + top * top);
            if(length > blackRadius)
            {
                return;
            }

            knobPosition.X += x;
            knobPosition.Y += y;

            //Knob.Margin = new Thickness(left, top, Knob.Margin.Right, Knob.Margin.Bottom);
        }

        private void releaseMouse()
        {
            mousePressed = false;

            myStoryboard.Begin();

            //Knob.Margin = new Thickness(0, 0, 0, 0);
            
        }

        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {

            double currentX, offsetX, offsetY, currentY;
            if(mousePressed)
            {
                setSimulator();

                currentX = e.GetPosition(this).X;
                currentY = e.GetPosition(this).Y;
                
                offsetX = currentX - mouseX;
                offsetY = currentY - mouseY;

                setKnobPosition(offsetX, offsetY);

                mouseX = currentX;
                mouseY = currentY;
            }
        }

        private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            releaseMouse();
        }

        private void Knob_MouseLeave(object sender, MouseEventArgs e)
        {
            if (mousePressed)
            {
                releaseMouse();
            }
        }
    }
}
