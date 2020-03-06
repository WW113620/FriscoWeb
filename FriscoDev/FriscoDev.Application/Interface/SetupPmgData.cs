using Application.Common;
using FriscoDev.Application.Models;
using FriscoDev.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FriscoDev.Application.Interface.PMGDataPacketProtocol;

namespace FriscoDev.Application.Interface
{
    public static class SetupPmgData
    {
        public static List<PMGConfiguration> ToConfigurations(this ConfigurationModel model)
        {
            List<PMGConfiguration> paramList = new List<PMGConfiguration>();

            DateTime date = Convert.ToDateTime(model.date + " " + model.time);

            DateTime pmgClock = new DateTime(date.Year, date.Month, date.Day,
                             date.Hour, date.Minute, date.Second, DateTimeKind.Local);

            DateTime usCentralDateTime = getUSCentralDateTime();
            string diffValue = (usCentralDateTime - pmgClock).TotalSeconds.ToString();

            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.Clock, diffValue, 1));
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.MinLimit, model.minSpeed.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.MaxLimit, model.maxSpeed.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.SpeedUnit, model.speedUnit, 1));
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.TemperatureUnit, model.temperatureUnit, 1));
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.Brightness, model.numBright.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.EnableMUTCDCompliance, model.mutcd.ToEmptyString(), 1));

            return paramList;
        }

        public static List<PMGConfiguration> ToQuickSetup(this QuickSetupModel model)
        {
            List<PMGConfiguration> paramList = new List<PMGConfiguration>();

            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.IdleDisplay, model.actionTypeIdle.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.IdleDisplayPage,model.pageTypeIdle, 1));

            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.SpeedLimit, model.limitSpeed.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.SpeedLimitDisplay, model.actionTypeLimit.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.SpeedLimitDisplayPage, model.pageTypeLimit, 1));
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.SpeedLimitAlertAction, model.alertActionLimit.ToString(), 1));

            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.AlertLimit, model.alertSpeed.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.AlertLimitDisplay, model.actionTypeAlert.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.AlertLimitDisplayPage, model.pageTypeAlert, 1));
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.AlertLimitAlertAction, model.alertActionAlert.ToString(), 1));

            return paramList;
        }


        public static DateTime getSystemDate(string value)
        {
            double totalSeconds = Convert.ToDouble(value);
            return DateTime.Now;
        }

        public static DateTime getUSCentralDateTime()
        {
            try
            {
                return System.TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, System.TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
            }
            catch (Exception ex)
            {
                return DateTime.UtcNow;
            }
        }

    }
}
