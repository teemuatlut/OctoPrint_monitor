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
            barCheck.IsChecked = (Properties.Settings.Default.visibleProgressbar == TaskbarItemProgressState.Error);
            onTopBox.IsChecked = (Properties.Settings.Default.AlwaysOnTop == true);
            iconToggle.IsChecked = (Properties.Settings.Default.taskIconToggle == true);
            targetTempCheck.IsChecked = (Properties.Settings.Default.showTarget == true);
            //opacityValue.Text = (Properties.Settings.Default.OpacitySetting*100).ToString();
            taskbarToggle.IsChecked = Properties.Settings.Default.TaskbarToggle;
            colorHexBox.Text = Properties.Settings.Default.backgroundColor.ToString();
            textColorBox.Text = Properties.Settings.Default.textColor.ToString();
            borderColorTop.Text = Properties.Settings.Default.gradientTop.ToString();
            borderColorBot.Text = Properties.Settings.Default.gradientBot.ToString();

            MainWindow.dataTimer.Stop(); // Stop the timer when settings window is opened in case the IP has changed.
        }

        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (APIBox.Text != "Please fill" && IPBox.Text != "Please fill")
            {
                Properties.Settings.Default.API_key = APIBox.Text;
                Properties.Settings.Default.IP = IPBox.Text;
                MainWindow.dataTimer.Start();
                if (MainWindow.worker.IsBusy.Equals(false))
                    MainWindow.worker.RunWorkerAsync();
            }
            Properties.Settings.Default.updateInterval = Convert.ToInt32(UpdateBox.Text);
            MainWindow.dataTimer.Interval = new TimeSpan(0, 0, Properties.Settings.Default.updateInterval);
            try
            {
                Properties.Settings.Default.Save();
                Hide();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("E07: Error trying to save settings");
                Show();
            }
        }

        private void connBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((APIBox.Text.Length > 10) || (IPBox.Text.Length > 10))
            {
                Properties.Settings.Default.API_key = APIBox.Text;
                Properties.Settings.Default.IP = IPBox.Text;
                try
                {
                    MainWindow.getVersion();
                    this.Resources["OK_icon_visibility"] = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    this.Resources["OK_icon_visibility"] = Visibility.Hidden;
                    System.Windows.Forms.MessageBox.Show("E02: Couldn't connect to OctoPrint.\nMake sure your IP setting was corrent\nand you have connected to your printer in OctoPrint.");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("E08: Fill in the IP address and API-key");
            }
        }

        private void IPBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IPBox.SelectAll();
        }

        public void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.visibleProgressbar = TaskbarItemProgressState.Error;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
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

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.TaskbarToggle = true;
        }

        private void taskbarToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.TaskbarToggle = false;
        }

        private void background_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Color colorHexAsColor = (Color)ColorConverter.ConvertFromString(colorHexBox.Text);
                SolidColorBrush myBrush = new SolidColorBrush(colorHexAsColor);
                Properties.Settings.Default.backgroundColor = myBrush;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("E03: Invalid color value");
            }
        }

        private void textColorBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try {
                Color colorHexAsColor = (Color)ColorConverter.ConvertFromString(textColorBox.Text);
                SolidColorBrush myBrush = new SolidColorBrush(colorHexAsColor);
                Properties.Settings.Default.textColor = myBrush;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("E04: Invalid color value");
            }
        }

        private void borderColorBot_LostFocus(object sender, RoutedEventArgs e)
        {
            try { 
                Color colorHexAsColor = (Color)ColorConverter.ConvertFromString(borderColorBot.Text);
                Properties.Settings.Default.gradientBot = colorHexAsColor;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("E05: Invalid color value");
            }
        }

        private void borderColorTop_LostFocus(object sender, RoutedEventArgs e)
        {
            try { 
                Color colorHexAsColor = (Color)ColorConverter.ConvertFromString(borderColorTop.Text);
                Properties.Settings.Default.gradientTop = colorHexAsColor;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("E06: Invalid color value");
            }
        }

        private void targetTempCheck_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.showTarget = true;
        }

        private void targetTempCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.showTarget = false;
        }

        private void iconToggle_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.taskIconToggle = true;

            if (App.isPrinting)
            {
                Application.Current.Resources["isVisibleRedIcon"] = Visibility.Visible;
            }
            else
            {
                Application.Current.Resources["isVisibleGrayIcon"] = Visibility.Visible;
            }

            Application.Current.Resources["myTestString"] = "Icon toggled on";
        }

        private void iconToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.taskIconToggle = false;

            Application.Current.Resources["isVisibleGrayIcon"] = Visibility.Hidden;
            Application.Current.Resources["isVisibleRedIcon"] = Visibility.Hidden;

            Application.Current.Resources["myTestString"] = "Icon toggled off";
        }

        private void Navigate_url(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            MainWindow.openWebPage();
        }

        private void IPBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Resources["IPbox_key"] = IPBox.Text;
        }
    }
}
