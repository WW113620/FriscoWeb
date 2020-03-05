using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FriscoDev.Application.Interface.PMGDataPacketProtocol;

namespace FriscoDev.Application.Interface
{
    public class PMGDataPacketProtocol
    {

        public enum ParamaterId
        {
            Unknown = 0,

            IdleDisplay = 1,
            IdleDisplayPage,

            SpeedLimit,
            SpeedLimitDisplay,
            SpeedLimitDisplayPage,
            SpeedLimitAlertAction,

            AlertLimit,
            AlertLimitDisplay,
            AlertLimitDisplayPage,
            AlertLimitAlertAction,

            MinLimit,
            MaxLimit,
            SpeedUnit,
            TemperatureUnit,
            Brightness,
            EnableMUTCDCompliance,

            Radar,
            RadarHoldoverTime,
            RadarCosine,
            RadarUnitResolution,
            RadarSensitivity,
            RadarTargetStrength,
            RadarTargetAcceptance,
            RadarTargetHoldOn,
            RadarOperationDirection,
            RadarExternalRadarSpeed,
            RadarExternalEchoPanRadarData,

            TrafficEnableRecording,
            TrafficTargetStrength,
            TrafficMinimumTrackingDistance,
            TrafficMinimumFollowingTime,
            TrafficDataOnDemand,

            WirelessPIN,
            EthernetIPSetting,
            EthernetIPAddress,
            EthernetSubnetMask,
            EthernetDefaultGateway,

            WifiMode,
            WifiAccessPointSecurity,
            WifiAccessPointPassword,
            WifiStationSecurity,
            WifiStationPassword,
            WifiStationSSID,
            WifiStationIPType,
            WifiStationIPAddress,
            WifiStationSubnetMask,
            WifiStationDefaultGateway,
            Clock
        };

