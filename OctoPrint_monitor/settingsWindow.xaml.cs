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
using System.Windows.Shapes;

namespace OctoPrint_monitor
{
    /// <summary>
    /// Interaction logic for settingsWindow.xaml
    /// </summary>
    public partial class settingsWindow : Window
    {
        //cSettings settings = new cSettings();
        public settingsWindow()
        {
            InitializeComponent();
            APIBox.Text = MainWindow.settings.API_key;
            IPBox.Text = MainWindow.settings.OctoPrintIP;
        }

        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //var test = new MainWindow();
            //MainWindow.saveSettings("myFile.bin");
            if (APIBox.Text.Length > 5) { MainWindow.settings.API_key = APIBox.Text; }
            if (IPBox.Text.Length > 5) { MainWindow.settings.OctoPrintIP = IPBox.Text; }
            MainWindow.saveSettings(MainWindow.settings.settingsFile);
            //settingsClosing();
        }
    }
}
