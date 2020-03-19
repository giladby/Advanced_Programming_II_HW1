﻿using System;
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
        SimulatorViewModel vm;
        bool connected;
        public MainWindow()
        {
            InitializeComponent();
            vm = new SimulatorViewModel(new MySimulatorModel(new MySimulatorClient()));
            DataContext = vm;
            connected = false;
            throttleSlider.IsEnabled = false;
            aileronSlider.IsEnabled = false;
            myJoystick.IsEnabled = false;
            connectButton.IsEnabled = true;


        }

        private void throttleSlider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine(throttleSlider.Value);
        }

        private void ipBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine(ipBox.Text);
        }

        private void portBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine(portBox.Text);
        }

        private void aileronSlider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine(aileronSlider.Value);
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            if (!connected)
            {
                //connect to simulator

                connected = true;
                statusLabel.Content = "Connected to simulator";
                connectButton.IsEnabled = false;
                ipBox.IsEnabled = false;
                portBox.IsEnabled = false;
                throttleSlider.IsEnabled = true;
                aileronSlider.IsEnabled = true;
                myJoystick.IsEnabled = true;
            }
        }
    }
}
