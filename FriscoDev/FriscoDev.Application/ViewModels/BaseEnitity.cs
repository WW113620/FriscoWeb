using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class BaseResult
    {
        public BaseResult() { }

        public BaseResult(int _code, string _msg)
        {
            this.code = _code;
            this.msg = _msg;
        }
        public int code { get; set; }
        public string msg { get; set; }
    }
    public class BaseEnitity
    {
        public int code { get; set; }
        public string msg { get; set; }
    }

    public class ResultEnitity : BaseEnitity
    {
        public string value { get; set; }
    }

    public class ModelEnitity<T> : BaseEnitity
    {
        public T model { get; set; }
    }

    public class DataEnitity<T> : BaseEnitity
    {
        public DataEnitity()
        {
            data = new List<T>();
        }
        public List<T> data { get; set; }
    }

    public class PageEntity<T> : BaseEnitity
    {
        public PageEntity()
        {
            data = new List<T>();
        }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int pageCount { get; set; }
        public int iCount { get; set; }
        public List<T> data { get; set; }
    }

}
