using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FriscoDev.Application.Models;

namespace FriscoDev.Data.Services
{
    public class TimeZoneInfoService: ITimeZoneInfoService
    {
        public List<Application.Models.TimeZoneInfo> GeTimeZoneInfoList()
        {
            using (var db=new PMGDATABASEEntities())
            {
                return db.TimeZoneInfo.ToList();
            }
        }
        public Application.Models.TimeZoneInfo GeTimeZoneInfo(Func<Application.Models.TimeZoneInfo, bool> filter)
        {
            using (var db=new PMGDATABASEEntities())
            {
                return db.TimeZoneInfo.FirstOrDefault(filter);
            }
        }
    }
}
