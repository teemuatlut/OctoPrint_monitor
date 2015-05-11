using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoPrint_monitor
{
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
    public class jobMain
    {
        public cJob job { get; set; }
        public cProgress progress { get; set; }
    }
    public class cJob
    {
        public cFile file { get; set; }
        public int estimatedPrintTime { get; set; }
        public cFilament filament { get; set; }
    }
    public class cProgress
    {
        public float completion { get; set; }
        public int filepos { get; set; }
        public int printTime { get; set; }
        public int printTimeLeft { get; set; }
    }
    public class cFilament
    {
        public int length { get; set; }
        public float volume { get; set; }
    }
    public class cFile
    {
        public string name { get; set; }
        public string origin { get; set; }
        public int size { get; set; }
        public int date { get; set; }
    }
    public class version
    {
        public string api { get; set; }
        public string server { get; set; }
    }

    [Serializable]
    public class cSettings
    {
        public string OctoPrintIP = null;
        public string API_key = null;//"3CE3939BEA404D2B9D9BE5A1B3CCD093"; //null;
        public string settingsFile = "myFile.xml";
        public int updateInterval = 5;
    }
}
