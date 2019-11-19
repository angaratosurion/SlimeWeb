using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SlimeWeb.Core.Interfaces;

namespace SlimeWeb.Core.ModuleInfo
{
    [Export(typeof(IModuleInfo)), ExportMetadata("Type", "ModuleInfo")]
  
    public class SlimeWebCoreInfo: IModuleInfo
    {
        public string Description
        {
            get
            {
                return "";
            }
            set { }
        }

        public string Name
        {
            get
            {
                return "SlimeWeb.Core";
            }
            set { }
        }

        public string SourceCode
        {
            get
            {
                return "https://github.com/angaratosurion/SlimeWeb.Core";
            }
            set { }
        }

        public string Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            set { }
        }

        public string WebSite
        {
            get
            {
                return "http://pariskoutsioukis.net/blog/";
            }
            set { }
        }
    }

}
