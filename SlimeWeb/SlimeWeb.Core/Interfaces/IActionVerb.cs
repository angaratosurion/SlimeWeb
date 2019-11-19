using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Interfaces
{
    public interface IActionVerb
    {
        #region Properties
        /// <summary>
        /// Gets the name of the verb.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the action.
        /// </summary>
        string Action { get; }

        /// <summary>
        /// Gets the controller.
        /// </summary>
        string Controller { get; }
        string Description { get; }
        /// <summary>
        /// Set's if it needs to be access by admins
        /// </summary>
        Boolean isAdminPalnel { get;  }
        /// <summary>
        /// Name of the Module
        /// </summary>
        String ModuleName { get; }
        /// <summary>
        /// Description of the Module 
        /// </summary>
        String Moduledescription { get;}
        #endregion
    }
}
