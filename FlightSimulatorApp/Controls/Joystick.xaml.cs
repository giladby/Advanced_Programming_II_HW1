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
    public partial class Joystick : UserControl
    {
        private bool mousePressed;
        private double blackRadius;
        private double mouseX;
        private double mouseY;
        public double elevatorValue
        {
           
            get
            {
                return -1 * (Knob.Margin.Top / blackRadius);
            }
            set
            {
                
            }
        }
        public double rudderValue
        {
            
            get
            {
                return Knob.Margin.Left / blackRadius;
            }
            set
            {

            }
        }
        public Joystick()
        {
            InitializeComponent();
            mousePressed = false;
            blackRadius = 50;
        }
        private void centerKnob_Completed(object sender, EventArgs e)
        {

        }

        private void setSimulator()
        {
            double rudder = Knob.Margin.Left / blackRadius;
            double elevator = -1 * (Knob.Margin.Top / blackRadius);
            Console.WriteLine($"rudder: {rudder}, elevator: {elevator}");
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
            double left = Knob.Margin.Left + x;
            double top = Knob.Margin.Top + y;
            double length = Math.Sqrt(left * left + top * top);
            if(length > blackRadius)
            {
                return;
            }
            Knob.Margin = new Thickness(left, top, Knob.Margin.Right, Knob.Margin.Bottom);
        }

        private void releaseMouse()
        {
            mousePressed = false;
            Knob.Margin = new Thickness(0, 0, 0, 0);
            setSimulator();
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
