using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data.NonDataModels.Navigation
{
    public class SlimeWebMenu
    {
        public String Name { get; set; }
        public String Title { get; set; }
        public String ControllerName { get; set; }
        public String ActionName{get;set;}
        public List<SlimeWebMenu> MenuItems { get; set; }
        public string Path { get; set; }
         
    }
}
