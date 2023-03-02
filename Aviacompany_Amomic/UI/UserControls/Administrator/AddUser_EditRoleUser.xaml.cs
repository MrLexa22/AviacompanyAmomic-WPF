using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Aviacompany_Amomic.UI.UserControls.Administrator
{
    /// <summary>
    /// Логика взаимодействия для AddUser_EditRoleUser.xaml
    /// </summary>
    public partial class AddUser_EditRoleUser : Window
    {
        public int ID_User = 0;
        public AddUser_EditRoleUser(int ID_User, int type_window)
        {
            InitializeComponent();
            this.ID_User = ID_User;
            if (type_window == 0)
            {
                this.Title = "Edit role";
                en_dis_inputs(false);
            }
            else
            {
                dp_dateBirthday.SelectedDate = DateTime.Now;
                this.Title = "Add user";
                en_dis_inputs(true);
            }

            var offices_db = ObjectsClass.db.Offices.ToList();
            cb_offices.ItemsSource = offices_db;

            if (type_window == 0)
            {
                var user = ObjectsClass.db.Users.Where(p => p.ID == ID_User).First();
                tb_email.Text = user.Email;
                tb_firstname.Text = user.FirstName;
                tb_lastname.Text = user.LastName;
                pb_password.Password = user.Password;
                dp_dateBirthday.SelectedDate = user.Birthdate;

                cb_offices.SelectedValue = user.OfficeID;
                cb_offices.SelectedValuePath = user.OfficeID.ToString();
                cb_offices.SelectedIndex = offices_db.FindIndex(p=>p.ID == user.OfficeID);

                cb_offices.SelectedValue = user.RoleID;
                if (user.RoleID == 1)
                    rb_admin.IsChecked = true;
                else
                    rb_user.IsChecked = true;
            }
        }
        public void en_dis_inputs(bool type)
        {
            tb_email.IsEnabled = type;
            tb_firstname.IsEnabled = type;
            tb_lastname.IsEnabled = type;
            cb_offices.IsEnabled = type;
            if (type == false)
                grid_editRole.Visibility = Visibility.Visible;
            else
               grid_addUser.Visibility = Visibility.Visible;
        }

        private void btn_cance_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public bool IsValidEmailAddress(string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if(ID_User > 0)
            {
                var user = ObjectsClass.db.Users.Where(p => p.ID == ID_User).First();
                if (rb_admin.IsChecked == true)
                    user.RoleID = 1;
                else
                    user.RoleID = 2;
                ObjectsClass.db.Users.AddOrUpdate(user);
                ObjectsClass.db.SaveChanges();
                ObjectsClass.ShwoMessage("Sucsessfully update role", "Sucsessfully update role selected user", this);
                this.Close();
            }
            else
            {
                if(IsValidEmailAddress(tb_email.Text.Trim()) == false)
                {
                    ObjectsClass.ShwoMessage("Email error", "Entered email not valid", this);
                    return;
                }

                if(ObjectsClass.db.Users.Where(p=>p.Email == tb_email.Text.Trim()).Count() > 0)
                {
                    ObjectsClass.ShwoMessage("Email error", "Entered email already exist", this);
                    return;
                }

                if (tb_firstname.Text.Trim() == "")
                {
                    ObjectsClass.ShwoMessage("First name error", "Entered first name not valid", this);
                    return;
                }
                if (tb_firstname.Text.Trim().Length < 3 || tb_firstname.Text.Trim().Length > 50)
                {
                    ObjectsClass.ShwoMessage("First name error", "Entered first name not valid", this);
                    return;
                }

                if (tb_lastname.Text.Trim() == "")
                {
                    ObjectsClass.ShwoMessage("Last name error", "Entered last name not valid", this);
                    return;
                }
                if (tb_lastname.Text.Trim().Length < 3 || tb_firstname.Text.Trim().Length > 50)
                {
                    ObjectsClass.ShwoMessage("Last name error", "Entered last name not valid", this);
                    return;
                }

                if(cb_offices.SelectedValue == null)
                {
                    ObjectsClass.ShwoMessage("Office error", "Selected office not valid", this);
                    return;
                }

                if (dp_dateBirthday.DisplayDate >= DateTime.Now.AddDays(-1))
                {
                    ObjectsClass.ShwoMessage("Date of birthday error", "Selected date of birthday not valid", this);
                    return;
                }

                if(pb_password.Password.Length < 4)
                {
                    ObjectsClass.ShwoMessage("Password error", "Entered password not valid", this);
                    return;
                }

                tb_firstname.Text = tb_firstname.Text[0].ToString().ToUpper() + tb_firstname.Text.Remove(0, 1);
                tb_lastname.Text = tb_lastname.Text[0].ToString().ToUpper() + tb_lastname.Text.Remove(0, 1);

                Users user = new Users();
                user.OfficeID = Convert.ToInt16(cb_offices.SelectedValue);
                user.RoleID = 2;
                user.Email = tb_email.Text.Trim();
                user.Active = true;
                user.Birthdate = dp_dateBirthday.SelectedDate;
                user.FirstName = tb_firstname.Text.Trim();
                user.LastName = tb_lastname.Text.Trim();
                user.Password = pb_password.Password;
                ObjectsClass.db.Users.Add(user);
                ObjectsClass.db.SaveChanges();
                ObjectsClass.ShwoMessage("Sucsessfully add user", "Sucsessfully adding user", this);
                this.Close();
            }
        }
    }
}
