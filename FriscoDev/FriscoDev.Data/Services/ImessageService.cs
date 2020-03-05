using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using FriscoDev.Application.ViewModels;

namespace FriscoDev.Data.Services
{
    public interface IMessageService: IDependency
    {
        List<MessageEntityVm> GetAertMessageList(int devType, string CS_ID, int pmdId, int pageIndex, int pageSize, out int iCount);
        void UpdateAllAlert(int devType, int pmdId);
    }
}
