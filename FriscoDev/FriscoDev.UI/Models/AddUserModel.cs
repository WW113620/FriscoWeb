using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace FriscoDev.UI.Models
{
    public class AddUserModel
    {
        [DisplayName("UserName")]
        public string RealName { get; set; }
        [DisplayName("UserEmail")]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }

        public int SelectedUserType { get; set; }
        public int Id { get; set; }
        public string SiteName { get; set; }
        public string ProfileImgUrl { get; set; }
        public string Login_UR_ID { get; set; }
        public string Default_Location { get; set; }
        public string TimeZone { get; set; }
        public string Organization { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CountryName { get; set; }
        public string Address { get; set; }
    }
}