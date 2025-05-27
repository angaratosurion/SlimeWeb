using Newtonsoft.Json.Linq;
using Quill.Delta;
using SlimeMarkUp.Core;
using SlimeMarkUp.Core.Extensions;
using SlimeWeb.Core.Managers.Markups.Interfaces;
using SlimeWeb.Core.Tools;
using System;
using System.Management.Automation.Language;
using System.Text;

namespace SlimeMarkUpManager.Managers.MarkupManager
{
    public  class SlimeMarManager : IMarkupManager
    {
        MarkupParser parser = new MarkupParser(new List<IBlockMarkupExtension>
            {
                new HeaderExtension(),
                new ImageExtension(),
                new TableExtension(),
                new ListExtension(),
                new CodeBlockExtension(),
                new BlockquoteExtension(),
                new InlineStyleExtension(),
                new LinkExtension()
            });
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

        public string ConvertToHtml(string input)
        {
            try
            {
                string ap = null;
                 if ( CommonTools.isEmpty(input)==false)
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


                    // quildelta = quildelta.Replace("{\"ops\":", "");
                    // quildelta=quildelta.Remove(quildelta.Length - 1, 1);
                    //var deltaOps = JArray.Parse(quildelta);


                    // var htmlConverter = new HtmlConverter(deltaOps);



                    // ap = htmlConverter.Convert();
                    // ap += "<br/><h2>Made By SlimeMarkUpManager</h2>";
                    var elements = parser.Parse(input);

                    var renderer = new HtmlRenderer();
                    string html = renderer.Render(elements);
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
                    ap = "<b>ssssss</b>";
                }
                else
                {
                    ap = ap = "<b>sssssssssfffer</b>";
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
                    ap = "<b>ssssss</b>";
                }
                else
                {
                    ap =  "<b>sssssssssfffer</b>";
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
