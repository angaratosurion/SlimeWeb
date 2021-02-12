using Newtonsoft.Json.Linq;
using Quill.Delta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
   public  class QuilDeltaManager
    {

        public string ToHrml(string quildelta)
        {
            try
            {
                string ap = null;
                 if ( CommonTools.isEmpty(quildelta)==false)
                {
                    var deltaOps = JArray.Parse(quildelta);
                  
                    var htmlConverter = new HtmlConverter(deltaOps);
                    ap = htmlConverter.Convert();
                    

                }




                return ap;
            }
            catch (Exception ex)
            {

                return null;
            }
        } 
    }
}
