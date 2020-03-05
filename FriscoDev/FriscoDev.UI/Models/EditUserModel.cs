using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriscoDev.UI.Models
{
    public class EditUserModel
    {
        public string RealName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RepeatNewPassword { get; set; }
        public int SelectedUserType { get; set; }

        public string ImgUrl { get; set; }

        public int UR_TYPE_ID { get; set; }
        public string ShowLoginTime { get; set; }
        public bool ActiveType { get; set; }
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
    public class AccountVm
    {
        public string UR_ID { get; set; }
        public string UR_Name { get; set; }
        public string UR_PASSWD { get; set; }
        public string UR_RealName { get; set; }
        public string CS_ID { get; set; }
        public string LN_ID { get; set; }
        public int UR_TYPE_ID { get; set; }
        public bool UR_ACTIVE { get; set; }
        public DateTime UR_ADDTIME { get; set; }
        public DateTime? UR_UPTIME { get; set; }
        public string UR_STATUS { get; set; }
        public string CS_Name { get; set; }
        public decimal xvalue { get; set; }
        public decimal yvalue { get; set; }
        public string pid { get; set; }
        public int pmdid { get; set; }
        public bool IS_ADMIN { get; set; }
        public string IMG_URL { get; set; }
        public string TIME_ZONE { get; set; }
        public string ZoomLevel { get; set; }
        public int MessageDays { get; set; }
        public string SiteName { get; set; }
        public string ProfileImgUrl { get; set; }
        public string Perent_UR_ID { get; set; }
    }
}