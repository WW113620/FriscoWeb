using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FriscoDev.Application.ViewModels;

namespace FriscoDev.UI.Models
{
    public class DeviceModel: DeviceLocationVm
    {
        public string DevCoordinateX { get; set; }
        public string DevCoordinateY { get; set; }
        public string StrStartDate { get; set; }
        public string StrEndDate { get; set; }
    }
    
}