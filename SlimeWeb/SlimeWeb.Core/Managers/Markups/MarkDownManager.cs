using Markdig;
using ReverseMarkdown;
using SlimeWeb.Core.Managers.Markups.Interfaces;
using System;

namespace SlimeWeb.Core.Managers.Markups
{
    public class MarkDownManager : IMarkupManager
    {
        MarkdownPipeline pipeline;
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

    }
}
