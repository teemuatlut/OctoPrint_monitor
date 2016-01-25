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

        public static Uri redIconUri = new Uri("pack://application:,,,/Icons/Error.ico", UriKind.RelativeOrAbsolute);
        public static Uri grayIconUri = new Uri("pack://application:,,,/Icons/Inactive.ico", UriKind.RelativeOrAbsolute);
        public static BitmapFrame redIcon = BitmapFrame.Create(redIconUri);
        public static BitmapFrame grayIcon = BitmapFrame.Create(grayIconUri);

        public MainWindow()
        {
            InitializeComponent();

            TaskbarItemInfo.Description = Properties.Settings.Default.version;

            TextBlock1.FontSize = 12;

            if (Properties.Settings.Default.taskIconToggle.Equals(true))
                App.Current.Resources["isVisibleGrayIcon"] = System.Windows.Visibility.Visible;

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
            if(worker.IsBusy.Equals(false))
                worker.RunWorkerAsync();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SettingWindow.Close();
            dataTimer.Stop();
            myRedIcon.Dispose();
            myGrayIcon.Dispose();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
            catch (WebException ex)
            {
                printer_data = null;
                job_data = null;
                throw new WebException();
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (e.Error.GetType().ToString() == "System.Net.WebException")
                {
                    //System.Windows.Forms.MessageBox.Show("myErrorType: "+e.Error.GetType().ToString());
                    this.TextBlock1.Text = "Could not connect to printer.\n";
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("E10: "+e.Error.ToString());
                }
            }
            else
            {
                try
                {
                    update_screen();
                    showBalloon();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("E01:" + ex);
                }
            }
        }
        void update_screen()
        {
            StringBuilder myBuilder = new StringBuilder();
            StringBuilder barInfo = new StringBuilder();
            //if (1 == 1) myBuilder.AppendFormat("Printer state: {0}", printer_data.state.stateString);
            myBuilder.Append("Printer state: ");
            myBuilder.Append(printer_data.state.text);
            if (1 == 1) myBuilder.AppendLine().AppendFormat("Tool temp: {0:0.0}", printer_data.temperature.tool0.actual);
            if (Properties.Settings.Default.showTarget.Equals(true)) myBuilder.AppendFormat("/{0:0}", printer_data.temperature.tool0.target);
            if (1 == 1) myBuilder.AppendLine().AppendFormat("Bed temp: {0:0.0}", printer_data.temperature.bed.actual);
            if (Properties.Settings.Default.showTarget.Equals(true)) myBuilder.AppendFormat("/{0:0}", printer_data.temperature.bed.target);
            if (job_data != null)
            {
                myBuilder.AppendLine().AppendFormat("Job progress: {0:0}%", job_data.progress.completion);
                TaskbarItemInfo.ProgressValue = job_data.progress.completion / 100;

                previousPrintTime = job_data.progress.printTime ?? default(int);
                var ETAvalue = job_data.progress.printTimeLeft ?? default(int);
                if (ETAvalue < 0) {
                    myBuilder.AppendLine().AppendFormat("ETA: -");
                    barInfo.AppendFormat("-% | -h -m");
                    this.Resources["notifyPopupText"] = string.Format("-% | -h -m");
                }
                else
                {
                    var ETA = new DateTime(0);
                    ETA = ETA.AddSeconds(ETAvalue);
                    myBuilder.AppendLine().AppendFormat("ETA: {0}h {1}m", ETA.Hour.ToString(), ETA.Minute.ToString());

                    barInfo.AppendFormat("{0:0}% | {1}h {2}m", job_data.progress.completion, ETA.Hour.ToString(), ETA.Minute.ToString());

                    this.Resources["notifyPopupText"] = string.Format("{0:0}% | {1}h {2}m", job_data.progress.completion, ETA.Hour.ToString(), ETA.Minute.ToString());
                }

                if (Properties.Settings.Default.taskIconToggle.Equals(true))
                {
                    Application.Current.Resources["isVisibleGrayIcon"] = Visibility.Hidden;
                    Application.Current.Resources["isVisibleRedIcon"] = Visibility.Visible;
                }

                this.Icon = redIcon;

                App.isPrinting = true;
            }
            else
            {
                TaskbarItemInfo.ProgressValue = 0;
                //myTaskbarIcon.Icon = new System.Drawing.Icon("Icons/Inactive.ico");
                barInfo.AppendFormat("[{0}]", printer_data.state.text);
                if (Properties.Settings.Default.taskIconToggle.Equals(true))
                {
                    Application.Current.Resources["isVisibleRedIcon"] = Visibility.Hidden;
                    Application.Current.Resources["isVisibleGrayIcon"] = Visibility.Visible;
                }
                this.Icon = grayIcon;
                App.isPrinting = false;
            }
            //myBuilder2.AppendFormat("{0:0.0} | {1:0.0}",
            //    printer_data.temperature.temps.tool0.actual,
            //    printer_data.temperature.temps.bed.actual);

            TextBlock1.Text = myBuilder.ToString();
            window_frame.Title = barInfo.ToString();
        }
        void showBalloon()
        {
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
                    var value = new DateTime(0);
                    value = value.AddSeconds(previousPrintTime);

                    myGrayIcon.ShowBalloonTip("Print finished!",
                        "Elapsed time: "
                        + value.Hour.ToString() + "h "
                        + value.Minute.ToString() + "min",
                        BalloonIcon.Info);
                    //System.Windows.Forms.MessageBox.Show("Print finished!");
                }
                printerPreviousState = printer_data.state.flags.printing;
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

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            openWebPage();
        }
    }

}
