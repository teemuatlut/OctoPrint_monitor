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

namespace OctoPrint_monitor
{

    /*    public class cSettings
        {
            public string OctoPrintIP;
            public string API_key;
        }
    */
    /*    public class A5class
        {
            public string B3 { get; set; }
            public int B4 { get; set; }

        }
        public class A6class
        {
            public List<int> B5 { get; set; }
        }
        public class T
        {
            public string A1 { get; set; }
            public string A2 { get; set; }
            public string A3 { get; set; }
            public List<string> A4 { get; set; }
            public A5class A5 { get; set; }
            public A6class A6 { get; set; }
        }
        string responseFromServer = @"{
            ""A1"": ""one"",
            ""A2"": ""two"",
            ""A3"": ""three"",
            ""A4"": [""B1"",""B2""],
            ""A5"": {
                ""B3"":""four"",
                ""B4"": 555
                },
            ""A6"": {""B5"": [666,777]}
        }";
     */
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /*
        public class cPrinterProfiles
        {
            public string name { get; set; }
            public string id { get; set; }
        }
        public class cCurrent
        {
            public string baudrate { get; set; }
            public string state { get; set; }
            public string port { get; set; }
            public string printerProfile { get; set; }
        }
        public class cOptions
        {
            public List<int> baudrates { get; set; }
            public List<cPrinterProfiles> printerProfiles { get; set; }
            public string printerProfilePreference { get; set; }
            public List<string> ports { get; set; }
        }
        public class conn
        {
            public cCurrent current { get; set; }
            public cOptions options { get; set; }
        }
        public class printer
        {
            public cState state { get; set; }
            public cTemperature temperature { get; set; }
            public cSd sd { get; set; }
        }
        public class cState
        {
            public string text { get; set; }
            public cFlags flags { get; set; }
        }
        public class cFlags
        {
            public bool operational { get; set; }
            public bool paused { get; set; }
            public bool printing { get; set; }
            public bool sdReady { get; set; }
            public bool error { get; set; }
            public bool ready { get; set; }
            public bool closedError { get; set; }
        }
        public class cTemperature
        {
            public cBed bed { get; set; }
            public cTool0 tool0 { get; set; }
        }
        public class cBed
        {
            public float actual { get; set; }
            public float target { get; set; }
            public float offset { get; set; }
        }
        public class cTool0
        {
            public float actual { get; set; }
            public float target { get; set; }
            public float offset { get; set; }
        }
        public class cSd
        {
            public bool read { get; set; }
        }
    */
    public partial class MainWindow : Window
    {

        // Setup
        public static cSettings settings = new cSettings();
        settingsWindow SettingWindow = new settingsWindow();
        printer data = new printer();
        DispatcherTimer dataTimer = new DispatcherTimer();
            

        public MainWindow()
        {
            InitializeComponent();
            TextBlock1.FontSize = 24;
            initializeTicker();

            if (settings.OctoPrintIP == null || settings.API_key == null)
            {
                if (File.Exists(settings.settingsFile))
                {
                    readSettings(settings.settingsFile);
                    dataTimer.Start();
                }
                else
                {
                    SettingWindow.Show();
                }
            }
            //if (settings.OctoPrintIP == null)
            //{
            //    SettingWindow.Show();
            //}
            //TaskBarIcon 
            //Properties.Settings.Default.test_setting1

            //settings.API_key = "3CE3939BEA404D2B9D9BE5A1B3CCD093";
            //settings.OctoPrintIP = "192.168.1.166";

        }

        public void initializeTicker()
        {
            //var dataTimer = new System.Windows.Threading.DispatcherTimer();
            dataTimer.Tick += dataTimer_Tick;
            dataTimer.Interval = new TimeSpan(0, 0, settings.updateInterval);
        }

        void dataTimer_Tick(object sender, EventArgs e)
        {
            //data = getPrinter();
            //data.temperature.tool0.actual.ToString();
            //TextBlock1.Text =   "Printer state: "+data.state.text.ToString()+"\n"
            //                    +"Tool temp: "+data.temperature.tool0.actual.ToString()+"\n"
            //                    +"Bed temp: "+data.temperature.bed.actual.ToString();
            updateScreen();
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {

            SettingWindow.Show();
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //dataTimer.Stop();
        }


        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    //settings.OctoPrintIP = TextBox1.Text;
        //    //conn connectionSettings = getConnection();

        //    //TextBlock1.Text = Convert.ToString(connectionSettings.current.state + "\n"
        //    //    + connectionSettings.options.printerProfiles[0].name + "\n"
        //    //    + connectionSettings.options.baudrates[0]) + "\n"
        //    //    + settings.OctoPrintIP.ToString();
        //    saveSettings("myFile.dat");
        //    //MessageBox.Show(Properties.Settings.Default.test_setting1.ToString());
        //}

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    readSettings("myFile.dat");
        //    TextBlock1.Text = Convert.ToString(
        //        settings.API_key + "\n"
        //        + settings.OctoPrintIP
        //        );
        //}

        //private void saveSettings()
        //{
        //    IFormatter formatter = new BinaryFormatter();
        //    Stream stream = new FileStream("myFile.bin", FileMode.Create, FileAccess.Write, FileShare.None);
        //    formatter.Serialize(stream, settings);
        //    File.WriteAllText("myFile.txt", settings.ToString());
        //}


        /*
                public conn getConnection()
                {
                    string json_string = getJSONstring(OctoPrinterIP+"/api/connection");
                    return convertConnectionJSONtoObj(json_string) as conn;
                }
                public printer getPrinter()
                {
                    string json_string = getJSONstring(OctoPrinterIP + "/api/printer");
                    return convertPrinterJSONtoObj(json_string) as printer;
                }
                public string getJSONstring(string octoPiIP)
                {
                    WebRequest request = WebRequest.Create("http://" + octoPiIP);
                    request.ContentType = "application/json";
                    request.Headers.Add("X-Api-key: " + API_key);
                    request.Proxy = null;
                    request.Method = "GET";
                    WebResponse response = request.GetResponse();
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    return reader.ReadToEnd() as string;
                }
                public conn convertConnectionJSONtoObj(string responseFromServer)
                {
                    MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(responseFromServer));
                    DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(conn));
                    return (conn)js.ReadObject(ms) as conn;
                }
                public printer convertPrinterJSONtoObj(string responseFromServer)
                {
                    MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(responseFromServer));
                    DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(printer));
                    return (printer)js.ReadObject(ms) as printer;
                }
        */
    }

}
