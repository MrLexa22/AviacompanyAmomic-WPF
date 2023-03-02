using Aviacompany_Amomic.UI.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Aviacompany_Amomic
{
    public static class ObjectsClass
    {
        public static MainWindow window_main { get; set; }
        public static int ID_User_Auth { get; set; }
        public static Session1_13Entities1 db { get; set; }
        public static int session_id { get; set; }
        public static void ShwoMessage(string title, string message, System.Windows.Window window)
        {
            WindowMessage windowMessage = new WindowMessage(title, message);
            windowMessage.Owner = window;
            windowMessage.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            windowMessage.ShowDialog();
        }
    }
}
