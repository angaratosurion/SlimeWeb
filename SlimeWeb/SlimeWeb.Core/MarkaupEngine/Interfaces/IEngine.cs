using System;
using System.Collections.Generic;
using System.Text;

namespace SlimeWeb.Core.MarkaupEngine.Interfaces
{
    public interface IEngine
    {
        string Name { get; }
        string Version { get; }
        // <summary>
        /// Renders the  content using the statically registered macros and renderers.
        /// </summary>
        /// <param name="wikiContent">The  content to be rendered.</param>
        /// <returns>The rendered html content.</returns>
        string Render(string Content);
    }
       
}
