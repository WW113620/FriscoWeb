using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using FriscoDev.Application.ViewModels;

namespace FriscoDev.Data.Services
{
    public interface IStatsLogService: IDependency
    {
        IEnumerable<StatsLogVm> GetStatsLogsToRealTimeCharts(int pid, string StartDate, string EndDate, string timeZone);

        IEnumerable<SpeedCount> GetSpeedCountFromStatsLogs(int pid, string StartDate, string EndDate, string timeZone,
            out double aveg);

        IEnumerable<CountPercentileEntity> GetSpeedPercentileList(int pid, string StartDate, string EndDate,
            string timeZone);

        IEnumerable<StatsLogNew> GetStatsLogListNew(int pid, string StartDate, string EndDate, string timeZone);

        IEnumerable<SpeedPercentileEntity> GetWeeklyCountTimeList(int pid, string StartDate, string EndDate,
            string timeZone);

        IEnumerable<SurveyDataEntity> GetSurveyDataList(int pid, string StartDate, string EndDate, string timeZone);

        IEnumerable<StatsLogVm> GetStatsLogsToDataLog(string startTime, string endTime, int pageIndex, int pageSize,
            int pid, string timeZone, out long iCount);

        IEnumerable<StatsLogVm> GetStatsLogsToReport(int pid, string StartDate, string EndDate, string timeZone);
    }
}
