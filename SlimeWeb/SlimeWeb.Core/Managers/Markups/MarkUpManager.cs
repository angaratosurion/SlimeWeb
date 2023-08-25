using SlimeWeb.Core.Managers.Markups.Interfaces;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;

namespace SlimeWeb.Core.Managers.Markups
{
    public static class MarkUpManager
    {
       static Dictionary<string, IMarkupManager> MarkupManagers = new Dictionary<string, IMarkupManager>();
        public static void Init()
        {
            MarkupManagers.Add(enumMarkupEngine.BBCODE.ToString(),new BBCodeManager());
           MarkupManagers.Add(enumMarkupEngine.MARKDOWN.ToString(), new MarkDownManager());
            MarkupManagers.Add(enumMarkupEngine.QUIL.ToString(), new QuilDeltaManager());
        }
        public static void RegisterMarkupManager(string name , IMarkupManager markupManager)
        {
            try
            {

                string ap = null;

                if (CommonTools.isEmpty(name) == false && markupManager!=null)
                {
                    MarkupManagers.Add(name, markupManager);
                }
                 
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                 
            }

        }
        public static string ConvertToHtml(string markdowncode)
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
        public static string ConvertFromHtmlToMarkUp(string htmlcode)
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
