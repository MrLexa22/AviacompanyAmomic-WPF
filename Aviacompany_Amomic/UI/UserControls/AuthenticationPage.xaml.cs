using Aviacompany_Amomic.UI.UserControls.Administrator;
using Aviacompany_Amomic.UI.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Aviacompany_Amomic.UserControls
{
    /// <summary>
    /// Логика взаимодействия для AuthenticationPage.xaml
    /// </summary>
    public partial class AuthenticationPage : System.Windows.Controls.Page
    {
        int count_login = 0;
        DispatcherTimer _timer;
        TimeSpan _time;
        public AuthenticationPage()
        {
            InitializeComponent();
            ObjectsClass.ID_User_Auth = 0;
            ObjectsClass.window_main.Title = "Login";
        }
        public void StartTimer()
        {
            _time = TimeSpan.FromSeconds(10);
            tb_time_text.Visibility = Visibility.Visible;
            btn_login.IsEnabled = false;
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                timer_tb.Text = _time.ToString("c");
                if (_time == TimeSpan.Zero)
                {
                    _timer.Stop();
                    tb_time_text.Visibility = Visibility.Hidden;
                    btn_login.IsEnabled = true;
                    count_login = 0;
                }
                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);
            _timer.Start();
        }
        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            if(tb_login.Text == "")
            {
                ObjectsClass.ShwoMessage("Login error", "Please enter login", ObjectsClass.window_main);
                return;
            }
            if (pb_password.Password == "")
            {
                ObjectsClass.ShwoMessage("Password error", "Please enter password", ObjectsClass.window_main);
                return;
            }

            var user_list = ObjectsClass.db.Users.Where(p => p.Email == tb_login.Text && p.Password == pb_password.Password).ToList();
            if(user_list.Count() == 0)
            {
                count_login += 1;
                if (count_login == 3)
                {
                    ObjectsClass.ShwoMessage("User not found", "User with entered username and password not found", ObjectsClass.window_main);
                    StartTimer();
                    return;
                }
                else
                {
                    ObjectsClass.ShwoMessage("User not found", "User with entered username and password not found", ObjectsClass.window_main);
                }
                return;
            }
            var user = user_list[0];
            if(user.Active == false)
            {
                ObjectsClass.ShwoMessage("User is blockes", "User with entered username is blocked. Please contact with administrator", ObjectsClass.window_main);
                return;
            }

            ObjectsClass.ID_User_Auth = user.ID;
            if(user.LogsUsers.Where(p=>p.WasCrashAndReason == null && p.DateTime_Exit == null).Count() > 0)
            {
                NoLogoutDetected noLogoutDetected = new NoLogoutDetected(user);
                noLogoutDetected.Owner = ObjectsClass.window_main;
                noLogoutDetected.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                noLogoutDetected.ShowDialog();
            }

            LogsUsers logs = new LogsUsers();
            logs.User_ID = user.ID;
            logs.DateTime_Login = DateTime.Now;
            ObjectsClass.db.LogsUsers.Add(logs);
            ObjectsClass.db.SaveChanges();

            ObjectsClass.session_id = logs.ID_Log;

            ObjectsClass.window_main.Title = "AMONIC Airlines Automation System";
            if (user.RoleID == 1)
                ObjectsClass.window_main.main_frame.Content = new AdministratorPage();
            if (user.RoleID == 2)
                ObjectsClass.window_main.main_frame.Content = new UserPage();
        }
        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            ObjectsClass.window_main.Close();
        }
    }
}
