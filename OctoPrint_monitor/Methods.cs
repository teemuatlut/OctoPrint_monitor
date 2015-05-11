using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;

namespace OctoPrint_monitor
{
    public partial class MainWindow
    {
        public version getVersion()
        {
            string json_string = getJSONstring(settings.OctoPrintIP + "/api/version");
            //return convertVersionJSONtoObj(json_string) as version;

            MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(json_string));
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(version));
            return (version)js.ReadObject(ms) as version;
        }
        public conn getConnection()
        {
            string json_string = getJSONstring(settings.OctoPrintIP + "/api/connection");
            //return convertConnectionJSONtoObj(json_string) as conn;

            MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(json_string));
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(conn));
            return (conn)js.ReadObject(ms) as conn;
        }
        public printer getPrinter()
        {
            string json_string = getJSONstring(settings.OctoPrintIP + "/api/printer");
            //return convertPrinterJSONtoObj(json_string) as printer;

            MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(json_string));
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(printer));
            return (printer)js.ReadObject(ms) as printer;
        }
        public jobMain getJob()
        {
            string json_string = getJSONstring(settings.OctoPrintIP + "/api/job");
            //return convertPrinterJSONtoObj(json_string) as printer;

            MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(json_string));
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(jobMain));
            return (jobMain)js.ReadObject(ms) as jobMain;
        }
        public string getJSONstring(string octoPiIP)
        {
            WebRequest request = WebRequest.Create("http://" + octoPiIP);
            request.ContentType = "application/json";
            request.Headers.Add("X-Api-key: " + settings.API_key);
            request.Proxy = null;
            request.Method = "GET";

            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            return reader.ReadToEnd() as string;

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
            formatter.Serialize(fs, settings);
            fs.Close();            
        }
        public void readSettings(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            XmlSerializer formatter = new XmlSerializer(typeof(cSettings));
            //BinaryFormatter formatter = new BinaryFormatter();
            settings = (cSettings)formatter.Deserialize(fs);
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
        public void updateScreen()
        {
            try
            {
                data = getPrinter();
                data.temperature.tool0.actual.ToString();
                TextBlock1.Text = "Printer state: " + data.state.text.ToString() + "\n"
                                    + "Tool temp: " + data.temperature.tool0.actual.ToString() + "\n"
                                    + "Bed temp: " + data.temperature.bed.actual.ToString();
                //TextBlock1.Text = "Current IP setting:\n"+settings.OctoPrintIP;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Couldn't retrieve data from OctoPrint.\nCheck your IP setting.\nError: "+ex);
                dataTimer.Stop();
                throw;
            }
        }
    }
}