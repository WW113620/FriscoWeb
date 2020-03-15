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

        public List<Pages> GetDisplayPagesByActionType(int pmgInch, int actionType, string loginName)
        {
            string sql = @" SELECT PageName,DisplayType,PageType,Content,Hash,Username FROM  [dbo].[Pages] WHERE DisplayType=@DisplayType AND PageType=@PageType AND Username=@Username ";
            return ExecuteDapper.QueryList<Pages>(sql, new { DisplayType = pmgInch, PageType = actionType, Username = loginName });
        }

        public Pages GetDisplayPagesByPageName(string PageName, string loginName)
        {
            string sql = @" SELECT PageName,DisplayType,PageType,Content,Hash,Username FROM  [dbo].[Pages] WHERE PageName=@PageName AND Username=@Username ";
            return ExecuteDapper.GetModel<Pages>(sql, new { PageName = PageName, Username = loginName });
        }

        public Pages GetDisplayPagesByPageName(string PageName, int DisplayType, int PageType, string loginName)
        {
            string sql = @" SELECT PageName,DisplayType,PageType,Content,Hash,Username 
                            FROM  [dbo].[Pages] WHERE PageName=@PageName AND DisplayType=@DisplayType AND PageType=@PageType  AND Username=@loginName ";
            return ExecuteDapper.GetModel<Pages>(sql, new { PageName = PageName, DisplayType = DisplayType, PageType = PageType, loginName = loginName });
        }

        public int DeletePage(string PageName, int DisplayType, int PageType, string loginName)
        {
            string sql = @" DELETE FROM [dbo].[Pages] WHERE PageName=@PageName AND DisplayType=@DisplayType AND PageType=@PageType  AND Username=@loginName ";
            return ExecuteDapper.GetRows(sql, new { PageName = PageName, DisplayType = DisplayType, PageType = PageType, loginName = loginName });
        }

        public int UpdatePage(string PageName, int DisplayType, int PageType, string loginName, string content)
        {
            string sql = @" UPDATE [Pages] SET Content=@content WHERE PageName=@PageName AND DisplayType=@DisplayType AND PageType=@PageType  AND Username=@loginName ";
            return ExecuteDapper.GetRows(sql, new { PageName = PageName, DisplayType = DisplayType, PageType = PageType, loginName = loginName, content = content });
        }


        public int InsertPage(string PageName, int DisplayType, int PageType, string Content, int Hash, string Username)
        {
            string sql = @" Insert into [Pages](PageName,DisplayType,PageType,Content,Hash,Username) values(@PageName,@DisplayType,@PageType,@Content,@Hash,@Username) ";
            return ExecuteDapper.GetRows(sql, new { PageName = PageName, DisplayType = DisplayType, PageType = PageType, Content = Content, Hash = Hash, Username = Username });
        }

    }
}
