using Data.Dapper;
using FriscoDev.Application.Models;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using FriscoDev.Application.Enum;
using FriscoDev.Application.ViewModels;

namespace FriscoDev.Data.Services
{
    public class UserService : IUserService
    {

        public Account GetModel(string userName, string password)
        {
            string sql = @" SELECT * FROM Account where UR_NAME=@UR_NAME AND UR_PASSWD=@UR_PASSWD ";
            var model = ExecuteDapper.QueryList<Account>(sql, new { UR_NAME = userName, UR_PASSWD = password }).FirstOrDefault();
            return model;
        }

        public List<Account> GetAccountList(string keyword, int pageIndex, int pageSize, out int icount)
        {
            string sql = @" SELECT * FROM Account ";
            string sort = " UR_ADDTIME DESC ";
            icount = 0;
            return ExecuteDapper.ExecutePageList<Account>(sql, sort, pageIndex, pageSize, out icount);
        }

        #region User List

        public IEnumerable<UserModel> GetAccountsByCustomerId(Account user)
        {
            if (user.UR_TYPE_ID == (int)UserType.Supervisor)
            {
                return GetAccountsByUID(user.CS_ID);
            }
            else if (user.UR_TYPE_ID == (int)UserType.Admin)
            {
                return GetAdminAccounts(user.CS_ID, user.UR_ID);
            }
            else
            {
                return GetRelugarAccounts(user.UR_ID);
            }
        }
        public IEnumerable<UserModel> GetAccountsByUID(string CS_ID)
        {
            const string sql = @"select u.*,c.CS_NAME,s.* 
                                from [Account] u left join CUSTOMER c on u.CS_ID = c.CS_ID
                                left join [SiteConfig] s on u.UR_ID = s.Login_UR_ID
                                where u.CS_ID=@CS_ID ";
            //using (var db=new EngrDev_NewEntities())
            //{
            //   return  db.Database.SqlQuery<UserModel>(sql, new {CS_ID = CS_ID});
            //}
            return ExecuteDapper.QueryList<UserModel>(sql, new { CS_ID = CS_ID });
        }
        public IEnumerable<UserModel> GetAdminAccounts(string CS_ID, string UR_ID)
        {
            const string sql = @" select u.*,c.CS_NAME,s.* 
                            from [Account] u left join CUSTOMER c on u.CS_ID = c.CS_ID
                            left join [SiteConfig] s on u.UR_ID = s.Login_UR_ID
                            where u.CS_ID=@CS_ID  and UR_TYPE_ID =3
                            union
                            select u.*,c.CS_NAME,s.* 
                            from [Account] u left join CUSTOMER c on u.CS_ID = c.CS_ID
                            left join [SiteConfig] s on u.UR_ID = s.Login_UR_ID
                            where u.UR_ID=@UR_ID
                             ";
            return ExecuteDapper.QueryList<UserModel>(sql, new { CS_ID = CS_ID, UR_ID = UR_ID });
        }
        public IEnumerable<UserModel> GetRelugarAccounts(string UR_ID)
        {
            const string sql = @" select u.*,c.CS_NAME,s.* 
                            from [Account] u left join CUSTOMER c on u.CS_ID = c.CS_ID
                            left join [SiteConfig] s on u.UR_ID = s.Login_UR_ID
                            where u.UR_ID=@UR_ID ";
            return ExecuteDapper.QueryList<UserModel>(sql, new { UR_ID = UR_ID });
        }

        public Account GetAccount(string userId)
        {
            using (var db = new PMGDATABASEEntities())
            {
                return db.Account.FirstOrDefault(c => c.UR_ID == userId);
            }
        }
        public bool EditAccount(Account account)
        {
            using (var db = new PMGDATABASEEntities())
            {
                 db.Account.AddOrUpdate(account);
                 return db.SaveChanges() > 0;
            }
        }
        public Account GetAccountByName(string userName)
        {
            const string sql = @"select * from [Account] where UR_NAME = @userName";
            return ExecuteDapper.QueryList<Account>(sql, new { userName = userName }).SingleOrDefault();
        }
        public UserLoginInfo GetUserLoginInfo(string id)
        {
            string sql = @" select * from (
                             select ROW_NUMBER() OVER (ORDER BY LoginTime DESC) AS [RowNumber],* from [UserLoginInfo] where UR_ID=@UR_ID
                             ) as t where RowNumber=2 ";
            return ExecuteDapper.QueryList<UserLoginInfo>(sql, new { UR_ID = id }).SingleOrDefault();
        }
        public void DeleteAccount(string id)
        {
            const string sql = @"delete from [Account] where UR_Id = @Uid";
            ExecuteDapper.GetRows(sql, new { Uid = id });
        }
        public void AddAccount(Account account)
        {
            ExecuteDapper.Insert(account, null);
        }
        public void AddSiteConfig(SiteConfig siteConfig)
        {
            ExecuteDapper.Insert(siteConfig, null);
        }
        #endregion

        public void AddOrUpdateSiteConfig(SiteConfig siteConfig)
        {
            using (var db = new PMGDATABASEEntities())
            {
                db.SiteConfig.AddOrUpdate(siteConfig);
                db.SaveChanges();
            }
        }

        public string GetParentIDByUser(string urId)
        {
            using (var db = new PMGDATABASEEntities())
            {
                string sql = @" SELECT TOP 1 [UR_ID]
                            FROM [Account]
                            where [UR_TYPE_ID]=2 and CS_ID=(select CS_ID from [Account] where [UR_ID]=@UR_ID )";
                var param = new System.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@UR_ID",
                    Value = urId
                };
                return db.Database.SqlQuery<string>(sql, param).FirstOrDefault();
            }
        }

        public SiteConfig GetSiteConfigByUser(string userId)
        {
            using (var db = new PMGDATABASEEntities())
            {
                return db.SiteConfig.FirstOrDefault(c => c.Login_UR_ID == userId);
            }
        }

        public SiteConfig GetSiteConfigByUserId(string userId)
        {
            using (var db = new PMGDATABASEEntities())
            {
                return db.SiteConfig.FirstOrDefault(c => c.Login_UR_ID == userId);
            }
        }
    }
}
