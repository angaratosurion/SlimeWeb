using SlimeWeb.Core.Managers.Markups.Interfaces;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;

namespace SlimeWeb.Core.Managers.Markups
{
    public class MarkUpManager
    {
        Dictionary<string, IMarkupManager> MarkupManagers = new Dictionary<string, IMarkupManager>();
        public MarkUpManager()
        {
            this.MarkupManagers.Add(enumMarkupEngine.BBCODE.ToString(),new BBCodeManager());
            this.MarkupManagers.Add(enumMarkupEngine.MARKDOWN.ToString(), new MarkDownManager());
            this.MarkupManagers.Add(enumMarkupEngine.QUIL.ToString(), new QuilDeltaManager());
        }
        public string ConvertToHtml(string markdowncode)
        {
            try
            {
                string ap = null;

                if (CommonTools.isEmpty(markdowncode) == false)
                {
                   var cmsengine= AppSettingsManager.GetAppWideCMSEngine();

                    if ( cmsengine!=null && MarkupManagers.ContainsKey(cmsengine))
                    {
                        IMarkupManager markupManager = MarkupManagers.GetValueOrDefault(cmsengine);
                        if ( markupManager!=null)
                        {
                            ap = markupManager.ConvertToHtml(markdowncode);
                        }
                    }
                }
                return ap;
            }
            catch (Exception ex)
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
                    var cmsengine = AppSettingsManager.GetAppWideCMSEngine();

                    if (cmsengine != null && MarkupManagers.ContainsKey(cmsengine))
                    {
                        IMarkupManager markupManager = MarkupManagers.GetValueOrDefault(cmsengine);
                        if (markupManager != null)
                        {
                            ap = markupManager.ConvertFromHtmlToMarkUp(htmlcode);
                        }
                    }
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
