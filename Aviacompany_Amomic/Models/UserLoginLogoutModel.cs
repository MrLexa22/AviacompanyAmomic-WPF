using System;
using System.Collections.Generic;
using System.Data.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Aviacompany_Amomic.Models
{
    public class UserLoginLogoutModel
    {
        public string login_date { get; set; }
        public string login_time { get; set; }
        public string logout_time { get; set; }
        public string time_spent { get; set; }
        public string reason { get; set; }
        public string ColorRow { get; set; }

        public UserLoginLogoutModel(
            DateTime login_datetime,
            DateTime? logout_datetime,
            string reason_inp)
        {
            login_date = login_datetime.Date.ToShortDateString();
            login_time = login_datetime.ToShortTimeString();
            if(logout_datetime != null )
            {
                logout_time = logout_datetime?.ToShortTimeString();
                TimeSpan? ts = logout_datetime - login_datetime;
                time_spent = ts?.Hours.ToString()+":"+ ts?.Minutes.ToString()+":"+ts?.Seconds.ToString();
            }
            else
            {
                logout_time = "**";
                time_spent = "**";
                reason = reason_inp;
                ColorRow = "State1";
            }
        }
    }
}
