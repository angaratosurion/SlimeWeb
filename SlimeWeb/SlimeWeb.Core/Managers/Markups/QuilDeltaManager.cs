using Newtonsoft.Json.Linq;
using Quill.Delta;
using SlimeWeb.Core.Managers.Markups.Interfaces;
using System;
using System.Text;

namespace SlimeWeb.Core.Managers.Markups
{
    public  class QuilDeltaManager : IMarkupManager
    {
        public string ConvertFromHtmlToMarkUp(string htmlcode)
        {
            try
            {
                string ap = null;
                if (CommonTools.isEmpty(htmlcode) == false)
                {
                    //htmlcode=htmlcode.Replace("{\"ops\":", "");
                    //htmlcode=htmlcode.Remove(htmlcode.Length - 1, 1);
                    ap = htmlcode;


                }




                return ap;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public string ConvertToHtml(string quildelta)
        {
            try
            {
                string ap = null;
                 if ( CommonTools.isEmpty(quildelta)==false)
                {




                    //var aquildelta = quildelta.ToCharArray();
                    //if (aquildelta != null)
                    //{
                    //    aquildelta[0] = '[';
                    //    aquildelta[aquildelta.Length - 1] = ']';

                    //    StringBuilder stringBuilder = new StringBuilder();
                    //    stringBuilder.Append(aquildelta);
                    //    quildelta = stringBuilder.ToString();


                    //}


                    quildelta = quildelta.Replace("{\"ops\":", "");
                    quildelta=quildelta.Remove(quildelta.Length - 1, 1);
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
