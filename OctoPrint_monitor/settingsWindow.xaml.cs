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
            //APIBox.Text = App.settings.API_key;
            //IPBox.Text = App.settings.OctoPrintIP;
            //UpdateBox.Text = App.settings.updateInterval.ToString();
            //barCheck.IsChecked = (App.settings.visibleProgressbar == TaskbarItemProgressState.Normal);
            APIBox.Text = Properties.Settings.Default.API_key;
            IPBox.Text = Properties.Settings.Default.IP;
            UpdateBox.Text = Properties.Settings.Default.updateInterval.ToString();
            barCheck.IsChecked = (Properties.Settings.Default.visibleProgressbar == TaskbarItemProgressState.Normal);
            onTopBox.IsChecked = (Properties.Settings.Default.AlwaysOnTop == true);
            opacityValue.Text = (Properties.Settings.Default.OpacitySetting*100).ToString();
            taskbarToggle.IsChecked = Properties.Settings.Default.TaskbarToggle;
        }

        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                //App.settings.API_key = APIBox.Text;
                //App.settings.OctoPrintIP = IPBox.Text;
                //App.settings.updateInterval = Convert.ToInt32( UpdateBox.Text);
                //MainWindow.saveSettings(App.settings.settingsFile);
                //MainWindow.dataTimer.Interval = new TimeSpan(0, 0, App.settings.updateInterval);
                //MainWindow.dataTimer.Start();
                //Hide();
                Properties.Settings.Default.API_key = APIBox.Text;
                Properties.Settings.Default.IP = IPBox.Text;
                Properties.Settings.Default.updateInterval = Convert.ToInt32(UpdateBox.Text);
                //MainWindow.saveSettings(Properties.Settings.Default.settingsFile);
                Properties.Settings.Default.Save();
                MainWindow.dataTimer.Interval = new TimeSpan(0, 0, Properties.Settings.Default.updateInterval);
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
                    //App.settings.API_key = APIBox.Text;
                    //App.settings.OctoPrintIP = IPBox.Text;
                    Properties.Settings.Default.API_key = APIBox.Text;
                    Properties.Settings.Default.IP = IPBox.Text;
                    MainWindow.getVersion();
                    this.Resources["OK_icon_visibility"] = Visibility.Visible;
                    //System.Windows.Media.Brush newBrush = new System.Windows.Media.Brush;
                    //connBtn.BorderBrush = System.Windows.Media.Brush.Green;
                    //connBtn.BorderBrush = "Green";
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
            //App.settings.visibleProgressbar = TaskbarItemProgressState.Normal;
            Properties.Settings.Default.visibleProgressbar = TaskbarItemProgressState.Normal;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //App.settings.visibleProgressbar = TaskbarItemProgressState.Error;
            Properties.Settings.Default.visibleProgressbar = TaskbarItemProgressState.None;
        }

        private void onTopBox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.AlwaysOnTop = true;
        }

        private void onTopBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.AlwaysOnTop = false;
        }

        private void opacityValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (opacityValue.IsFocused)
            {
                try
                {
                    int value = Convert.ToInt16(opacityValue.Text);
                    if (value > 100) value = 100;
                    if (value < 10) value = 10;
                    Properties.Settings.Default.OpacitySetting = (double)value / 100;
                }
                catch (Exception ex) { }
            }
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.TaskbarToggle = true;
        }

        private void taskbarToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.TaskbarToggle = false;
        }

        //private void greenBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    this.DataContext = App.settings;
        //    App.settings.visibleProgressbar = TaskbarItemProgressState.Normal;
        //}

        //private void yellowBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    this.DataContext = App.settings;
        //    App.settings.visibleProgressbar = TaskbarItemProgressState.Paused;
        //}
    }
}
