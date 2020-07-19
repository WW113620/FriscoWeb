using FriscoDev.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class AdministrationViewModel : CustomerAccount
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int UserType { get; set; }
        public bool UserActive { get; set; }
        public int IsAdd { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }

        public string AddTimeValue { get; set; }
        public string Active { get; set; }
    }

    public class AdministrationRequest : BaseReqestParams
    {
        public string Email { get; set; }
    }
}
