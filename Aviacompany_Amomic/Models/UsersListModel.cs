using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace Aviacompany_Amomic.Models
{
    public class UsersListModel
    {
        public int? GetAge(DateTime? dateBirthday)
        {
            return DateTime.Now.Year - dateBirthday?.Year;
        }
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public string Age { get; set; }
        public string UserRole { get; set; }
        public string Email { get; set; }
        public string Office { get; set; }
        public bool Active { get; set; }
        public string ColorRow { get; set; }
        public UsersListModel(
            int iD,
            DateTime? birthday,
            string firstName,
            string lastName,
            string userRole,
            string email,
            string office,
            bool? active)
        {
            ID = iD;
            Age = GetAge(birthday).ToString();
            FirstName = firstName;
            LastName = lastName;
            UserRole = userRole;
            Email = email;
            Office = office;
            if(active == false)
                ColorRow = "State1";
            else
            {
                if(userRole == "Administrator")
                    ColorRow = "State2";
            }
        }
    }
}
