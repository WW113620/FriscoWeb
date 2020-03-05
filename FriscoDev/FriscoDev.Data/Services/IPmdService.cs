using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using FriscoDev.Application.ViewModels;

namespace FriscoDev.Data.Services
{
    public interface IPmdService: IDependency
    {
        Pmd GetPmgById(int pmgid);
        Pmd GetPmd(string pId);
        void UpdatePmd(Pmd pmdModel);
        int GetDeviceTypeById(string IMSI);
        IEnumerable<Pmd> GetDevicesByCustomerId(string cusId);
        IEnumerable<DeviceLocationVm> GetDevicesLocation(string imsi, string startDate, string endDate);
    }
}
