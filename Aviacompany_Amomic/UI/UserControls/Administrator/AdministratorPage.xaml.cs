using Aviacompany_Amomic.Models;
using Aviacompany_Amomic.UI.Windows;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Aviacompany_Amomic.UI.UserControls.Administrator
{
    /// <summary>
    /// Логика взаимодействия для AdministratorPage.xaml
    /// </summary>
   
    public partial class AdministratorPage : System.Windows.Controls.Page
    {
        public AdministratorPage()
        {
            InitializeComponent();
            getListOffices();
            getListUsers(0);
        }
        public void getListOffices()
        {
            List<Offices> offices_list = new List<Offices>();
            Offices office = new Offices();
            office.Title = "All offices";
            office.ID = 0;
            offices_list = new List<Offices>();
            offices_list.Add(office);

            var offices_db = ObjectsClass.db.Offices.ToList();
            foreach (var a in offices_db)
            {
                offices_list.Add(a);
            }
            cb_offices.ItemsSource = offices_list;
            cb_offices.SelectedValue = 0;
            cb_offices.SelectedValuePath = "0";
            cb_offices.SelectedIndex = 0;
        }
        public void getListUsers(int office_ID)
        {
            var users_list = ObjectsClass.db.Users.Include("Offices").Include("Roles").ToList();
            if (office_ID != 0)
                users_list = users_list.Where(p => p.OfficeID == office_ID).ToList();
            List<UsersListModel> users = new List<UsersListModel>();
            users = users_list.Select(p=> new UsersListModel(p.ID,p.Birthdate,p.FirstName,p.LastName,p.Roles.Title,p.Email,p.Offices.Title,p.Active)).ToList();
            datagrid_users.ItemsSource = users;
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
        private void cb_offices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_offices.SelectedValue != null)
            {
                getListUsers(Convert.ToInt16(cb_offices.SelectedValue));
            }
        }
        private void btn_enable_disbale_login_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_users.SelectedValue != null)
            {
                int idUser = Convert.ToInt16(datagrid_users.SelectedValue);
                var user = ObjectsClass.db.Users.Where(p => p.ID == idUser).First();
                user.Active = !user.Active;
                ObjectsClass.db.Users.AddOrUpdate(user);
                getListUsers(Convert.ToInt16(cb_offices.SelectedValue));
                ObjectsClass.ShwoMessage("User sucssfully updates", "Status update and now it's '" + (user.Active == true ? "Enabled" + "'" : "Disabled" + "'"), ObjectsClass.window_main);
            }
        }
        private void datagrid_users_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(datagrid_users.SelectedValue != null)
            {
                btn_change_role.IsEnabled = true;
                btn_enable_disbale_login.IsEnabled = true;
            }
            else
            {
                btn_change_role.IsEnabled = false;
                btn_enable_disbale_login.IsEnabled = false;
            }
        }

        private void btn_change_role_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_users.SelectedValue != null)
            {
                AddUser_EditRoleUser window_add_role = new AddUser_EditRoleUser(Convert.ToInt16(datagrid_users.SelectedValue),0);
                window_add_role.Owner = ObjectsClass.window_main;
                window_add_role.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                window_add_role.ShowDialog();
                getListUsers(Convert.ToInt16(cb_offices.SelectedValue));
            }
        }

        private void btn_add_user_Click(object sender, RoutedEventArgs e)
        {
            AddUser_EditRoleUser window_add_role = new AddUser_EditRoleUser(0, 1);
            window_add_role.Owner = ObjectsClass.window_main;
            window_add_role.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            window_add_role.ShowDialog();
            getListUsers(Convert.ToInt16(cb_offices.SelectedValue));
        }
    }
}
