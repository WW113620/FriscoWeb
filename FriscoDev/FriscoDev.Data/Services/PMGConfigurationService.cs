using Data.Dapper;
using FriscoDev.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Data.Services
{
    public class PMGConfigurationService : IPMGConfigurationService
    {
        public int DeleteConfigurationByPmgid(int pmgid, string paramaterIds)
        {
            string sql = string.Format(@" DELETE FROM [PMGConfiguration] WHERE [PMG ID]=@pmgid AND [Parameter ID] IN ({0}) ", paramaterIds);
            return ExecuteDapper.GetRows(sql, new { pmgid = pmgid });
        }

        public List<PMGConfiguration> GetConfigurationByPmgid(int pmgid, string paramaterIds)
        {
            string sql = string.Format(@" select [PMG ID] as PMG_ID,[Parameter ID] as Parameter_ID,Value,[State]  from  [dbo].[PMGConfiguration] WHERE [PMG ID]=@pmgid AND [Parameter ID] IN ({0}) ", paramaterIds);
            return ExecuteDapper.QueryList<PMGConfiguration>(sql, new { pmgid = pmgid });
        }

    }
}
