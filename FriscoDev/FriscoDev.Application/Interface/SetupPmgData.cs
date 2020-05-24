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

        public static List<PMGConfiguration> ToRadarData(this RadarViewModel model)
        {
            List<PMGConfiguration> paramList = new List<PMGConfiguration>();

            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.Radar, model.Radar.ToString(), 1));
            if (model.Radar == 1)
            {
                paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.RadarHoldoverTime, model.RadarHoldoverTime.ToString(), 1));
                paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.RadarCosine, model.RadarCosine.ToString(), 1));
                paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.RadarUnitResolution, model.RadarUnitResolution.ToString(), 1));
                paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.RadarSensitivity, model.RadarSensitivity.ToString(), 1));

                paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.RadarTargetStrength, model.RadarTargetStrength.ToString(), 1));
                paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.RadarTargetAcceptance, model.RadarTargetAcceptance.ToString(), 1));
                paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.RadarTargetHoldOn, model.RadarTargetHoldOn.ToString(), 1));
                paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.RadarOperationDirection, model.RadarOperationDirection.ToString(), 1));
            }
            else
            {
                paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.RadarExternalRadarSpeed, model.RadarExternalRadarSpeed.ToString(), 1));
                paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.RadarExternalEchoPanRadarData, model.RadarExternalEchoPanRadarData.ToString(), 1));
            }

            return paramList;
        }


        public static List<PMGConfiguration> ToCommunication(this CommunicationViewModel model)
        {
            List<PMGConfiguration> paramList = new List<PMGConfiguration>();

            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.WirelessPIN, model.WirelessPIN, 1));
            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.EthernetIPSetting, model.EthernetIPSetting.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.EthernetIPAddress, model.EthernetIPAddress, 1));
            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.EthernetSubnetMask, model.EthernetSubnetMask, 1));
            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.EthernetDefaultGateway, model.EthernetDefaultGateway, 1));

            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.WifiMode, model.WiFIMode.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.WifiAccessPointSecurity, model.WIFIAccessPointSecurity.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.WifiAccessPointPassword, model.WIFIAccessPointPassword, 1));
            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.WifiStationSecurity, model.WIFIStationSecurity.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.WifiStationPassword, model.WIFIStationPassword, 1));

            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.WifiStationSSID, model.WIFIStationSSID, 1));
            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.WifiStationIPType, model.WIFIStationIPType.ToString(), 1));
            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.WifiStationIPAddress, model.WIFIStationIPAddress, 1));
            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.WifiStationSubnetMask, model.WIFIStationSubnetMask, 1));
            paramList.Add(new PMGConfiguration(model.PMGID, ParamaterId.WifiStationDefaultGateway, model.WIFIStationDefaultGateway, 1));

            return paramList;
        }

    }
}
