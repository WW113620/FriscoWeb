//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FriscoDev.Application.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class StatsLog
    {
        public short Target_ID { get; set; }
        public System.DateTime Timestamp { get; set; }
        public string Direction { get; set; }
        public Nullable<decimal> LastSpeed { get; set; }
        public Nullable<decimal> PeakSpeed { get; set; }
        public Nullable<decimal> AverageSpeed { get; set; }
        public Nullable<byte> Strength { get; set; }
        public string Classfication { get; set; }
        public Nullable<short> Duration { get; set; }
        public Nullable<short> Product_ID { get; set; }
        public int PMD_ID { get; set; }
    }
}
