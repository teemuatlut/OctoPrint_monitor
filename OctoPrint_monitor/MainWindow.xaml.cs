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
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Threading;
using System.Windows.Shell;
using System.ComponentModel;


namespace OctoPrint_monitor
{
    public partial class MainWindow : Window
    {
        printer printer_data = new printer();
        jobMain job_data = new jobMain();
        public static DispatcherTimer dataTimer = new DispatcherTimer();
        public BackgroundWorker worker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();

            TaskbarItemInfo.Description = Properties.Settings.Default.version;
            TaskbarItemInfo.ProgressValue = 0.5;
            TextBlock1.FontSize = 12;
            initializeTicker();
            create_bgWorker();
            System.Windows.Forms.MessageBox.Show(Convert.ToString(this.Background.GetType()));
            //Properties.Settings.Default.OpacitySetting = 50;

           //if (App.settings.OctoPrintIP == null || App.settings.API_key == null)
           // {
           //     if (File.Exists(App.settings.settingsFile))
           //     {
           //         readSettings(App.settings.settingsFile);
           //         if ((App.settings.OctoPrintIP != null) && (App.settings.API_key != null))
           //         {
           //             worker.RunWorkerAsync();
           //             dataTimer.Start();
           //         }

           //     }
           //     else
           //     {
           //         settingsWindow SettingWindow = new settingsWindow();
           //         SettingWindow.Show();
           //     }
           // }

            if (Properties.Settings.Default.IP == "Please fill" || Properties.Settings.Default.API_key == "Please fill")
            {
                settingsWindow SettingWindow = new settingsWindow();
                SettingWindow.Show();
            }
            worker.RunWorkerAsync();
        }

        public void initializeTicker()
        {
            dataTimer.Tick += dataTimer_Tick;
            //dataTimer.Interval = new TimeSpan(0, 0, App.settings.updateInterval);
            dataTimer.Interval = new TimeSpan(0, 0, Properties.Settings.Default.updateInterval);
            dataTimer.Stop();
        }

        void dataTimer_Tick(object sender, EventArgs e)
        {
            worker.RunWorkerAsync();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dataTimer.Stop();
            myTaskbarIcon.Dispose();
        }

        private void tryBtn_Click(object sender, RoutedEventArgs e)
        {
            worker.RunWorkerAsync();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.DataContext = App.settings;
            this.DataContext = Properties.Settings.Default;
        }

        void create_bgWorker()
        {
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            printer_data = getPrinter();
            job_data = (printer_data.state.flags.printing || printer_data.state.flags.paused) ? (getJob()) : null;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                TextBlock1.Text = "";
                if (1 == 1) TextBlock1.Text += "Printer state: " + printer_data.state.stateString + "\n";
                if (1 == 1) TextBlock1.Text += "Tool temp: " + printer_data.temperature.temps.tool0.actual.ToString();
                if (1 == 1) TextBlock1.Text += "/" + printer_data.temperature.temps.tool0.target.ToString() + "\n";
                if (1 == 1) TextBlock1.Text += "Bed temp: " + printer_data.temperature.temps.bed.actual.ToString() + "\n";
                if (job_data != null)
                {
                    TextBlock1.Text += "Job progress: " + job_data.progress.completion.ToString();
                    //TaskbarItemInfo.ProgressValue = job_data.progress.completion/100;
                }
                //TextBlock1.Text += "Bar: " + App.settings.visibleProgressbar.ToString();
                TextBlock1.Text += "Bar: " + Properties.Settings.Default.visibleProgressbar.ToString();
                Application.Current.Resources["Try_visibility"] = Visibility.Hidden;
                window_frame.Title = "";
                if (job_data != null) window_frame.Title += job_data.progress.completion.ToString() + " | ";
                if (1 == 1) window_frame.Title += printer_data.temperature.temps.tool0.actual.ToString() + " | " ;
                if (1 == 1) window_frame.Title += printer_data.temperature.temps.bed.actual.ToString();
            }
            catch (Exception ex)
            {
                dataTimer.Stop();
                Application.Current.Resources["Try_visibility"] = Visibility.Visible;
                //this.TextBlock1.Text = "Could not connect to printer.\n"
                //    + "Ip setting: " + App.settings.OctoPrintIP + "\n"
                //    + "API-key: " + App.settings.API_key;
                this.TextBlock1.Text = "Could not connect to printer.\n"
                    + "Ip setting: " + Properties.Settings.Default.IP + "\n"
                    + "API-key: " + Properties.Settings.Default.API_key;
                System.Windows.Forms.MessageBox.Show("Error:\n" + ex);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            settingsWindow SettingWindow = new settingsWindow();
            SettingWindow.Show();
        }

        private void MenuItem_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void window_frame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }

}
