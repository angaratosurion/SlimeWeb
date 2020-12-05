using System;
using System.Collections.Generic;
using System.Text;

namespace SlimeWeb.Core.MarkaupEngine.Interfaces
{
    public interface IEngine
    {
        // <summary>
        /// Renders the  content using the statically registered macros and renderers.
        /// </summary>
        /// <param name="wikiContent">The  content to be rendered.</param>
        /// <returns>The rendered html content.</returns>
        string Render(string Content);

        /// <summary>
        /// Renders the  content using the a custom formatter with statically registered macros.
        /// </summary>
        /// <param name="wikiContent">The  content to be rendered.</param>
        /// <param name="formatter">The custom formatter used when rendering.</param>
        /// <returns>The rendered html content.</returns>
        //string Render(string Content, Formatter formatter);

        /// <summary>
        /// Renders the  content using the specified macros and statically registered renderers.
        /// </summary>
        /// <param name="wikiContent">The  content to be rendered.</param>
        /// <param name="macros">A collection of macros to be used when rendering.</param>
        /// <returns>The rendered html content.</returns>
        string Render(string Content, IEnumerable<IMacro> macros);

        /// <summary>
        /// Renders the  content using the specified renderers with statically registered macros.
        /// </summary>
        /// <param name="wikiContent">The  content to be rendered.</param>
        /// <param name="renderers">A collection of renderers to be used when rendering.</param>
        /// <returns>The rendered html content.</returns>
      //  string Render(string Content, IEnumerable<IRenderer> renderers);

        /// <summary>
        /// Renders the  content using the specified macros and renderers.
        /// </summary>
        /// <param name="wikiContent">The  content to be rendered.</param>
        /// <param name="macros">A collection of macros to be used when rendering.</param>
        /// <param name="renderers">A collection of renderers to be used when rendering.</param>
        /// <returns>The rendered html content.</returns>
     //   string Render(string Content, IEnumerable<IMacro> macros, IEnumerable<IRenderer> renderers);

        /// <summary>
        /// Renders the  content using the specified macros and custom formatter.
        /// </summary>
        /// <param name="wikiContent">The  content to be rendered.</param>
        /// <param name="macros">A collection of macros to be used when rendering.</param>
        /// <param name="formatter">The custom formatter used when rendering.</param>
        /// <returns>The rendered html content.</returns>
        //string Render(string Content, IEnumerable<IMacro> macros, Formatter formatter);
    }
}
