using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class AccountViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string UserType { get; set; }
        public int HideUserType { get; set; }
        public string Customer { get; set; }
        public string Active { get; set; }
        public int ActiveType { get; set; }
        public string IsAdmin { get; set; }
        public string ImgUrl { get; set; }
        public string TimeZone { get; set; }
        public string SiteName { get; set; }
        public string ProfileImgUrl { get; set; }
        public string LastLoginTime { get; set; }

    }
}
