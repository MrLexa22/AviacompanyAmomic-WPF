using Aviacompany_Amomic.UserControls;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

namespace Aviacompany_Amomic
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ObjectsClass.window_main = this;
            ObjectsClass.db = new Session1_13Entities1();
            ObjectsClass.window_main.main_frame.Content = new AuthenticationPage();
        }

        private void Main_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ObjectsClass.ID_User_Auth != 0)
            {
                var logs = ObjectsClass.db.LogsUsers.Where(p => p.ID_Log == ObjectsClass.session_id).First();
                logs.DateTime_Exit = DateTime.Now;
                ObjectsClass.db.LogsUsers.AddOrUpdate(logs);
                ObjectsClass.db.SaveChanges();
            }
        }
    }
}
