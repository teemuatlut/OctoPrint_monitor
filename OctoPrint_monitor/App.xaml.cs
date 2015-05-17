using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;

namespace OctoPrint_monitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static cSettings settings = new cSettings();
    }

    [Serializable]
    public class cSettings : INotifyPropertyChanged
    {
        private string _OctoPrintIP = null;
        private string _API_key = null;
        private string _settingsFile = "myFile.xml";
        private int _updateInterval = 5;
        private TaskbarItemProgressState _visibleProgressbar = TaskbarItemProgressState.Normal;
        //public string error_noConnection = "Couldn't connect to OctoPrint.\nMake sure your IP setting was corrent\nand you have connected to your printer in OctoPrint.";
        public event PropertyChangedEventHandler PropertyChanged;

        public string OctoPrintIP
        {
            get { return _OctoPrintIP; }
            set
            {
                _OctoPrintIP = value;
                OnPropertyChanged("OctoPrintIP");
            }
        }
        public string API_key
        {
            get { return _API_key; }
            set
            {
                _API_key = value;
                OnPropertyChanged("API_key");
            }
        }
        public string settingsFile
        {
            get { return _settingsFile; }
            set
            {
                _settingsFile = value;
                OnPropertyChanged("settingsFile");
            }
        }
        public int updateInterval
        {
            get { return _updateInterval; }
            set
            {
                _updateInterval = value;
                OnPropertyChanged("updateInterval");
            }
        }
        public TaskbarItemProgressState visibleProgressbar
        {
            get { return _visibleProgressbar ; }
            set
            {
                _visibleProgressbar = value;
                OnPropertyChanged("visibleProgressbar");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }
    }
}
