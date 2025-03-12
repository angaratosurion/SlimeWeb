using Newtonsoft.Json.Linq;
using Quill.Delta;
using SlimeWeb.Core.Managers.Markups.Interfaces;
using SlimeWeb.Core.Tools;
using System;
using System.Text;

namespace SlimeWeb.Core.Managers.Markups
{
    public  class QuilDeltaManager : IMarkupManager
    {

           string postcreationhtmlcode = "<script src=\"https://cdn.quilljs.com/1.3.6/quill.min.js\"></script>\r\n       " +
            "     <link href=\"https://cdn.quilljs.com/1.3.6/quill.snow.css\" rel=\"stylesheet\">\r\n          " +
            " <script src=\""+AppSettingsManager.GetPathBase()+"/wwwroot/lib/quill-image-resize-module/image-resize.min.js\">" +
            "</script>" + " <div id=\"scrolling-container\">\r\n                  " +
            "      <div class=\"form-group\" id=\"editor\" name=\"editor\" >" +
            " </div>\r\n                        </div>"+
            "<script src=\""+AppSettingsManager.GetPathBase()+"/wwwroot/js/quil-markup/post-create.js\"></script>";




          string postedithtmlcode = "<script src=\"https://cdn.quilljs.com/1.3.6/quill.min.js\"></script>\r\n       " +
            "     <link href=\"https://cdn.quilljs.com/1.3.6/quill.snow.css\" rel=\"stylesheet\">\r\n          " +
            " <script src=\""+AppSettingsManager.GetPathBase()+"/wwwroot/lib/quill-image-resize-module/image-resize.min.js\">" +
            "</script>" + "<script src=\""+AppSettingsManager.GetPathBase()+"/wwwroot/js/quil-markup/post-edit.js\"></script>";
        //+
            //" <textarea asp-for=\"content\" class=\"form-control\"" +
            //" id=\"content\" hidden=\"hidden\" name=\"content\">\r\n  " +
            //"      </textarea> ";



        string pagecreationhtmlcode = "<script src=\"https://cdn.quilljs.com/1.3.6/quill.min.js\">" +
            "</script>\r\n            " +
            "<link href=\"https://cdn.quilljs.com/1.3.6/quill.snow.css\" " +
            "rel=\"stylesheet\">\r\n          " +
            " <script src=\""+AppSettingsManager.GetPathBase()+"/wwwroot/lib/quill-image-resize-module/image-resize.min.js\"></script>"
            + " <div id=\"scrolling-container\">\r\n                  " +
            "      <div class=\"form-group\" id=\"editor\" name=\"editor\" >" +
            " </div>\r\n                        </div>" +
            "<script src=\""+AppSettingsManager.GetPathBase()+"/wwwroot/js/quil-markup/page-create.js\"></script>";



         string pageedithtmlcode = "<script src=\"https://cdn.quilljs.com/1.3.6/quill.min.js\">" +
            "</script>\r\n            <link href=\"https://cdn.quilljs.com/1.3.6/quill.snow.css\" " +
            "rel=\"stylesheet\">\r\n          " +
            " <script src=\""+AppSettingsManager.GetPathBase()+"/wwwroot/lib/quill-image-resize-module/image-resize.min.js\"></script>"
            + " <div id=\"scrolling-container\">\r\n                  " +
            "      <div class=\"form-group\" id=\"editor\" name=\"editor\" >" +
            " </div>\r\n                        </div>" +
            "<script src=\"" + AppSettingsManager.GetPathBase() +"/wwwroot/js/quil-markup/page-edit.js\"></script>";
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
                CommonTools.ErrorReporting(ex);

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
                CommonTools.ErrorReporting(ex);

                return null;
            }
        }

        public string PageCreationandEditHtml(bool CreateAction)
        {
            try
            {
                string ap = null;
                if (CreateAction)
                {
                    ap = pagecreationhtmlcode;
                }
                else
                {
                    ap = pageedithtmlcode;
                }
                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }
        }

        public string PostCreationandEditHtml(bool CreateAction)
        {
            try
            {
                string ap = null;
                if (CreateAction)
                {
                    ap = postcreationhtmlcode;
                }
                else
                {
                    ap = postedithtmlcode;
                }
                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }
        }
    }
}
