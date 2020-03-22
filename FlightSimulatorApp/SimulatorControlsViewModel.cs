using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FlightSimulatorApp
{
    class SimulatorControlsViewModel : UserControl
    {
        private ISimulatorModel model;
        public SimulatorControlsViewModel(ISimulatorModel m)
        {
            this.model = m;
        }

        double VM_elevator;
        public double VM_Elevator
        {
            get
            {
                return VM_elevator;
            }
            set
            {
                if (VM_elevator != value)
                {
                    VM_elevator = value;
                    model.addSetString("/controls/flight/elevator", VM_elevator);
                }
            }
        }
        double VM_rudder;
        public double VM_Rudder
        {
            get
            {
                return VM_rudder;
            }
            set
            {
                if (VM_rudder != value)
                {
                    VM_rudder = value;
                    model.addSetString("/controls/flight/rudder", VM_rudder);
                }
            }
        }
        double VM_aileron;
        public double VM_Aileron
        {
            get
            {
                return VM_aileron;
            }
            set
            {
                if (VM_aileron != value)
                {
                    VM_aileron = value;
                    model.addSetString("/controls/flight/aileron", VM_aileron);
                }
            }
        }
        double VM_throttle;
        public double VM_Throttle
        {
            get
            {
                return VM_throttle;
            }
            set
            {
                if (VM_throttle != value)
                {
                    VM_throttle = value;
                    model.addSetString("/controls/engines/engine/throttle", VM_throttle);
                }
            }
        }

        public void connect(string ip, int port)
        {
            model.connect(ip, port);
        }
    }
}
