using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace FlightSimulatorApp.Controls
{
    public partial class Joystick : UserControl
    {
        double mouseX, mouseY;
        List<Ellipse> knobs;
        bool mousePressed;
        public Joystick()
        {
            InitializeComponent();
            mousePressed = false;
            knobs = new List<Ellipse> { KnobMask, KnobBase5, KnobBase4, KnobBase3, KnobBase2, KnobBase1 };
        }


        private void centerKnob_Completed(object sender, EventArgs e)
        {

        }

        private void KnobMask_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void KnobMask_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mousePressed = false;
        }

        private void KnobMask_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!mousePressed)
            {
                mouseX = e.GetPosition(sender as Ellipse).X;
                mouseY = e.GetPosition(sender as Ellipse).Y;
                Console.WriteLine(mouseX);
            }
        }
    }
}
