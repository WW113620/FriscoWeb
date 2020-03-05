using Data.Dapper;
using FriscoDev.Application.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace FriscoDev.Data.Services
{
    public class MessageService : IMessageService
    {
        public List<MessageEntityVm> GetAertMessageList(int devType, string CS_ID, int pmdId, int pageIndex, int pageSize, out int iCount)
        {
            string sql = @"SELECT t.* from (
                                  select ROW_NUMBER() OVER (ORDER BY a.Timestamp DESC) AS [RowNumber],a.*,(case  a.Category when 1 then 'Debug' when 2 then 'Information' when 4 then 'Warning' when 8 then 'Error' when 16 then 'Fatal' else 'OTHER' end) CategoryName,b.PmdName
                                   from [Message] a 
                                  left join pmd as b on a.DeviceID=b.[PMD ID] 
                                  where a.DeviceID=@DeviceID and a.DeviceType=@DeviceType {0}                  
                                ) as t
                                where @PageSize * (@CurrentPage - 1) < RowNumber AND RowNumber <= @PageSize * @CurrentPage ";
            string sqlCount = @" select count(0) as iCount from [Message] a 
                                left join pmd as b on a.DeviceID=b.[PMD ID] 
                                where a.DeviceID=@DeviceID and a.DeviceType=@DeviceType {0}  ";
            string sqlWhere = string.Empty;
            if (!string.IsNullOrEmpty(CS_ID))
            {
                sqlWhere = string.Format(" and b.CS_ID={0} ", CS_ID);
            }
            sql = string.Format(sql, sqlWhere);
            sqlCount = string.Format(sqlCount, sqlWhere);
            iCount = ExecuteDapper.QueryList<int>(sqlCount, new { DeviceID = pmdId, DeviceType = devType }).FirstOrDefault();

            return ExecuteDapper.QueryList<MessageEntityVm>(sql, new
            {
                DeviceID = pmdId,
                DeviceType = devType,
                CurrentPage = pageIndex,
                PageSize = pageSize
            }).ToList();
        }
        public void UpdateAllAlert(int devType, int pmdId)
        {
            string sql = @" DELETE [Message]  WHERE DeviceID=@DeviceID and DeviceType=@DeviceType ";
            ExecuteDapper.GetRows(sql, new { DeviceID = pmdId, DeviceType = devType });
        }
    }
}
