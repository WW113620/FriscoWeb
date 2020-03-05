using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Dapper;
using FriscoDev.Application.ViewModels;

namespace FriscoDev.Data.Services
{
    public class PmdService: IPmdService
    {

        public Pmd GetPmgById(int pmgid)
        {
            const string sql = @"SELECT top 1 p.*,p.[PMD ID] as PMDID FROM [PMD] as p where [PMD ID]= @PMGID ";
            return ExecuteDapper.QueryList<Pmd>(sql, new { PMGID = pmgid }).SingleOrDefault();
        }

        public Pmd GetPmd(string pId)
        {
            const string sql = @"SELECT top 1 p.*,p.[PMD ID] as PMDID FROM [PMD] as p where [IMSI]= @IMSI ";
            return ExecuteDapper.QueryList<Pmd>(sql, new { IMSI = pId }).SingleOrDefault();
        }
        public void UpdatePmd(Pmd pmdModel)
        {
            const string pmdSql = @"update [PMD] set NewConfiguration = @NewConfiguration, NewConfigurationTime =@NewConfigurationTime where [IMSI]=@IMSI";
            ExecuteDapper.GetRows(pmdSql, pmdModel);
        }
        public int GetDeviceTypeById(string IMSI)
        {
            const string sql = @"select top 1 DeviceType from pmd where [IMSI]=@IMSI ";
            return ExecuteDapper.QueryList<int>(sql, new { IMSI = IMSI }).FirstOrDefault();
        }
        public IEnumerable<Pmd> GetDevicesByCustomerId(string cusId)
        {
            const string sql = @"   select [PMD ID] as PMDID,* from [PMD] where Clock=0 and DeviceType in (1,2,4,5,6) and CS_ID =@CsId order by DeviceType ";
            return ExecuteDapper.QueryList<Pmd>(sql, new { CsId = cusId });
        }
        public IEnumerable<DeviceLocationVm> GetDevicesLocation(string imsi, string startDate, string endDate)
        {
            const string sql = @" select p.Address,p.PMDName,p.[PMD ID] as PMDID,p.DeviceType,d.* from [dbo].[DeviceLocation] as d
                                    left join [dbo].[PMD] as p on d.[IMSI]=p.IMSI
                                    where p.PMDName is not null and p.PMDName!='' and d.[IMSI]=@IMSI 
                                    and ((@startDate<=d.StartDate and @endDate>=d.EndDate) or (@startDate<=d.StartDate and (d.EndDate is null or d.EndDate=''))) 
                                    order by d.ID ";
            return ExecuteDapper.QueryList<DeviceLocationVm>(sql, new { IMSI = imsi, startDate = startDate, endDate = endDate });
        }
    }
}
