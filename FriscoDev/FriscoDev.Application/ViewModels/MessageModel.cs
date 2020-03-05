using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class MessageModel
    {
        public int _row_number_ { get; set; }
        public int DeviceID { get; set; }
        public int Category { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public int DeviceType { get; set; }
        public string PMDName { get; set; }
        public string CategoryName { get; set; }

        public string ShowDeviceType
        {
            get
            {
                return DevFun.GetDeviceType(DeviceType);
            }
        }

        public string ShowTimestamp
        {
            get
            {
                return DevFun.ConvertShowDate(Timestamp);
            }
        }

    }
}
