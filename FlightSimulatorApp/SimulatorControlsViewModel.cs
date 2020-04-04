using System.Windows.Controls;

namespace FlightSimulatorApp
{
    public class SimulatorControlsViewModel : UserControl
    {
        private ISimulatorModel model;
       
        public SimulatorControlsViewModel(ISimulatorModel m)
        {
            model = m;
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
                    model.AddSetString("/controls/flight/elevator", VM_elevator);
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
                    model.AddSetString("/controls/flight/rudder", VM_rudder);
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
                    model.AddSetString("/controls/flight/aileron", VM_aileron);
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
                    model.AddSetString("/controls/engines/current-engine/throttle", VM_throttle);
                }
            }
        }

        public void Connect(string ip, int port)
        {
            model.Connect(ip, port);
        }

        public void Disconnect()
        {
            model.Disconnect();
        }
    }
}
