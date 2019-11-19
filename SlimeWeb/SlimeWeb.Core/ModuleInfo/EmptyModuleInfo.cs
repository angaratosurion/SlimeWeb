using SlimeWeb.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.ModuleInfo
{
    public class EmptyModuleInfo : IModuleInfo
    {
        public string Name{ get; set; }

           
      
        public string Description { get; set; }

        public string Version { get; set; }

        public string WebSite { get; set; }

        public string SourceCode { get; set; }
}
}
