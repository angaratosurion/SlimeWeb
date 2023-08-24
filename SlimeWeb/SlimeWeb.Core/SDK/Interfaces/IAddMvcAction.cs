// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
namespace SlimeWeb.Core.SDK.Interfaces
{
    /// <summary>
    ///  this part is based on ExtCore Framework https://github.com/ExtCore/ExtCore
    /// Describes an action that must be executed inside the AddMvc method and might be used by the extensions
    /// to configure the MVC.
    /// </summary>
    public interface IAddMvcAction
    {
        /// <summary>
        /// Priority of the action. The actions will be executed in the order specified by the priority (from smallest to largest).
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Contains any code that must be executed inside the AddMvc method.
        /// </summary>
        /// <param name="mvcBuilder">
        /// Will be provided by the ExtCore.Mvc and might be used to configure the MVC.
        /// </param>
        /// <param name="serviceProvider">
        /// Will be provided by the ExtCore.Mvc and might be used to get any service that is registered inside the DI at this moment.
        /// </param>
        void Execute(IMvcBuilder mvcBuilder, IServiceProvider serviceProvider);
    }
}
