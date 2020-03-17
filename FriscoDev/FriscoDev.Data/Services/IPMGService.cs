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
    public interface IPMGService : IDependency
    {
        List<PMGModel> GetPMGList(string userName, int type);

        PMGModel GetPMGModel(string imsi);

        List<PMGModel> GetOnlinePMGList();
    }
}
