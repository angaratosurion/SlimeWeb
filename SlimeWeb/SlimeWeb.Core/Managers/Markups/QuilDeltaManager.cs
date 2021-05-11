using Newtonsoft.Json.Linq;
using Quill.Delta;
using SlimeWeb.Core.Managers.Markups.Interfaces;
using System;

namespace SlimeWeb.Core.Managers.Markups
{
    public  class QuilDeltaManager : IMarkupManager
    {
        public string ConvertFromHtmlToMarkUp(string htmlcode)
        {
            throw new NotImplementedException();
        }

        public string ConvertToHtml(string quildelta)
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
