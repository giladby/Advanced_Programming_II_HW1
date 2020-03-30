using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Configuration;
using Microsoft.Maps.MapControl.WPF;
using System.Windows.Shell;


namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool roadViewFlag;
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new
            {
                (Application.Current as App).controlsVM,
                (Application.Current as App).mapVM,
                (Application.Current as App).dashboardVM
            };
            
            myMap.Focus();

            ipBox.Text = ConfigurationManager.AppSettings.Get("ip");
            portBox.Text = ConfigurationManager.AppSettings.Get("port");

            (Application.Current as App).dashboardVM.VM_Status = MyStatus.notConnectedStatus;

            roadViewFlag = true;
        }

        private void connectedMode()
        {
            myControls.connectedMode();
            connectButton.IsEnabled = false;
            ipBox.IsEnabled = false;
            portBox.IsEnabled = false;
            controlsView.Visibility = Visibility.Collapsed;
            controlsTextBox.Visibility = Visibility.Collapsed;
            airplane.Visibility = Visibility.Visible;
            planeBoxView.Visibility = Visibility.Collapsed;
            planeViewText.Visibility = Visibility.Collapsed;
            centerButton.IsEnabled = true;
            planeColorBox.IsEnabled = true;
            planeColorBox.SelectedItem = blackColor;
        }

        private void disconnectedMode()
        {
            myControls.disconnectedMode();
            connectButton.IsEnabled = true;
            ipBox.IsEnabled = true;
            portBox.IsEnabled = true;
            controlsView.Visibility = Visibility.Visible;
            controlsTextBox.Visibility = Visibility.Visible;
            airplane.Visibility = Visibility.Collapsed;
            planeViewText.Visibility = Visibility.Visible;
            planeBoxView.Visibility = Visibility.Visible;
            centerButton.IsEnabled = false;
            planeColorBox.IsEnabled = false;
            planeColorBox.SelectedItem = null;
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).controlsVM.connect(ipBox.Text, int.Parse(portBox.Text));
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

        private void mapViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (roadViewFlag)
            {
                mapViewButton.Content = "Change To Road View";
                myMap.Mode = new AerialMode(true);
                roadViewFlag = false;
            } else
            {
                mapViewButton.Content = "Change To Aerial View";
                myMap.Mode = new RoadMode();
                roadViewFlag = true;
            }
        }

        private void centerButton_Click(object sender, RoutedEventArgs e)
        {
            myMap.ZoomLevel = 6;
            myMap.Center = MapLayer.GetPosition(airplane);
        }

        private void planeColorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (planeColorBox.SelectedItem != null)
            {
                string color = planeColorBox.SelectedItem.ToString().Split(' ').Last();
                string imagePath = "Resources/" + color + "_airplane.png";
                plane.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            }
        }
    }
}
