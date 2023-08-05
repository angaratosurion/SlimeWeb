using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers.Markups.Interfaces
{
    public interface IMarkupManager
    {
        public string ConvertToHtml(string markdowncode);
        public string ConvertFromHtmlToMarkUp(string htmlcode);
    }
}
