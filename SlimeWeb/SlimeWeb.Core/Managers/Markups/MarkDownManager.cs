

using Markdig;
//using Markdig;
using ReverseMarkdown;
using SlimeWeb.Core.Managers.Markups.Interfaces;
using SlimeWeb.Core.Tools;
using System;

namespace SlimeWeb.Core.Managers.Markups
{
    public class MarkDownManager : IMarkupManager
    {
        MarkdownPipeline pipeline;
        const string postcreationhtmlcode = " <script src=\"/wwwroot/lib/tinymce/tinymce.min.js\"" +
            " referrerpolicy=\"origin\">" +
            "</script><script>\r\n   " +
            " tinymce.init({\r\n        " +
            "selector: '#content',\r\n        " +
            "plugins: ' code a11ychecker advcode " +
            "casechange formatpainter " +
            "linkchecker autolink lists " +
            "checklist media mediaembed " +
            "pageembed permanentpen powerpaste " +
            "table advtable tinycomments" +
            " tinymcespellchecker image " +
            "imagetools',\r\n       " +
            " toolbar: 'a11ycheck addcomment " +
            "showcomments casechange checklist " +
            "code formatpainter pageembed " +
            "permanentpen table link|image'\r\n        " +
            ", images_upload_url: '@ViewBag.pathbase/Posts/Upload/@(Context.Request.RouteValues[\"id\"])'" +
            ",\r\n        automatic_uploads: true,\r\n        images_reuse_filename: true,\r\n       " +
            " //images_upload_base_path: '~/AppData/AppData/Temp/'\r\n    })\r\n               " +
            " </script>";
        const string postedithtmlcode = "<script>\r\n    " +
            "tinymce.init({\r\n        " +
            "selector: '#content',\r\n        " +
            "plugins: ' code a11ychecker advcode " +
            "casechange formatpainter linkchecker" +
            " autolink lists checklist media" +
            " mediaembed pageembed permanentpen " +
            "powerpaste table advtable tinycomments " +
            "tinymcespellchecker image imagetools',\r\n   " +
            "     toolbar: 'a11ycheck addcomment showcomments" +
            " casechange checklist code formatpainter " +
            "pageembed permanentpen table link|image'\r\n   " +
            "     , images_upload_url: '@ViewBag.pathbase/Posts/UploadEdit/@(Context.Request.RouteValues[\"id\"])',\r\n      " +
            "  automatic_uploads: true,\r\n        images_reuse_filename: true,\r\n       " +
            " //images_upload_base_path: '~/AppData/AppData/Temp/'\r\n    })\r\n               " +
            " </script>";
        const string pagecreationhtmlcode = " <script src=\"/wwwroot/lib/tinymce/tinymce.min.js\"" +
            " referrerpolicy=\"origin\"></script>" +
            "<script>\r\n    tinymce.init({\r\n    " +
            "    selector: '#content',\r\n      " +
            "  plugins: ' code a11ychecker advcode " +
            "casechange formatpainter linkchecker " +
            "autolink lists checklist media mediaembed" +
            " pageembed permanentpen powerpaste table " +
            "advtable tinycomments tinymcespellchecker " +
            "image imagetools',\r\n       " +
            " toolbar: 'a11ycheck addcomment " +
            "showcomments casechange checklist " +
            "code formatpainter pageembed " +
            "permanentpen table link|image'\r\n    " +
            "    , images_upload_url: '@ViewBag.pathbase/Posts/Upload/@(Context.Request.RouteValues[\"id\"])',\r\n    " +
            "    automatic_uploads: true,\r\n        images_reuse_filename: true,\r\n      " +
            "  //images_upload_base_path: '~/AppData/AppData/Temp/'\r\n " +
            "   })\r\n                </script>";
        const string pageedithtmlcode = " <script src=\"/wwwroot/lib/tinymce/tinymce.min.js\"" +
            " referrerpolicy=\"origin\"></script>" +
            "<script>\r\n   " +
            " tinymce.init({\r\n       " +
            " selector: '#content',\r\n     " +
            "   plugins: ' code a11ychecker advcode " +
            "casechange formatpainter linkchecker autolink " +
            "lists checklist media mediaembed" +
            " pageembed permanentpen powerpaste " +
            "table advtable tinycomments" +
            " tinymcespellchecker image " +
            "imagetools',\r\n        " +
            "toolbar: 'a11ycheck addcomment" +
            " showcomments casechange checklist" +
            " code formatpainter pageembed" +
            " permanentpen table link|image'\r\n  " +
            "      , images_upload_url: '@ViewBag.pathbase/Posts/UploadEdit/@(Context.Request.RouteValues[\"id\"])',\r\n        automatic_uploads: true,\r\n        images_reuse_filename: true,\r\n        //images_upload_base_path: '~/AppData/AppData/Temp/'\r\n    })\r\n                </script>\r\n            }\r\n        }";
        public string ConvertToHtml(string markdowncode)
        {
            try
            {
                string ap=null;

                if ( CommonTools.isEmpty(markdowncode)==false)
                {
                 pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                 ap= Markdown.ToHtml(markdowncode, pipeline);
                }
                return ap;
            }
            catch (Exception  ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public string ConvertFromHtmlToMarkUp(string htmlcode)
        {
            try
            {
                string ap = null;

                if (CommonTools.isEmpty(htmlcode) == false)
                {

                    Converter conv = new  Converter();

                    ap=conv.Convert(htmlcode);
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
