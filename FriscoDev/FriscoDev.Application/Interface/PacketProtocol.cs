using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FriscoDev.Application.Interface.PacketProtocol;

namespace FriscoDev.Application.Interface
{
    public class PacketProtocol
    {

        public static int GetPMDDisplaySize(string pageName)
        {
            if (string.IsNullOrEmpty(pageName))
                return 12;

            string ext = Path.GetExtension(pageName);

            if (ext.Contains("15"))
                return 15;
            if (ext.Contains("18"))
                return 18;
            else
                return 12;

        }

        public static int byte2Int(byte b)
        {
            return (int)(b & 0xff);
        }

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

       
    }

   

}
