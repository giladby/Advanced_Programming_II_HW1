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


namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static public string connectedStatus = "Connected";
        static public string notConnectedStatus = "Not Connected";
        static public string tryingToConnectStatus = "Trying To Connect...";
        static public string connectionFailedStatus = "Failed Connecting To Simulator";
        static public string disconnectedStatus = "The Simulator Disconnected";
        static public string rcvErrorStatus = "Error Trying To Receive Data From Simulator";
        static public string sendErrorStatus = "Error Trying To Send Data To Simulator";
        static public string okStatus = "OK";

        SimulatorViewModel vm;
        public MainWindow()
        {
            vm = new SimulatorViewModel(new MySimulatorModel(new MySimulatorClient()));
            DataContext = vm;
            InitializeComponent();
            
            disconnectedMode();
            statusBox.Text = MainWindow.notConnectedStatus;
        }

        private void disconnectedMode()
        {
            connectButton.IsEnabled = true;
            ipBox.IsEnabled = true;
            portBox.IsEnabled = true;
            throttleSlider.IsEnabled = false;
            throttleSlider.Value = 0;
            aileronSlider.IsEnabled = false;
            aileronSlider.Value = 0;
            myJoystick.IsEnabled = false;
            controlsView.Visibility = Visibility.Visible;
        }

        private void connectedMode()
        {
            connectButton.IsEnabled = false;
            ipBox.IsEnabled = false;
            portBox.IsEnabled = false;
            throttleSlider.IsEnabled = true;
            aileronSlider.IsEnabled = true;
            myJoystick.IsEnabled = true;
            controlsView.Visibility = Visibility.Collapsed;
        }

        private void throttleSlider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //Console.WriteLine(throttleSlider.Value);
        }

        private void ipBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Console.WriteLine(ipBox.Text);
        }

        private void portBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Console.WriteLine(portBox.Text);
        }

        private void aileronSlider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //Console.WriteLine(aileronSlider.Value);
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            vm.connect(ipBox.Text, int.Parse(portBox.Text));
        }

        private void statusBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(statusBox.Text == MainWindow.connectedStatus)
            {
                connectedMode();
            }
            if(statusBox.Text == MainWindow.disconnectedStatus || statusBox.Text == MainWindow.notConnectedStatus)
            {
                disconnectedMode();
            }
        }
    }
}
