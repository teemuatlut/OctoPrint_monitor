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
using Hardcodet.Wpf.TaskbarNotification;


namespace OctoPrint_monitor
{
    public partial class MainWindow : Window
    {
        printer printer_data = new printer();
        jobMain job_data = new jobMain();
        public static DispatcherTimer dataTimer = new DispatcherTimer();
        public static BackgroundWorker worker = new BackgroundWorker();
        public static settingsWindow SettingWindow = new settingsWindow();
        static bool printerPreviousState = false;
        static int previousPrintTime = 0;
        //public static TaskbarIcon myTaskbarIcon = new TaskbarIcon();

        public static int i = 0;

        public MainWindow()
        {
            InitializeComponent();

            TaskbarItemInfo.Description = Properties.Settings.Default.version;
            //TaskbarItemInfo.ProgressValue = 0.5;
            TextBlock1.FontSize = 12;

            if (Properties.Settings.Default.taskIconToggle.Equals(true))
                App.Current.Resources["isVisible"] = System.Windows.Visibility.Visible;
            else
                App.Current.Resources["isVisible"] = System.Windows.Visibility.Hidden;

            initializeTicker();
            create_bgWorker();

            if (Properties.Settings.Default.IP == "Please fill" || Properties.Settings.Default.API_key == "Please fill")
            {
                //settingsWindow SettingWindow = new settingsWindow();
                SettingWindow.Show();
            }
            else
            {
                worker.RunWorkerAsync();
                dataTimer.Start();
            }
        }

        public void initializeTicker()
        {
            dataTimer.Tick += dataTimer_Tick;
            dataTimer.Interval = new TimeSpan(0, 0, Properties.Settings.Default.updateInterval);
        }

        void dataTimer_Tick(object sender, EventArgs e)
        {
            //window_frame.Title = i.ToString();
            //if (i % 10 == 0 && worker.IsBusy.Equals(false))
            if(worker.IsBusy.Equals(false))
                worker.RunWorkerAsync();
            //i++;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SettingWindow.Close();
            dataTimer.Stop();
            myTaskbarIcon.Dispose();
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
            try
            {
                printer_data = getPrinter();
                job_data = (printer_data.state.flags.printing || printer_data.state.flags.paused) ? (getJob()) : null;
            }
            catch (Exception ex)
            {
                printer_data = null;
                job_data = null;
                System.Windows.Forms.MessageBox.Show("Error:\n" + ex);
                return;
                //throw;
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                update_screen();
                if (job_data != null)
                {
                    previousPrintTime = job_data.progress.printTime ?? default(int);
                }
                if (printerPreviousState.Equals(true)
                    && printer_data.state.flags.printing.Equals(false)
                    && printer_data.state.flags.paused.Equals(false)
                    && printer_data.state.flags.error.Equals(false)
                    && Properties.Settings.Default.taskIconToggle.Equals(true))
                {
                    //double? _int = previousPrintTime;
                    var value = new DateTime(0);
                    value = value.AddSeconds(previousPrintTime);
                    //Console.WriteLine("Print time was "
                    //    + value.Hour.ToString() + "h "
                    //    + value.Minute.ToString() + "s");

                    myTaskbarIcon.ShowBalloonTip("Print finished!",
                        "Elapsed time: "
                        + value.Hour.ToString() + "h "
                        + value.Minute.ToString() + "min",
                        BalloonIcon.Info);
                    //System.Windows.Forms.MessageBox.Show("Print finished!");
                }
                printerPreviousState = printer_data.state.flags.printing;
            }
            catch (WebException wex)
            {
                //dataTimer.Stop();
                this.TextBlock1.Text = "Could not connect to printer.\n";
                //    + "Ip setting: " + Properties.Settings.Default.IP + "\n"
                //    + "API-key: " + Properties.Settings.Default.API_key;
                //System.Windows.Forms.MessageBox.Show("Error:\n" + ex);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error:" + ex);
            }
        }
        void update_screen()
        {
            StringBuilder myBuilder = new StringBuilder();
            StringBuilder myBuilder2 = new StringBuilder();
            //if (1 == 1) myBuilder.AppendFormat("Printer state: {0}", printer_data.state.stateString);
            myBuilder.Append("Printer state: ");
            myBuilder.Append(printer_data.state.stateString);
            if (1 == 1) myBuilder.AppendLine().AppendFormat("Tool temp: {0:0.0}", printer_data.temperature.temps.tool0.actual);
            if (Properties.Settings.Default.showTarget.Equals(true)) myBuilder.AppendFormat("/{0:0}", printer_data.temperature.temps.tool0.target);
            if (1 == 1) myBuilder.AppendLine().AppendFormat("Bed temp: {0:0.0}", printer_data.temperature.temps.bed.actual);
            if (Properties.Settings.Default.showTarget.Equals(true)) myBuilder.AppendFormat("/{0:0}", printer_data.temperature.temps.bed.target);
            if (job_data != null)
            {
                myBuilder.AppendLine().AppendFormat("Job progress: {0:0}%", job_data.progress.completion);
                TaskbarItemInfo.ProgressValue = job_data.progress.completion / 100;
                myBuilder2.AppendFormat("{0:0}% | ", job_data.progress.completion);
            }
            myBuilder2.AppendFormat("{0:0.0} | {1:0.0}",
                printer_data.temperature.temps.tool0.actual,
                printer_data.temperature.temps.bed.actual);

            TextBlock1.Text = myBuilder.ToString();
            window_frame.Title = myBuilder2.ToString();

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
