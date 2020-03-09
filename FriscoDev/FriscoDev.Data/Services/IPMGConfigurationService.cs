using Application.Models;
using FriscoDev.Application.Models;
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

    }
}
