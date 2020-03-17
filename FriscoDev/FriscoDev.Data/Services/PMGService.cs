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
    public class PMGService : IPMGService
    {
        public List<PMGModel> GetPMGList(string userName, int type)
        {
            string sql = @"SELECT [PMDName]
                              ,[IMSI]
                              ,[DeviceType]
                              ,[Address]
                              ,[Username]
                              ,[Location]
                              ,[Connection]
                              ,[StatsCollection]
                              ,[PMD ID] as PMD_ID
                              ,[CS_ID]
                          FROM [dbo].[PMD] ";
            if (type == 1)
            {
                sql = @"SELECT [PMDName]
                              ,[IMSI]
                              ,[DeviceType]
                              ,[Address]
                              ,[Username]
                              ,[Location]
                              ,[Connection]
                              ,[StatsCollection]
                              ,[PMD ID] as PMD_ID
                              ,[CS_ID]
                          FROM [dbo].[PMD] WHERE Username=@Username ";
            }
            return ExecuteDapper.QueryList<PMGModel>(sql, new { Username = userName });
        }

        public PMGModel GetPMGModel(string imsi)
        {
            string sql = @"SELECT [PMDName]
                              ,[IMSI]
                              ,[DeviceType]
                              ,[Address]
                              ,[Username]
                              ,[Location]
                              ,[Connection]
                              ,[StatsCollection]
                              ,[PMD ID] as PMD_ID
                              ,[CS_ID]
                          FROM [dbo].[PMD] WHERE IMSI=@IMSI ";
            return ExecuteDapper.GetModel<PMGModel>(sql, new { IMSI = imsi });
        }
        public PMGModel GetPMGModelVm(string CS_ID)
        {
            string sql = @"SELECT [PMDName]
                              ,[IMSI]
                              ,[DeviceType]
                              ,[Address]
                              ,[Username]
                              ,[Location]
                              ,[Connection]
                              ,[StatsCollection]
                              ,[PMD ID] as PMD_ID
                              ,[CS_ID]
                          FROM [dbo].[PMD] WHERE CS_ID=@CS_ID ";
            return ExecuteDapper.GetModel<PMGModel>(sql, new { CS_ID = CS_ID });
        }

        public List<PMGModel> GetOnlinePMGList()
        {
            string sql = @"SELECT [PMDName]
                              ,[IMSI]
                              ,[DeviceType]
                              ,[Address]
                              ,[Username]
                              ,[Location]
                              ,[Connection]
                              ,[StatsCollection]
                              ,[PMD ID] as PMD_ID
                              ,[CS_ID]
                          FROM [dbo].[PMD] WHERE Connection=1 ";
            
            return ExecuteDapper.QueryList<PMGModel>(sql);
        }

    }
}
