using Q101.BbCodeNetCore;
using SlimeWeb.Core.Managers.Markups.Interfaces;
using SlimeWeb.Core.Tools;
using System;

namespace SlimeWeb.Core.Managers.Markups
{
    public  class BBCodeManager : IMarkupManager
    {
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
    }
}
