//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FriscoDev.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class DEVICE
    {
        public string DEV_ID { get; set; }
        public string DEV_NAME { get; set; }
        public string DEV_CODE { get; set; }
        public string CS_ID { get; set; }
        public int DEV_TYPE { get; set; }
        public Nullable<decimal> DEV_COORDINATE_X { get; set; }
        public Nullable<decimal> DEV_COORDINATE_Y { get; set; }
        public string DEV_ADDRESS { get; set; }
        public bool DEV_ACTIVE { get; set; }
        public System.DateTime DEV_ADDTIME { get; set; }
        public Nullable<System.DateTime> DEV_UPTIME { get; set; }
        public Nullable<int> PMD_ID { get; set; }
        public string CCID { get; set; }
    }
}
