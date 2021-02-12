﻿using Markdig;
using ReverseMarkdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
    public class MarkDownManager
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
        public string ConvertFromHtmlToMarkDwon(string htmlcode)
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
