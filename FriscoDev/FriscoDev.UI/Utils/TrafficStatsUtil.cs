using FriscoDev.Application.ViewModels;
using FriscoDev.Data;
using FriscoDev.Data.PMGDataPacketProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriscoDev.UI.Utils
{
    public class TrafficStatsUtil
    {
        public PMGFriscoConnection pmgConnection = null;
        public void TrafficDataStats(TrafficDataModel model)
        {
            //pmgConnection = new PMGFriscoConnection();
            if (model.BtnName.Equals("buttonTrafficStatsRecordStartStop"))
            {
                string text = model.BtnStartText;

                if (text.Equals("Start Download"))
                {
                    pmgConnection.currentStatsStatusOnPMG.statsTransferState =
                         PMGFriscoConnection.StatsStatusInfo.StatsTransferState.Transfering;

                    pmgConnection.currentStatsStatusOnPMG.numStatsTransferred = 0;
                    pmgConnection.currentStatsStatusOnPMG.downloadCanceled = false;
                    pmgConnection.currentStatsStatusOnPMG.downloadStartTime = DateTime.Now;

                    //SetStatusMessage("Downloading Traffic Data from PMG ....! ", 0);

                    text = "Cancel Download";
                    pmgConnection.SendStatsCommand(StatsCommandType.SendStats);
                }
                else if (text.Equals("Cancel Download"))
                {
                    pmgConnection.currentStatsStatusOnPMG.statsTransferState =
                           PMGFriscoConnection.StatsStatusInfo.StatsTransferState.Idle;

                    pmgConnection.currentStatsStatusOnPMG.lastStatsReceviedMsgSentCount = -1;
                    pmgConnection.currentStatsStatusOnPMG.downloadCanceled = true;
                    pmgConnection.CloseAndDeleteStatsFileWriter();

                    // Remove any unprocessed stats messages
                    pmgConnection.ResetStatsRecordsQueue();

                    //SetStatusMessage("Download Canceled!", 5);

                    text = "Start Download";
                    pmgConnection.SendStatsCommand(StatsCommandType.StopStats);
                }

                //StatsStatusUpdateToGUI();
            }
            else if (model.BtnName.Equals("buttonStatsClearMemory"))
            {
                pmgConnection.SendStatsCommand(StatsCommandType.DeleteStats);
                System.Threading.Thread.Sleep(150);
                pmgConnection.SendStatsCommand(StatsCommandType.StatsStatus);
            }
            else if (model.BtnName.Equals("buttonStatsCheckStatus"))
            {
                pmgConnection.SendStatsCommand(StatsCommandType.StatsStatus);
            }
        }
    }
}