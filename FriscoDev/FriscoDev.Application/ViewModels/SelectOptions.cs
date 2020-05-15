using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class SelectOption
    {
        public string value { get; set; }

        public string Text { get; set; }
    }

    public class GraphicsOptions : SelectOption
    {
        public string ImageUrl { get; set; }
    }
}
