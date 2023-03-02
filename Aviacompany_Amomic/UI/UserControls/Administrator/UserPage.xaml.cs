using Aviacompany_Amomic.Models;
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

namespace Aviacompany_Amomic.UI.UserControls.Administrator
{
    /// <summary>
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();
            var logs = ObjectsClass.db.LogsUsers.Where(p=>p.User_ID == ObjectsClass.ID_User_Auth).OrderBy(p=>p.DateTime_Login).ToList();
            var logs_now = ObjectsClass.db.LogsUsers.Where(p => p.ID_Log == ObjectsClass.session_id).First();
            logs.Remove(logs_now);
            var logs_list = logs.Select(p=>new UserLoginLogoutModel(p.DateTime_Login,p.DateTime_Exit,p.WasCrashAndReason)).ToList();

            tb_numbercrash.Text = logs.Where(p => p.DateTime_Exit == null && p.WasCrashAndReason != null).Count().ToString();
            tb_firstName.Text = ObjectsClass.db.Users.Where(p => p.ID == ObjectsClass.ID_User_Auth).First().FirstName.Replace("\r","").Replace("\n", "");

            var logs_without_errors = logs.Where(p => p.DateTime_Exit != null).ToList();


            double totalSeconds = 0;
            var logs_for_total = logs_without_errors.Where(p => p.DateTime_Login >= DateTime.Now.AddDays(-30));
            foreach (var a in logs_for_total)
            {
                totalSeconds += ((TimeSpan)a.DateTime_Exit?.TimeOfDay - a.DateTime_Login.TimeOfDay).TotalSeconds;
            }
            TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);
            tb_timeonsystem.Text = timeSpan.Hours.ToString()+":"+timeSpan.Minutes.ToString()+ ":"+timeSpan.Seconds.ToString();
            datagrid_errors.ItemsSource = logs_list;
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            if (ObjectsClass.ID_User_Auth != 0)
            {
                var logs = ObjectsClass.db.LogsUsers.Where(p => p.ID_Log == ObjectsClass.session_id).First();
                logs.DateTime_Exit = DateTime.Now;
                ObjectsClass.db.LogsUsers.AddOrUpdate(logs);
                ObjectsClass.db.SaveChanges();
            }
            ObjectsClass.window_main.main_frame.Content = new AuthenticationPage();
        }
    }
}
