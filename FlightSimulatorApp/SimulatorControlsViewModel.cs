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

        string VM_elevator;

        public string VM_Elevator
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

        string VM_rudder;

        public string VM_Rudder
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

        string VM_aileron;

        public string VM_Aileron
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

        string VM_throttle;

        public string VM_Throttle
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
