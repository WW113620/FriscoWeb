using Application.Models;
using FriscoDev.Application.Models;
using System.Collections.Generic;
using FriscoDev.Application.ViewModels;

namespace FriscoDev.Data.Services
{
    public interface IUserService : IDependency
    {
        SiteConfig GetSiteConfigByUserId(string userId);
        Account GetModel(string userName, string password);
        List<Account> GetAccountList(string keyword, int pageIndex, int pageSize, out int icount);
        void AddOrUpdateSiteConfig(SiteConfig siteConfig);
        string GetParentIDByUser(string urId);
        SiteConfig GetSiteConfigByUser(string userId);
        IEnumerable<UserModel> GetAccountsByCustomerId(Account user);
        Account GetAccount(string userId);
        UserLoginInfo GetUserLoginInfo(string id);
        void DeleteAccount(string id);
        void AddAccount(Account account);
        Account GetAccountByName(string userName);
        void AddSiteConfig(SiteConfig siteConfig);
        bool EditAccount(Account account);
        IEnumerable<UserModel> GetRelugarAccounts(string UR_ID);

        AdministrationViewModel GetAdministration(string userId);

        PageResult<AdministrationViewModel> GetAdminList(AdministrationRequest request);
    }
}
