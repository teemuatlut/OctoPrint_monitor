using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Windows;

namespace OctoPrint_monitor
{
    public partial class MainWindow
    {
        //private void saveSettingsToFile(object sender, RoutedEventArgs e)
        //{
        //    //settings.OctoPrintIP = TextBox1.Text;
        //    //conn connectionSettings = getConnection();

        //    //TextBlock1.Text = Convert.ToString(connectionSettings.current.state + "\n"
        //    //    + connectionSettings.options.printerProfiles[0].name + "\n"
        //    //    + connectionSettings.options.baudrates[0]) + "\n"
        //    //    + settings.OctoPrintIP.ToString();
        //    saveSettings(settings.settingsFile);
        //    //MessageBox.Show(Properties.Settings.Default.test_setting1.ToString());
        //}

        //private void readSettingsFile(object sender, RoutedEventArgs e)
        //{
        //    readSettings(settings.settingsFile);
        //    TextBlock1.Text = Convert.ToString(
        //        "API-key: "+settings.API_key + "\n"
        //        + "IP: "+settings.OctoPrintIP
        //        );
        //}

        //private void connectBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    /*printer printerSettings = getPrinter();

        //    TextBlock1.Text = Convert.ToString("Current state: "+printerSettings.state.text+"\n"
        //        +"Current hotend temperature: "+printerSettings.temperature.tool0.actual);
        //     */
        //    dataTimer.Start();
        //    updateScreen();
        //}
    }
}