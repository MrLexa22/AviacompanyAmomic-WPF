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
using System.Windows.Shapes;

namespace Aviacompany_Amomic.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для NoLogoutDetected.xaml
    /// </summary>
    public partial class NoLogoutDetected : Window
    {
        public NoLogoutDetected(Users user)
        {
            InitializeComponent();
            Date_last_login.Text = user.LogsUsers.Where(p=>p.WasCrashAndReason == null && p.DateTime_Exit == null).First().DateTime_Login.Date.ToShortDateString();
            Time_last_login.Text = user.LogsUsers.Where(p => p.WasCrashAndReason == null && p.DateTime_Exit == null).First().DateTime_Login.TimeOfDay.ToString();
        }

        private void rb_Checked(object sender, RoutedEventArgs e)
        {
            tb_reason.Text = tb_reason.Text.Replace("SoftCrash. ", "");
            tb_reason.Text = tb_reason.Text.Replace("SystemCrash. ", "");
            if (rb_softcrash.IsChecked == true)
            {
                string reason = "SoftCrash. " + tb_reason.Text;
                tb_reason.Text = reason;
            }

            if (rb_syscrash.IsChecked == true)
            {
                string reason = "SystemCrash. " + tb_reason.Text;
                tb_reason.Text = reason;
            }
        }

        private void btn_confirm_Click(object sender, RoutedEventArgs e)
        {
            if (ObjectsClass.ID_User_Auth == 0)
                return;

            if (tb_reason.Text == null)
            {
                ObjectsClass.ShwoMessage("Error reason", "Reason can't be exmpty", ObjectsClass.window_main);
                return;
            }

            if (tb_reason.Text.Trim() == "")
            {
                ObjectsClass.ShwoMessage("Error reason", "Reason can't be exmpty", ObjectsClass.window_main);
                return;
            }

            if (tb_reason.Text.Trim() == "")
            {
                ObjectsClass.ShwoMessage("Error reason", "Reason can't be exmpty", ObjectsClass.window_main);
                return;
            }

            if (rb_softcrash.IsChecked == false && rb_syscrash.IsChecked == false)
            {
                ObjectsClass.ShwoMessage("Error types crash", "Select one of type crash", ObjectsClass.window_main);
                return;
            }

            var errors_logs = ObjectsClass.db.LogsUsers.Where(p => p.User_ID == ObjectsClass.ID_User_Auth && p.WasCrashAndReason == null && p.DateTime_Exit == null).ToList();
            foreach (var error in errors_logs)
            {
                error.WasCrashAndReason = tb_reason.Text.Trim();
                ObjectsClass.db.LogsUsers.AddOrUpdate(error);
            }
            ObjectsClass.db.SaveChanges();
            this.Close();
        }
        private void no_logout_detected_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ObjectsClass.ID_User_Auth == 0)
                return;

            if (tb_reason.Text == null)
            {
                ObjectsClass.ShwoMessage("Error reason", "Reason can't be exmpty", ObjectsClass.window_main);
                return;
            }

            if (tb_reason.Text.Trim() == "")
            {
                ObjectsClass.ShwoMessage("Error reason", "Reason can't be exmpty", ObjectsClass.window_main);
                return;
            }

            if (tb_reason.Text.Trim() == "")
            {
                ObjectsClass.ShwoMessage("Error reason", "Reason can't be exmpty", ObjectsClass.window_main);
                return;
            }

            if (rb_softcrash.IsChecked == false && rb_syscrash.IsChecked == false)
            {
                ObjectsClass.ShwoMessage("Error types crash", "Select one of type crash", ObjectsClass.window_main);
                return;
            }

            var errors_logs = ObjectsClass.db.LogsUsers.Where(p => p.User_ID == ObjectsClass.ID_User_Auth && p.WasCrashAndReason == null).ToList();
            foreach (var error in errors_logs)
            {
                error.WasCrashAndReason = tb_reason.Text.Trim();
                ObjectsClass.db.LogsUsers.AddOrUpdate(error);
            }
            ObjectsClass.db.SaveChanges();
        }
    }
}
