using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;

namespace FriscoDev.Data.Services
{
    public interface ITimeZoneInfoService: IDependency
    {
        List<Application.Models.TimeZoneInfo> GeTimeZoneInfoList();
        Application.Models.TimeZoneInfo GeTimeZoneInfo(Func<Application.Models.TimeZoneInfo, bool> filter);
    }
}
