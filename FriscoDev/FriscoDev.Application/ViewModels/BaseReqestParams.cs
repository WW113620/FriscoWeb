using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class BaseReqestParams
    {
        public int page { get; set; } = 1;
        public int limit { get; set; } = 10;
        public long count { get; set; } = 0;
        public long pageCount => (long)Math.Ceiling(count / Convert.ToDouble(limit));
    }

    public class PageResult<T> : BaseEnitity
    {
        public PageResult()
        {
            page = new BaseReqestParams();
            data = new List<T>();
        }
        public BaseReqestParams page { get; set; }
        public List<T> data { get; set; }
    }

    public class LayuiPageResult<T> where T : class
    {
        public LayuiPageResult()
        {
            data = new List<T>();
        }
        public int code { get; set; }
        public string msg { get; set; }
        public long count { get; set; }
        public List<T> data { get; set; }
    }

}
