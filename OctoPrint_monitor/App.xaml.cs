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
    public partial class App : Application
    {
        public static bool isPrinting = false;

        //public static Uri redIconUri = new Uri("pack://application:,,,/Icons/Error.ico", UriKind.RelativeOrAbsolute);
        //public static Uri grayIconUri = new Uri("pack://application:,,,/Icons/Inactive.ico", UriKind.RelativeOrAbsolute);
    }
}
