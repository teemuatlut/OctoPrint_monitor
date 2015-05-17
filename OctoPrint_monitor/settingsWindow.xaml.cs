using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.Windows.Shell;

namespace OctoPrint_monitor
{

    public partial class settingsWindow : Window
    {
        public settingsWindow()
        {
            InitializeComponent();
            APIBox.Text = App.settings.API_key;
            IPBox.Text = App.settings.OctoPrintIP;
            UpdateBox.Text = App.settings.updateInterval.ToString();
            barCheck.IsChecked = (App.settings.visibleProgressbar == TaskbarItemProgressState.Normal);
        }

        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                App.settings.API_key = APIBox.Text;
                App.settings.OctoPrintIP = IPBox.Text;
                App.settings.updateInterval = Convert.ToInt32( UpdateBox.Text);
                MainWindow.saveSettings(App.settings.settingsFile);
                MainWindow.dataTimer.Interval = new TimeSpan(0, 0, App.settings.updateInterval);
                MainWindow.dataTimer.Start();
                Hide();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error trying to save settings");
                Show();
            }
        }

        private void connBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((APIBox.Text.Length > 10) || (IPBox.Text.Length > 10))
            {
                try
                {
                    App.settings.API_key = APIBox.Text;
                    App.settings.OctoPrintIP = IPBox.Text;
                    MainWindow.getVersion();
                    this.Resources["OK_icon_visibility"] = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    this.Resources["OK_icon_visibility"] = Visibility.Hidden;
                    System.Windows.Forms.MessageBox.Show("Couldn't connect to OctoPrint.\nMake sure your IP setting was corrent\nand you have connected to your printer in OctoPrint.");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Fill in the IP and API");
            }
        }

        private void IPBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IPBox.SelectAll();
        }

        public void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            App.settings.visibleProgressbar = TaskbarItemProgressState.Normal;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            App.settings.visibleProgressbar = TaskbarItemProgressState.Error;
        }

        private void greenBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.settings;
            App.settings.visibleProgressbar = TaskbarItemProgressState.Normal;
        }

        private void yellowBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.settings;
            App.settings.visibleProgressbar = TaskbarItemProgressState.Paused;
        }
    }
}
