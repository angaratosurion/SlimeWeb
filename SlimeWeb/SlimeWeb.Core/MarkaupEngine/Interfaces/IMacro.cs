using SlimeWeb.Core.MarkaupEngine.Compilation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlimeWeb.Core.MarkaupEngine.Interfaces
{
    public interface IMacro
    {
        /// <summary>
        /// Gets the id of the macro.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the list of rules for the macro.
        /// </summary>
        IList<MacroRule> Rules { get; }
    }
}
