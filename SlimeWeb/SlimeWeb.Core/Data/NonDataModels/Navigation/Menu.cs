using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data.NonDataModels.Navigation
{
    public class Menu
    {
        public String Name { get; set; }
        public String Title { get; set; }
        public string ControllerName { get; set; }
        public string Actionanme{get;set;}
        public List<Menu> MenuItems { get; set; }
    }
}
