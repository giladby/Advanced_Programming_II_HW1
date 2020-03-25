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
using System.Configuration;
using Microsoft.Maps.MapControl.WPF;


namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        SimulatorControlsViewModel controlsVM;
        SimulatorMapViewModel mapVM;
        SimulatorDashboardViewModel dashboardVM;

        public MainWindow()
        {
            ISimulatorClient client = new MySimulatorClient();
            ISimulatorModel model = new MySimulatorModel(client);
            controlsVM = new SimulatorControlsViewModel(model);
            mapVM = new SimulatorMapViewModel(model);
            dashboardVM = new SimulatorDashboardViewModel(model);
            DataContext = new
            {
                controlsVM,
                mapVM,
                dashboardVM
            };
            InitializeComponent();

            myMap.Focus();

            ipBox.Text = ConfigurationManager.AppSettings.Get("ip");
            portBox.Text = ConfigurationManager.AppSettings.Get("port");

            disconnectedMode();
            statusBox.Text = MyStatus.notConnectedStatus;
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
            airplane.Visibility = Visibility.Collapsed;
            planeViewText.Visibility = Visibility.Visible;
            planeBoxView.Visibility = Visibility.Visible;
        }

        private void connectedMode()
        {
            airplane.Visibility = Visibility.Visible;
            connectButton.IsEnabled = false;
            ipBox.IsEnabled = false;
            portBox.IsEnabled = false;
            throttleSlider.IsEnabled = true;
            aileronSlider.IsEnabled = true;
            myJoystick.IsEnabled = true;
            controlsView.Visibility = Visibility.Collapsed;
            planeBoxView.Visibility = Visibility.Collapsed;
            planeViewText.Visibility = Visibility.Collapsed;
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
            controlsVM.connect(ipBox.Text, int.Parse(portBox.Text));
        }

        private void statusBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(statusBox.Text == MyStatus.connectedStatus)
            {
                connectedMode();
                return;
            }
            if(statusBox.Text == MyStatus.disconnectedStatus || statusBox.Text == MyStatus.notConnectedStatus)
            {
                disconnectedMode();
                return;
            }
            if (statusBox.Text == MyStatus.startLatitudeErrorStatus || statusBox.Text == MyStatus.startLongitudeErrorStatus)
            {
                airplane.Visibility = Visibility.Collapsed;
                return;
            }
        }

    }
}
