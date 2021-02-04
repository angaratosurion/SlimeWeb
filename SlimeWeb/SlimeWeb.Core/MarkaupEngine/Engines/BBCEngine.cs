using CodeKicker.BBCode;
using SlimeWeb.Core.MarkaupEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.MarkaupEngine.Engines
{
    class BBCEngine : IEngine
    {
        public string Name { get { return "BBCode Engine"; } }

        public string Version { get { return "1.0.0.0"; } }

        public string Render(string Content)
        {
            try
            { string ap=null ;

                if( CommonTools.isEmpty(Content)==false)
                {
                    //List<BBTag> bBTags = new List<BBTag>();

                    //BBCodeParser bBCodeParser = new BBCodeParser(bBTags );
                    ap=BBCode.ToHtml(Content);

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
