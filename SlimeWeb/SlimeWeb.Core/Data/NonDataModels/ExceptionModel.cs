using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SlimeWeb.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data.NonDataModels
{
   public class ExceptionModel//:PageModel
    {
        
         
        
        
        public DateTime TimeStamp { get; set; }
        public String Level { get; set; }
        public String Logger { get; set; }
        public String Message { get; set; }
    }
}

