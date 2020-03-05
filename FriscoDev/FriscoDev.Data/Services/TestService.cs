using Data.Dapper;
using FriscoDev.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Data.Services
{
    public class TestService: ITestService
    {

        public List<Account> GetList()
        {
            //using (DbContextEntities db = new DbContextEntities())
            //{
            //    return db.Account.ToList();
            //}
            string sql = @" select * from Account where UR_NAME=@UR_NAME ";
            var model = ExecuteDapper.QueryList<Account>(sql, new { UR_NAME = "test" });
            return model;
        }

    }
}
