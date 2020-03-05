using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class MessageEntityVm
    {
        public int RowNumber { get; set; }
        public int DeviceDelete { get; set; }
        public int PmdId { get; set; }
        public int CATEGORY { get; set; }
        public DateTime TIMESTAMP { get; set; }

        public string TIMESTAMP2 { get; set; }
        public string MESSAGE { get; set; }
        public int DEVICE_TYPE { get; set; }

        public string CATEGORYNAME { get; set; }

        public string DeviceId { get; set; }

        public bool IsDelete { get; set; }

        public string PmdName { get; set; }
    }
}