        public List<ParamConfigureEntry> getParamConfigureEntryList()
        {
            List<ParamConfigureEntry> paramList = new List<ParamConfigureEntry>();

            // Idle
            /* paramList.Add(new ParamConfigureEntry(ParamaterId.IdleDisplay, (uint)Std_Ops.Idle_Action.primary));
             paramList.Add(new ParamConfigureEntry(ParamaterId.IdleDisplayPage, Std_Ops.Idle_Action.getFilename()));

             // Limit Display
             paramList.Add(new ParamConfigureEntry(ParamaterId.SpeedLimit, (uint)Std_Ops.Speed_Limit));
             paramList.Add(new ParamConfigureEntry(ParamaterId.SpeedLimitDisplay, (uint)Std_Ops.Limit_Action.primary));
             paramList.Add(new ParamConfigureEntry(ParamaterId.SpeedLimitDisplayPage, Std_Ops.Limit_Action.getFilename()));
             paramList.Add(new ParamConfigureEntry(ParamaterId.SpeedLimitAlertAction, (uint)Std_Ops.Limit_Action.alert));

             // Alert Display
             paramList.Add(new ParamConfigureEntry(ParamaterId.AlertLimit, (uint)Std_Ops.Alert_Speed));
             paramList.Add(new ParamConfigureEntry(ParamaterId.AlertLimitDisplay, (uint)Std_Ops.Alert_Action.primary));
             paramList.Add(new ParamConfigureEntry(ParamaterId.AlertLimitDisplayPage, Std_Ops.Alert_Action.getFilename()));
             paramList.Add(new ParamConfigureEntry(ParamaterId.AlertLimitAlertAction, (uint)Std_Ops.Alert_Action.alert));

             // Configuration Tab
             paramList.Add(new ParamConfigureEntry(ParamaterId.MinLimit, (uint)Min_Speed_Threshold));
             paramList.Add(new ParamConfigureEntry(ParamaterId.MaxLimit, (uint)Max_Speed_Threshold));
             paramList.Add(new ParamConfigureEntry(ParamaterId.SpeedUnit, (uint)Speed_Measurement_Unit));
             paramList.Add(new ParamConfigureEntry(ParamaterId.TemperatureUnit, (uint)Temp_Unit));
             paramList.Add(new ParamConfigureEntry(ParamaterId.Brightness, (uint)Manual_Brightness_Level));
             paramList.Add(new ParamConfigureEntry(ParamaterId.EnableMUTCDCompliance, (uint)MUTCD_Compliance_Enable));

             // Radar
             paramList.Add(new ParamConfigureEntry(ParamaterId.Radar, (uint)Radar_Enable));
             paramList.Add(new ParamConfigureEntry(ParamaterId.RadarHoldoverTime, (uint)Radar_Holdover_Time));
             paramList.Add(new ParamConfigureEntry(ParamaterId.RadarCosine, (uint)Radar_Cosine));
             paramList.Add(new ParamConfigureEntry(ParamaterId.RadarUnitResolution, (uint)Speed_Resolution));
             paramList.Add(new ParamConfigureEntry(ParamaterId.RadarSensitivity, (uint)Radar_Sensitivity));
             paramList.Add(new ParamConfigureEntry(ParamaterId.RadarTargetStrength, (uint)Radar_Target_Strength));
             paramList.Add(new ParamConfigureEntry(ParamaterId.RadarTargetAcceptance, (uint)Radar_Target_Acceptance));
             paramList.Add(new ParamConfigureEntry(ParamaterId.RadarTargetHoldOn, (uint)Radar_Target_Hold_On));
             paramList.Add(new ParamConfigureEntry(ParamaterId.RadarOperationDirection, (uint)Radar_Operating_Direction));
             paramList.Add(new ParamConfigureEntry(ParamaterId.RadarExternalRadarSpeed, (uint)External_Radar_Speed));
             paramList.Add(new ParamConfigureEntry(ParamaterId.RadarExternalEchoPanRadarData, (uint)Echo_PAN_Radar_Data));

             // Traffic Data
             paramList.Add(new ParamConfigureEntry(ParamaterId.TrafficEnableRecording, (uint)Stats_Enable));
             paramList.Add(new ParamConfigureEntry(ParamaterId.TrafficTargetStrength, (uint)Stats_Target_Strength));
             paramList.Add(new ParamConfigureEntry(ParamaterId.TrafficMinimumTrackingDistance, (uint)Stats_Min_Tracking_Distance));
             paramList.Add(new ParamConfigureEntry(ParamaterId.TrafficMinimumFollowingTime, (uint)Stats_Min_Following_Time));
             paramList.Add(new ParamConfigureEntry(ParamaterId.TrafficDataOnDemand, (uint)Stats_Offload_Option));

             // Wireless PIN
             paramList.Add(new ParamConfigureEntry(ParamaterId.WirelessPIN, (uint)Bluetooth_PIN));

             // Ethernet
             paramList.Add(new ParamConfigureEntry(ParamaterId.EthernetIPSetting, (uint)Ethernet_Connection_Type));
             paramList.Add(new ParamConfigureEntry(ParamaterId.EthernetIPAddress, (uint)Ethernet_Static_IP));
             paramList.Add(new ParamConfigureEntry(ParamaterId.EthernetSubnetMask, (uint)Ethernet_Static_Subnet_Mask));
             paramList.Add(new ParamConfigureEntry(ParamaterId.EthernetDefaultGateway, (uint)Ethernet_Static_Default_Gateway));

             // WiFI
             paramList.Add(new ParamConfigureEntry(ParamaterId.WifiMode, (uint)WIFI_Mode));
             paramList.Add(new ParamConfigureEntry(ParamaterId.WifiAccessPointSecurity, (uint)WIFI_AP_Security));
             paramList.Add(new ParamConfigureEntry(ParamaterId.WifiAccessPointPassword, Util.GetAsciiString(WIFI_AP_Password)));
             paramList.Add(new ParamConfigureEntry(ParamaterId.WifiStationSecurity, (uint)WIFI_Station_Security));
             paramList.Add(new ParamConfigureEntry(ParamaterId.WifiStationPassword, Util.GetAsciiString(WIFI_Station_Password)));
             paramList.Add(new ParamConfigureEntry(ParamaterId.WifiStationSSID, Util.GetAsciiString(WIFI_Station_SSID)));
             paramList.Add(new ParamConfigureEntry(ParamaterId.WifiStationIPType, (uint)WIFI_Station_IP_Type));
             paramList.Add(new ParamConfigureEntry(ParamaterId.WifiStationIPAddress, (uint)WIFI_Station_Static_IP));
             paramList.Add(new ParamConfigureEntry(ParamaterId.WifiStationSubnetMask, (uint)WIFI_Station_Static_Mask));
             paramList.Add(new ParamConfigureEntry(ParamaterId.WifiStationDefaultGateway, (uint)WIFI_Station_Static_Gateway));
     */

            return paramList;
        }
    }

    public class ParamConfigureEntry
    {
        public int pmgId = 0;
        public ParamaterId paramId = 0;
        public string value = string.Empty;
        public byte state = 0;

        public ParamConfigureEntry() { }
        public ParamConfigureEntry(ParamaterId paramIdIn, string valueIn, int pmgIdIn = 0, byte stateIn = 0)
        {
            paramId = paramIdIn;
            value = valueIn;
            pmgId = pmgIdIn;
            state = stateIn;
        }

        public ParamConfigureEntry(ParamaterId paramIdIn, uint valueIn, int pmgIdIn = 0, byte stateIn = 0)
        {
            paramId = paramIdIn;
            value = valueIn.ToString();
            pmgId = pmgIdIn;
            state = stateIn;
        }

        public uint getUintValue()
        {
            uint valueOut = 0;

            uint.TryParse(value, out valueOut);
            return valueOut;
        }
        public string toString()
        {
            return paramId.ToString() + "= [" + value + "], state=" + state + ", pmgid=" + pmgId;
        }
    }

}
