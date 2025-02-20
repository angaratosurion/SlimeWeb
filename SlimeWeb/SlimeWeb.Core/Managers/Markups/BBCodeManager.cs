using Q101.BbCodeNetCore;
using SlimeWeb.Core.Managers.Markups.Interfaces;
using SlimeWeb.Core.Tools;
using System;

namespace SlimeWeb.Core.Managers.Markups
{
    public  class BBCodeManager : IMarkupManager
    {

        const string postcreationhtmlcode = "<script src=\\\"/wwwroot/lib/tinymce\r\n/" +
            "tinymce.min.js\\\" \r\nreferrerpolicy=\\\"origin\\\">" +
            "</script> " +
            "<script>\r\n\r\n               " +
            " tinymce.init({ selector: '#content',\r\n       " +
            " plugins: 'bbcode code a11ychecker advcode casechange " +
            "formatpainter linkchecker autolink lists checklist media " +
            "mediaembed pageembed permanentpen powerpaste table advtable" +
            " tinycomments tinymcespellchecker image imagetools',\r\n        " +
            "toolbar: 'a11ycheck addcomment showcomments casechange checklist " +
            "code formatpainter pageembed permanentpen table link|image'\r\n       " +
            "     , images_upload_url: '@ViewBag.pathbase/Posts/Upload/@(Context.Request.RouteValues[\"id\"])',\r\n  " +
            "automatic_uploads: true,\r\n images_reuse_filename: true,\r\n" +
            " //images_upload_base_path: '~/AppData/AppData/Temp/'\r\n                })\r\n\r\n  " +
            "       " +
            "       </script>";
        const string postedithtmlcode = "<script>\r\n\r\ntinymce.init({ selector: '#content',\r\nplugins: 'bbcode code a11ychecker \r\nadvcode casechange formatpainter " +
            "\r\nlinkchecker autolink lists checklist \r\nmedia mediaembed pageembed permanentpen \r\npowerpaste table advtable tinycomments\r\n tinymcespellchecker image imagetools'," +
            "\r\ntoolbar: 'a11ycheck addcomment \r\nshowcomments casechange checklist \r\ncode formatpainter pageembed \r\npermanentpen table link|image'\r\n," +
            " images_upload_url: '@ViewBag.pathbase/Posts/UploadEdit/@(Context.Request.RouteValues[\"id\"])',\r\n" +
            "automatic_uploads: true,\r\n" +
            "images_reuse_filename: true," +
            "\r\n//images_upload_base_path: \r\n" +
            "'~/AppData/AppData/Temp/'\r\n        " +
            "})\r\n </script>";

        const string pagecreationhtmlcode = "<script src=\"/wwwroot/lib/tinymce/tinymce.min.js\"" +
            " referrerpolicy=\"origin\"></script>\r\n\r\n\r\n            if (ViewBag.CreateAction == true)\r\n            {\r\n               " +
            " <script>\r\n\r\n                tinymce.init({ selector: '#content',\r\n        plugins: 'bbcode code a11ychecker advcode casechange formatpainter " +
            "linkchecker autolink lists checklist media mediaembed pageembed permanentpen powerpaste table advtable tinycomments tinymcespellchecker image " +
            "imagetools',\r\n        toolbar: 'a11ycheck addcomment showcomments casechange checklist code formatpainter pageembed permanentpen table " +
            "link|image'\r\n            , images_upload_url: '@ViewBag.pathbase/Posts/Upload/@(Context.Request.RouteValues[\"id\"])',\r\n        " +
            "automatic_uploads: true,\r\n        images_reuse_filename: true,\r\n        //images_upload_base_path: '~/AppData/AppData/Temp/'\r\n })\r\n\r\n" +
            "</script>";
        const string pageedithtmlcode = "  <script>\r\n\r\n    " +
            "          " +
            "  tinymce.init({ selector: '#content',\r\n      " +
            "  plugins: 'bbcode code a11ychecker advcode " +
            "casechange formatpainter linkchecker autolink " +
            "lists checklist media mediaembed pageembed" +
            " permanentpen powerpaste table advtable" +
            " tinycomments tinymcespellchecker image " +
            "imagetools',\r\n        toolbar: 'a11ycheck " +
            "addcomment showcomments casechange checklist" +
            " code formatpainter pageembed permanentpen table " +
            "link|image'\r\n                   " +
            " , images_upload_url: '@ViewBag.pathbase/Posts/UploadEdit/@(Context.Request.RouteValues[\"id\"])',\r\n       " +
            " automatic_uploads: true,\r\n        images_reuse_filename: true,\r\n        //images_upload_base_path: '~/AppData/AppData/Temp/'\r\n  " +
            "              })\r\n\r\n        " +
            "        </script>";
        public string ConvertFromHtmlToMarkUp(string htmlcode)
        {
            throw new NotImplementedException();
        }

        public string ConvertToHtml(string bbcodeValue)
        {
            try
            {
                string ap = null;

                if (CommonTools.isEmpty(bbcodeValue) == false)
                {
                    var html = BbCode.ToHtml(bbcodeValue);
                    ap = html;
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
