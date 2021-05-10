using Q101.BbCodeNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
   public  class BBCodeManager
    {
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
