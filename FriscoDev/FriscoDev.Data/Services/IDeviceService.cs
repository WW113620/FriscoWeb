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
    public  interface IDeviceService : IDependency
    {

        List<PMGModel> GetDeviceList(string CS_ID, string keyword, int pageIndex, int pageSize, out int icount);

        List<MessageModel> GetDeviceMessageList(string pmgId, string startDate, int pageIndex, int pageSize, out int icount);

        int DeleteWarningMessage(int pmgId);

        bool CheckDeviceHasMsg(int pmgId);
        IEnumerable<DeviceLocationVm> GetDevicesLocation(string imsi);
        int DeleteLocation(string id);
        int Add(FriscoDev.Application.Models.PMD device);
        Pmd GetPmd(string pId);
        int Delete(string id);
        int CheckDevice(string IMSI, int activeId);
        int GetDeviceType(string Id);
        Pmd GetLeasedDevice(string id);
        Pmd Get(string id);
        int Update(FriscoDev.Application.Models.PMD device);

        int SaveDevicePosition(string imsi, string x,string y);

    }
}
