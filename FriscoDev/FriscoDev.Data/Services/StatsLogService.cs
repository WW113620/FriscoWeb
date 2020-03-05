using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Dapper;
using FriscoDev.Application.ViewModels;

namespace FriscoDev.Data.Services
{
    public class StatsLogService: IStatsLogService
    {
        public IEnumerable<StatsLogVm> GetStatsLogsToRealTimeCharts(int pid, string StartDate, string EndDate, string timeZone)
        {
            DateTime dStart = Convert.ToDateTime(StartDate);
            DateTime dEnd = Convert.ToDateTime(EndDate);
            const string sql = @"select [target id] as TargetId,[PMD ID] as PMDID,* from [StatsLog] where [PMD ID] = @Pid and [timestamp] between @Start and @End  order by [Timestamp]";
            return ExecuteDapper.QueryList<StatsLogVm>(sql, new
            {
                Pid = pid,
                Start = dStart,
                End = dEnd
            });
        }
        public IEnumerable<SpeedCount> GetSpeedCountFromStatsLogs(int pid, string StartDate, string EndDate, string timeZone, out double aveg)
        {
            string sql = @"WITH t AS 
                            (SELECT top 100 percent Direction,AverageSpeed,count(1)as iCount 
                            from StatsLog
                            where [PMD ID]=@Pid and [timestamp] between @Start and @End
                            group by Direction,AverageSpeed
                            order by AverageSpeed) 
                            SELECT AverageSpeed,ISNULL(AWAY,0) as AwayCount,ISNULL(CLOS,0) as ClosCount 
                            FROM t pivot( sum(iCount) FOR Direction IN (CLOS,AWAY)) m ";
            string sqlAvg = @" select SUM(AverageSpeed)/Count(0) as AvgSpeed from [StatsLog]";
            DateTime dStart = Convert.ToDateTime(StartDate);
            DateTime dEnd = Convert.ToDateTime(EndDate);

            var AvgSpeed = ExecuteDapper.QueryList<AverageSpeed>(sqlAvg).FirstOrDefault();
            aveg = AvgSpeed.AvgSpeed;
            return ExecuteDapper.QueryList<SpeedCount>(sql, new
            {
                Pid = pid,
                Start = dStart,
                End = dEnd
            });
        }
        public IEnumerable<CountPercentileEntity> GetSpeedPercentileList(int pid, string StartDate, string EndDate, string timeZone)
        {
            string sql = @" SELECT AverageSpeed,isnull(Count(0),0) as iCount
                            from StatsLog
                            where  [PMD ID] = @Pid and[timestamp] between @Start and @End
                            -- where  [PMD ID] = 404685 and[timestamp] between '2017-2-19 00:00' and '2017-2-26 23:59'
                            group by AverageSpeed
                            order by AverageSpeed  ";

            DateTime dStart = Convert.ToDateTime(StartDate);
            DateTime dEnd = Convert.ToDateTime(EndDate);

            return ExecuteDapper.QueryList<CountPercentileEntity>(sql, new
            {
                Pid = pid,
                Start = dStart,
                End = dEnd
            });
        }
        public IEnumerable<StatsLogNew> GetStatsLogListNew(int pid, string StartDate, string EndDate, string timeZone)
        {
            string sql = @"select *,
                        (CASE WHEN SubDate>'00:00:00' and SubDate<='02:30:00' then '02:30:00'
                        WHEN SubDate>'02:30:00' and SubDate<='05:00:00' then '05:00:00'
                        WHEN SubDate>'05:00:00' and SubDate<='07:30:00' then '07:30:00'
                        WHEN SubDate>'07:30:00' and SubDate<='10:00:00' then '10:00:00'
                        WHEN SubDate>'10:00:00' and SubDate<='12:30:00' then '12:30:00'
                        WHEN SubDate>'12:30:00' and SubDate<='15:00:00' then '15:00:00'
                        WHEN SubDate>'15:00:00' and SubDate<='17:30:00' then '17:30:00'
                        WHEN SubDate>'17:30:00' and SubDate<='20:00:00' then '20:00:00'
                        WHEN SubDate>'20:00:00' and SubDate<='22:30:00' then '22:30:00'
                        ELSE '00:00:00' END ) as TimeSplit
                        from 
                        (select *,Datepart(weekday,[timestamp]) as WeekDate,convert(char(8),[timestamp],108) as SubDate
                        from StatsLog
                        where [PMD ID]=@Pid and [timestamp] between @Start and @End
                        --where [PMD ID]=404685 and [timestamp] between '2015-1-20 00:00' and '2017-2-26 23:59'
                        ) s
                        order by TimeSplit ";

            DateTime dStart = Convert.ToDateTime(StartDate);
            DateTime dEnd = Convert.ToDateTime(EndDate);
            return ExecuteDapper.QueryList<StatsLogNew>(sql, new
            {
                Pid = pid,
                Start = dStart,
                End = dEnd
            });
        }
        public IEnumerable<SpeedPercentileEntity> GetWeeklyCountTimeList(int pid, string StartDate, string EndDate, string timeZone)
        {
            string sql = @"with t as (
                        select top 100 percent *,
                        (CASE WHEN subhour>'00:00:00' and subhour<='02:30:00' then '02:30:00'
                         WHEN subhour>'02:30:00' and subhour<='05:00:00' then '05:00:00'
                         WHEN subhour>'05:00:00' and subhour<='07:30:00' then '07:30:00'
                         WHEN subhour>'07:30:00' and subhour<='10:00:00' then '10:00:00'
                         WHEN subhour>'10:00:00' and subhour<='12:30:00' then '12:30:00'
                         WHEN subhour>'12:30:00' and subhour<='15:00:00' then '15:00:00'
                         WHEN subhour>'15:00:00' and subhour<='17:30:00' then '17:30:00'
                         WHEN subhour>'17:30:00' and subhour<='20:00:00' then '20:00:00'
                         WHEN subhour>'20:00:00' and subhour<='22:30:00' then '22:30:00'
                         ELSE '00:00:00' END ) as TimeSplit
                         from 
                        (select *,Datepart(weekday,[timestamp]) as weekdate,convert(char(8),[timestamp],108) as subhour
                        from StatsLog
                        where [PMD ID]=@Pid and [timestamp] between @Start and @End
                        ) s)
                        select TimeSplit,ISNULL([1],0) as 'SundayCount',ISNULL([2],0) as 'MondayCount',ISNULL([3],0) as 'TuesdayCount',
                        ISNULL([4],0) as 'WednesdayCount',ISNULL([5],0) as 'ThursdayCount',ISNULL([6],0) as 'FridayCount',ISNULL([7],0) as 'SaturdayCount'
                         from  (select top 100 percent TimeSplit,weekdate,Count(0) as iCount from t
                        group by TimeSplit,weekdate ) m
                        pivot( sum(iCount) FOR weekdate IN ([1],[2],[3],[4],[5],[6],[7])) k  order by TimeSplit
                         ";

