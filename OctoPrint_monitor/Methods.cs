using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;
using System.Windows;

namespace OctoPrint_monitor
{
    public partial class MainWindow
    {
        public static version getVersion()
        {
            string json_string="";
            try
            {
                json_string = getJSONstring(App.settings.OctoPrintIP + "/api/version");
                //System.Windows.Forms.MessageBox.Show("Connection works ok!");
                //settingsWindow.ConnOKico.Visibility = "Visible";
            }
            catch (Exception ex) 
            {
                System.Windows.Forms.MessageBox.Show("Couldn't connect to OctoPrint.\nMake sure your IP setting was corrent\nand you have connected to your printer in OctoPrint.");
                //string json_string = null;
            }
            //return convertVersionJSONtoObj(json_string) as version;
            MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(json_string));
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(version));
            return (version)js.ReadObject(ms) as version;
        }
        public conn getConnection()
        {
            string json_string = getJSONstring(App.settings.OctoPrintIP + "/api/connection");
            //return convertConnectionJSONtoObj(json_string) as conn;

            MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(json_string));
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(conn));
            return (conn)js.ReadObject(ms) as conn;
        }
        public printer getPrinter()
        {
            string json_string = getJSONstring(App.settings.OctoPrintIP + "/api/printer");
            //return convertPrinterJSONtoObj(json_string) as printer;

            MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(json_string));
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(printer));
            return (printer)js.ReadObject(ms) as printer;
        }
        public jobMain getJob()
        {
            string json_string = getJSONstring(App.settings.OctoPrintIP + "/api/job");
            //return convertPrinterJSONtoObj(json_string) as printer;

            MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(json_string));
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(jobMain));
            return (jobMain)js.ReadObject(ms) as jobMain;
        }
        public static string getJSONstring(string octoPiIP)
        {
            WebRequest request = WebRequest.Create("http://" + octoPiIP);
            request.ContentType = "application/json";
            request.Headers.Add("X-Api-key: " + App.settings.API_key);
            request.Proxy = null;
            request.Method = "GET";
            //try
            //{
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                return reader.ReadToEnd() as string;
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show("Couldn't retrieve data from OctoPrint.\n"
            //        +"Check your IP setting.\n If the problem persists, delete your "
            //        +MainWindow.settings.settingsFile
            //        +" file\n"
            //        +"Ip given: "
            //        +settings.OctoPrintIP
            //        +"\nAPI-key: "
            //        +settings.API_key
            //        +"\nError: "
            //        + ex);
            //    dataTimer.Stop();
            //    throw;
            //}
        }
/*
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
        public version convertVersionJSONtoObj(string responseFromServer)
        {
            MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(responseFromServer));
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(version));
            return (version)js.ReadObject(ms) as version;
        }
*/
        public static void saveSettings(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            XmlSerializer formatter = new XmlSerializer(typeof(cSettings));
            //BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, App.settings);
            fs.Close();            
        }
        public void readSettings(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            XmlSerializer formatter = new XmlSerializer(typeof(cSettings));
            //BinaryFormatter formatter = new BinaryFormatter();
            App.settings = (cSettings)formatter.Deserialize(fs);
            fs.Close();
        }
        public void hideFile(string fileName)
        {
            FileAttributes attributes = File.GetAttributes(fileName);
            File.SetAttributes(fileName, FileAttributes.Hidden);
        }
        public void unhideFile(string fileName)
        {
            FileAttributes attributes = File.GetAttributes(fileName);
            attributes = (attributes & ~FileAttributes.Hidden);
            File.SetAttributes(fileName, attributes);
        }
        //public void updateScreen()
        //{
        //    try
        //    {
        //        //printer_data = getPrinter();
        //        //job_data = (printer_data.state.flags.printing || printer_data.state.flags.paused) ? (getJob()) : null;
        //        //worker.RunWorkerAsync();
        //        TextBlock1.Text = "";
        //        if (1 == 1) TextBlock1.Text += "Printer state: " + printer_data.state.stateString + "\n";
        //        if (1 == 1) TextBlock1.Text += "Tool temp: " + printer_data.temperature.temps.tool0.actual.ToString();
        //        if (1 == 1) TextBlock1.Text += "/" + printer_data.temperature.temps.tool0.target.ToString() + "\n";
        //        if (1 == 1) TextBlock1.Text += "Bed temp: " + printer_data.temperature.temps.bed.actual.ToString() + "\n";
        //        if (job_data != null)
        //        {
        //            TextBlock1.Text += "Job progress: " + job_data.progress.completion.ToString();
        //            //TaskbarItemInfo.ProgressValue = job_data.progress.completion/100;
        //        }
        //        TextBlock1.Text += "Bar: " + App.settings.visibleProgressbar.ToString();
        //        //TaskbarItemInfo.ProgressValue = Convert.ToDouble(printer_data.temperature.temps.tool0.actual/100);
        //        //TextBlock1.Text = "Current IP setting:\n"+settings.OctoPrintIP;
        //        Application.Current.Resources["Try_visibility"] = Visibility.Hidden;
        //    }
        //    catch (Exception ex)
        //    {
        //        dataTimer.Stop();
        //        Application.Current.Resources["Try_visibility"] = Visibility.Visible;
        //        this.TextBlock1.Text = "Could not connect to printer.\n"
        //            +"Ip setting: "+ App.settings.OctoPrintIP +"\n"
        //            +"API-key: " + App.settings.API_key;
        //        System.Windows.Forms.MessageBox.Show("Error:\n"+ex);
        //    }
        //}
    }
}