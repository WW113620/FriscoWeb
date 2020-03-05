using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class UserModel
    {
        public string UR_ID { get; set; }
        public string UR_Name { get; set; }
        public string UR_PASSWD { get; set; }
        public string UR_RealName { get; set; }
        public string CS_ID { get; set; }
        public string LN_ID { get; set; }
        public int UR_TYPE_ID { get; set; }
        public int UR_ACTIVE { get; set; }
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