            DateTime dStart = Convert.ToDateTime(StartDate);
            DateTime dEnd = Convert.ToDateTime(EndDate);
            return ExecuteDapper.QueryList<SpeedPercentileEntity>(sql, new
            {
                Pid = pid,
                Start = dStart,
                End = dEnd
            });
        }
        public IEnumerable<SurveyDataEntity> GetSurveyDataList(int pid, string StartDate, string EndDate, string timeZone)
        {
            #region sql     
            string sql = @" with t as (
                        select top 100 percent weekdate,Direction,min(subhour) as mintime,MAX(subhour) as maxtime from 
                        (
                        select *,Datepart(weekday,[timestamp]) as weekdate,convert(char(8),[timestamp],108) as subhour
                        from StatsLog where  [PMD ID]=@Pid and [timestamp] between @Start and @End
                        ) a
                        group by weekdate,Direction
                        order by weekdate)
                        SELECT t.weekdate as weeddate,
                         (SELECT mintime FROM t as a WHERE a.weekdate=t.weekdate AND a.Direction='AWAY ')AS 'aStartDate',
                        (SELECT maxtime FROM t as c WHERE c.weekdate=t.weekdate AND c.Direction='AWAY ')AS 'aEndDate',
                        (SELECT mintime FROM t as b WHERE b.weekdate=t.weekdate AND b.Direction='CLOS ')AS 'cStartDate',
                        (SELECT maxtime FROM t as d WHERE d.weekdate=t.weekdate AND d.Direction='CLOS ')AS 'cEndDate'
                        FROM t group by t.weekdate";
            #endregion

            DateTime dStart = Convert.ToDateTime(StartDate);
            DateTime dEnd = Convert.ToDateTime(EndDate);

            return ExecuteDapper.QueryList<SurveyDataEntity>(sql, new
            {
                Pid = pid,
                Start = dStart,
                End = dEnd
            });
        }
        public IEnumerable<StatsLogVm> GetStatsLogsToDataLog(string startTime, string endTime, int pageIndex, int pageSize, int pid, string timeZone, out long iCount)
        {
            const string sql = @"select t.* from (
                                select ROW_NUMBER() OVER (ORDER BY [timestamp] DESC) AS [RowNumber],[target id] as TargetId,[PMD ID] as PMDID,* from [StatsLog] 
                                where [PMD ID] = @Pid and [timestamp] between @Start and @End
                                ) as t
                                where @PageSize * (@CurrentPage - 1) < RowNumber AND RowNumber <= @PageSize * @CurrentPage ";
            const string sqlCount = @"select count(0) as iCount from [StatsLog] 
                                       where [PMD ID] = @Pid and [timestamp] between @Start and @End ";
            iCount = ExecuteDapper.QueryList<int>(sqlCount, new
            {
                Pid = pid,
                Start = startTime,
                End = endTime
            }).FirstOrDefault();
            return ExecuteDapper.QueryList<StatsLogVm>(sql, new
            {
                Pid = pid,
                Start = startTime,
                End = endTime,
                CurrentPage = pageIndex,
                PageSize = pageSize
            });
        }
        public IEnumerable<StatsLogVm> GetStatsLogsToReport(int pid, string StartDate, string EndDate, string timeZone)
        {
            const string sql = @"with t as(
            select *, 
            SUBSTRING( CONVERT(varchar,[Timestamp],120),0,14)+':00' as StatDate,
            cast(datename(Minute, [Timestamp]) as int) Minss
             from StatsLog 
            where [PMD ID] = @Pid and [timestamp] between @Start and @End
             )
             select  [target id] as TargetId,[PMD ID] as PMDID, *,
             SUBSTRING( CONVERT(varchar,[Timestamp],120),0,14)+':'+(CASE WHEN  Minss >0 and Minss <15  THEN '00'
            WHEN Minss >=15 and Minss <30  THEN '15'
            WHEN Minss >=30 and Minss <45  THEN '30'
            ELSE '45' END) StatDate2 from t

            ";
            //DateTime dStart = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(StartDate), TimeZoneInfo.FindSystemTimeZoneById(timeZone));
            //DateTime dEnd = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(EndDate), TimeZoneInfo.FindSystemTimeZoneById(timeZone));
            DateTime dStart = Convert.ToDateTime(StartDate);
            DateTime dEnd = Convert.ToDateTime(EndDate);

            return ExecuteDapper.QueryList<StatsLogVm>(sql, new
            {
                Pid = pid,
                Start = dStart,
                End = dEnd
            });
        }
    }
}
