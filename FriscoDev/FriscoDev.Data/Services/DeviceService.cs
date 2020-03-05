using Application.Common;
using Data.Dapper;
using FriscoDev.Application.Models;
using FriscoDev.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Data.Services
{
    public class DeviceService : IDeviceService
    {
        public List<PMGModel> GetDeviceList(string keyword, int pageIndex, int pageSize, out int icount)
        {
            string sql = @" SELECT [PMD ID] as PMD_ID,* FROM PMD ";
            string sort = " [IMSI] ASC ";
            icount = 0;
            return ExecuteDapper.ExecutePageList<PMGModel>(sql, sort, pageIndex, pageSize, out icount);
        }

        public List<MessageModel> GetDeviceMessageList(string pmgId, string startDate, int pageIndex, int pageSize, out int icount)
        {
            string sql = @" SELECT a.*,b.PMDName,
                           ( CASE a.Category WHEN 1 THEN 'Debug' WHEN 2 THEN 'Information' WHEN 4 THEN 'Warning' WHEN 8 THEN 'Error' WHEN 16 THEN 'Fatal' ELSE 'Other' END) as CategoryName
                           FROM 
                          [dbo].[Message] as a
                          INNER JOIN [dbo].[PMD] as b ON a.DeviceID=b.[PMD ID]  WHERE a.DeviceID>0 {0}";

            string sqlWhere = string.Empty;
            if (!string.IsNullOrEmpty(pmgId) && pmgId.ToInt(0) > 0)
            {
                sqlWhere += " AND a.DeviceID=@DeviceID ";
            }

            if (!string.IsNullOrEmpty(startDate))
            {
                sqlWhere += " AND a.Timestamp >= @startDate ";
            }

            sql = string.Format(sql, sqlWhere);
            string sort = " Timestamp DESC ";
            icount = 0;
            return ExecuteDapper.ExecutePageList<MessageModel>(sql, sort, pageIndex, pageSize, out icount, false, new { DeviceID = pmgId, startDate = startDate });
        }

        public int DeleteWarningMessage(int pmgId)
        {
            string sql = @" DELETE FROM [Message] WHERE DeviceID=@DeviceID ";
            return ExecuteDapper.GetRows(sql, new { DeviceID = pmgId });
        }

        public bool CheckDeviceHasMsg(int pmgId)
        {
            string sql = " SELECT COUNT(0) AS iCount FROM [Message] WHERE  DeviceId =@DeviceId ";
            return ExecuteDapper.GetRows(sql, new { DeviceId = pmgId }) > 0;
        }
        public IEnumerable<DeviceLocationVm> GetDevicesLocation(string imsi)
        {
            const string sql = @"select p.PMDName,p.[PMD ID] as PMDID,p.DeviceType,d.* from [dbo].[DeviceLocation] as d
                                left join [dbo].[PMD] as p on d.[IMSI]=p.IMSI
                                where p.PMDName is not null and p.PMDName!='' and d.[IMSI]=@IMSI order by d.ID ";
            return ExecuteDapper.QueryList<DeviceLocationVm>(sql, new { IMSI = imsi });
        }
        public int DeleteLocation(string id)
        {
            string sql = @" DELETE FROM DeviceLocation WHERE ID=@Id ";
            return ExecuteDapper.GetRows(sql, new { Id = id });
        }
        public Pmd GetPmd(string pId)
        {
            const string sql = @"SELECT top 1 p.*,p.[PMD ID] as PMDID FROM [PMD] as p where [IMSI]= @IMSI ";
            return ExecuteDapper.QueryList<Pmd>(sql, new { IMSI = pId }).SingleOrDefault();
        }
        public int Add(Pmd device)
        {
            int i = 0;
            const string pmdSql = @"insert into [PMD] (PMDName,IMSI,Username,[PMD ID],Connection,StatsCollection,DeviceType,Location,Address,Clock,FirmwareVersion,NumFirmwareUpdateAttempts,KeepAliveMessageInterval,CS_ID,HighSpeedAlert,LowSpeedAlert) values(@PMDName,@IMSI,@Username,@PMDID,@Connection,@StatsCollection,@DeviceType,@Location,@Address,@Clock,@FirmwareVersion,@NumFirmwareUpdateAttempts,@KeepAliveMessageInterval,@CS_ID,@HighSpeedAlert,@LowSpeedAlert)";
            if (device.DeviceType == 3)
            {
                Pmd pmd = GetPmd(device.IMSI);
                if (pmd == null)
                {
                    ExecuteDapper.GetRows(pmdSql, device);
                }
                else
                {
                    device.PMDID = pmd.PMDID;
                }
                string Sql = @"insert into [PMDConfig](PMDName,IMSI,Username,[PMD ID],Connection,StatsCollection,DeviceType,Location,Address,Clock,FirmwareVersion,NumFirmwareUpdateAttempts,KeepAliveMessageInterval,CS_ID,HighSpeedAlert,LowSpeedAlert,[AddTime]) values(@PMDName,@IMSI,@Username,@PMDID,@Connection,@StatsCollection,@DeviceType,@Location,@Address,@Clock,@FirmwareVersion,@NumFirmwareUpdateAttempts,@KeepAliveMessageInterval,@CS_ID,@HighSpeedAlert,@LowSpeedAlert,GETDATE())";
                i = ExecuteDapper.GetRows(Sql, device);
                AddLeasedDevice(device);
            }
            else
            {
                i = ExecuteDapper.GetRows(pmdSql, device);
            }
            if (i > 0)
            {
                AddDeviceLocation(device.IMSI, device.Location, 0);
            }
            return i;
        }

        public int AddLeasedDevice(Pmd device)
        {
            string sql = @"INSERT INTO [dbo].[LeasedDeviceLog]
                                           ([IMSI]
                                           ,[PMDID]
                                           ,[BelongName]
                                           ,[LeasedStartDate]
                                           ,[LeasedEndDate]
                                           ,[CreateTime])
                                      VALUES
                                           (@IMSI
                                           ,@PMDID
                                           ,@BelongName
                                           ,@LeasedStartDate
                                           ,@LeasedEndDate
                                           ,GETDATE())";
            return ExecuteDapper.GetRows(sql, new { IMSI = device.IMSI, PMDID = device.PMDID, BelongName = device.BelongName, LeasedStartDate = device.LeasedStartDate, LeasedEndDate = device.LeasedEndDate });
        }
        public void AddDeviceLocation(string imsi, string location, int type = 0)
        {
            if (!string.IsNullOrEmpty(imsi) && !string.IsNullOrEmpty(location))
            {
                string sql = @"INSERT INTO [dbo].[DeviceLocation]
                               ([IMSI] ,[Location] ,[StartDate])
                               VALUES (@IMS ,@Location,getdate())";
                if (type == 0)
                {
                    ExecuteDapper.GetRows(sql, new { IMS = imsi, Location = location });
                }
                else
                {
                    int i = UpdateDeviceLocationEndDate(imsi);
                    if (i > 0)
                    {
                        ExecuteDapper.GetRows(sql, new { IMS = imsi, Location = location });
                    }
                }
            }
        }
        public int UpdateDeviceLocationEndDate(string imsi)
        {
            if (!string.IsNullOrEmpty(imsi))
            {
                const string sql = @"UPDATE [dbo].[DeviceLocation]
                                     SET [EndDate] =GETDATE()
                                     WHERE ID=(
                                     select MAX(ID) from [dbo].[DeviceLocation] where [IMSI]=@IMSI
                                     );";
                return ExecuteDapper.GetRows(sql, new { IMSI = imsi });

            }
            else
                return 0;
        }
        public int Delete(string id)
        {
            const string pmdSql = @"delete from [PMD] where [IMSI] = @IMSI";
            return ExecuteDapper.GetRows(pmdSql, new { IMSI = id });
        }
        public int CheckDevice(string IMSI, int activeId)
        {
            const string pmdSql = @"update [PMD] set [Clock]=@Clock where [IMSI] =@IMSI";
            return ExecuteDapper.GetRows(pmdSql, new { Clock = activeId, IMSI = IMSI });
        }
        public int GetDeviceType(string Id)
        {
            const string sql = @" SELECT [DeviceType] FROM [dbo].[PMD] where [IMSI] =@Id ";
            return ExecuteDapper.QueryList<int>(sql, new { Id = Id }).SingleOrDefault();
        }
        public Pmd GetLeasedDevice(string id)
        {
            const string sql = @" select p.[PMD ID] as PMDID, p.* ,t.Id,t.[BelongName],t.[LeasedStartDate],t.[LeasedEndDate]
                                  from [PMD] as p left join 
                                  (SELECT top 1 * FROM [LeasedDeviceLog] where [IMSI]= @IMSI )as t on p.IMSI=t.IMSI
                                  where p.[IMSI] =@IMSI ";
            return ExecuteDapper.QueryList<Pmd>(sql, new { IMSI = id }).SingleOrDefault();
        }
        public Pmd Get(string id)
        {
            const string sql = @"select [PMD ID] as PMDID,* from [PMD] where [IMSI] =@IMSI ";
            return ExecuteDapper.QueryList<Pmd>(sql, new { IMSI = id }).SingleOrDefault();
        }
        public int Update(Pmd device)
        {
            bool isInsert = false;
            var pmd = Get(device.IMSI);
            if (pmd != null)
            {
                if (!string.IsNullOrEmpty(pmd.Location) && !pmd.Location.Equals(device.Location))
                {
                    isInsert = true;
                }
                else
                {
                    isInsert = false;
                }
            }

            if (device.DeviceType == 3)
            {
                string Sql = @"insert into [PMDConfig](PMDName,IMSI,Username,[PMD ID],Connection,StatsCollection,DeviceType,Location,Address,Clock,FirmwareVersion,NumFirmwareUpdateAttempts,KeepAliveMessageInterval,CS_ID,HighSpeedAlert,LowSpeedAlert,[AddTime]) values(@PMDName,@IMSI,@Username,@PMD_ID,@Connection,@StatsCollection,@DeviceType,@Location,@Address,@Clock,@FirmwareVersion,@NumFirmwareUpdateAttempts,@KeepAliveMessageInterval,@CS_ID,@HighSpeedAlert,@LowSpeedAlert,GETDATE())";
                ExecuteDapper.GetRows(Sql, device);
                UpdateLeasedDevice(device);
            }
            const string pmdSql = @"update [PMD] set [PMDName]=@PMDName,[StatsCollection]=@StatsCollection,DeviceType=@DeviceType,Address=@Address,Location=@Location,HighSpeedAlert=@HighSpeedAlert,LowSpeedAlert=@LowSpeedAlert where [IMSI] =@IMSI";
            int i = ExecuteDapper.GetRows(pmdSql, device);
            if (i > 0 && isInsert)
            {
                AddDeviceLocation(device.IMSI, device.Location, 1);
            }
            return i;
        }
        public int UpdateLeasedDevice(Pmd device, int type = 0)
        {
            string sql = @"UPDATE  [LeasedDeviceLog]
                                    SET [BelongName] = @BelongName
                                        ,[LeasedStartDate] = @LeasedStartDate
                                        ,[LeasedEndDate] = @LeasedEndDate
                                        {0}
                                     WHERE Id=@Id";
            string sqlFor = string.Empty;
            if (type == 0)
            {
                sqlFor = " ,[CreateTime] = GETDATE() ";
            }
            string strSql = string.Format(sql, sqlFor);
            return ExecuteDapper.GetRows(strSql, new { Id = device.Id, BelongName = device.BelongName, LeasedStartDate = device.LeasedStartDate, LeasedEndDate = device.LeasedEndDate });
        }
    }
}
