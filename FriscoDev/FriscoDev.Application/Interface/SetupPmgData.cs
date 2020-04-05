using Application.Common;
using FriscoDev.Application.Models;
using FriscoDev.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FriscoDev.Application.Interface.PacketProtocol;

namespace FriscoDev.Application.Interface
{
    public static class SetupPmgData
    {
        public static List<PMGConfiguration> ToConfigurations(this ConfigurationModel model)
        {
            List<PMGConfiguration> paramList = new List<PMGConfiguration>();

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
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.IdleDisplayPage, model.pageTypeIdle.ToEmptyString(), 1));

            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.SpeedLimit, model.limitSpeed.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.SpeedLimitDisplay, model.actionTypeLimit.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.SpeedLimitDisplayPage, model.pageTypeLimit.ToEmptyString(), 1));
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.SpeedLimitAlertAction, model.alertActionLimit.ToString(), 1));

            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.AlertLimit, model.alertSpeed.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.AlertLimitDisplay, model.actionTypeAlert.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.AlertLimitDisplayPage, model.pageTypeAlert.ToEmptyString(), 1));
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.AlertLimitAlertAction, model.alertActionAlert.ToString(), 1));

            return paramList;
        }

        public static List<PMGConfiguration> ToTrafficData(this TrafficDataViewModel model)
        {
            List<PMGConfiguration> paramList = new List<PMGConfiguration>();

            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.TrafficEnableRecording, model.TrafficEnableRecording.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.TrafficTargetStrength, model.TrafficTargetStrength.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.TrafficMinimumTrackingDistance, model.TrafficMinimumTrackingDistance.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.TrafficMinimumFollowingTime, model.TrafficMinimumFollowingTime.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.TrafficDataOnDemand, model.TrafficDataOnDemand.ToString(), 1));
          
            return paramList;
        }

    }
}
