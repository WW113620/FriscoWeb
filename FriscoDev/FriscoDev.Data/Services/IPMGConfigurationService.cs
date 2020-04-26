using Application.Models;
using FriscoDev.Application.Models;
using FriscoDev.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Data.Services
{

    public interface IPMGConfigurationService : IDependency
    {
        int DeleteConfigurationByPmgid(int pmgid,string paramaterIds);

        List<PMGConfiguration> GetConfigurationByPmgid(int pmgid, string paramaterIds);

        List<Pages> GetDisplayPagesByActionType(int pmgInch, int actionType, string loginName);

        Pages GetDisplayPagesByPageName(string PageName, string loginName);

        Pages GetDisplayPagesByPageName(string PageName, int DisplayType, int PageType, string loginName);

        int DeletePage(string PageName, int DisplayType, int PageType, string loginName);

        int UpdatePage(string PageName, int DisplayType, int PageType, string Username, string content,int hash);

        int InsertPage(string PageName, int DisplayType, int PageType, string Content, int Hash, string Username);

        int GetGPIOModules(int PMGID);

        List<PMGConfigurationModel> GetGPIOModules(int PMGID, int State);
    }
}
