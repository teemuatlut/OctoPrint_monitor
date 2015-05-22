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
                //json_string = getJSONstring(App.settings.OctoPrintIP + "/api/version");
                json_string = getJSONstring(Properties.Settings.Default.IP + "/api/version");
            }
            catch (Exception ex) 
            {
                System.Windows.Forms.MessageBox.Show("Couldn't connect to OctoPrint.\nMake sure your IP setting was corrent\nand you have connected to your printer in OctoPrint.");
            }
            MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(json_string));
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(version));
            return (version)js.ReadObject(ms) as version;
        }
        public conn getConnection()
        {
            //string json_string = getJSONstring(App.settings.OctoPrintIP + "/api/connection");
            string json_string = getJSONstring(Properties.Settings.Default.IP + "/api/connection");

            MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(json_string));
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(conn));
            return (conn)js.ReadObject(ms) as conn;
        }
        public printer getPrinter()
        {
            //string json_string = getJSONstring(App.settings.OctoPrintIP + "/api/printer");
            string json_string = getJSONstring(Properties.Settings.Default.IP + "/api/printer");

            MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(json_string));
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(printer));
            return (printer)js.ReadObject(ms) as printer;
        }
        public jobMain getJob()
        {
            //string json_string = getJSONstring(App.settings.OctoPrintIP + "/api/job");
            string json_string = getJSONstring(Properties.Settings.Default.IP + "/api/job");

            MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(json_string));
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(jobMain));
            return (jobMain)js.ReadObject(ms) as jobMain;
        }
        public static string getJSONstring(string octoPiIP)
        {
            WebRequest request = WebRequest.Create("http://" + octoPiIP);
            request.ContentType = "application/json";
            //request.Headers.Add("X-Api-key: " + App.settings.API_key);
            request.Headers.Add("X-Api-key: " + Properties.Settings.Default.API_key);
            request.Proxy = null;
            request.Method = "GET";

            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            return reader.ReadToEnd() as string;
        }

        //public static void saveSettings(string fileName)
        //{
        //    FileStream fs = new FileStream(fileName, FileMode.Create);
        //    XmlSerializer formatter = new XmlSerializer(typeof(cSettings));
        //    //BinaryFormatter formatter = new BinaryFormatter();
        //    formatter.Serialize(fs, App.settings);
        //    fs.Close();            
        //}
        //public void readSettings(string fileName)
        //{
        //    FileStream fs = new FileStream(fileName, FileMode.Open);
        //    XmlSerializer formatter = new XmlSerializer(typeof(cSettings));
        //    //BinaryFormatter formatter = new BinaryFormatter();
        //    App.settings = (cSettings)formatter.Deserialize(fs);
        //    fs.Close();
        //}
        //public void hideFile(string fileName)
        //{
        //    FileAttributes attributes = File.GetAttributes(fileName);
        //    File.SetAttributes(fileName, FileAttributes.Hidden);
        //}
        //public void unhideFile(string fileName)
        //{
        //    FileAttributes attributes = File.GetAttributes(fileName);
        //    attributes = (attributes & ~FileAttributes.Hidden);
        //    File.SetAttributes(fileName, attributes);
        //}
    }
}