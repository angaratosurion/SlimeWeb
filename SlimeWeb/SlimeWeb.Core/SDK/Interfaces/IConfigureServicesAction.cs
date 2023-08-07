namespace SlimeWeb.Core.SDK.Interfaces
{
    /// <summary>
    /// Describes an action that must be executed inside the ConfigureServices method of a web application's Startup class
    /// and might be used by the extensions to register any service inside the DI.
    /// </summary>
    public interface IConfigureServicesAction
    {
        /// <summary>
        /// Priority of the action. The actions will be executed in the order specified by the priority (from smallest to largest).
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Contains any code that must be executed inside the ConfigureServices method of the web application's Startup class.
        /// </summary>
        /// <param name="services">
        /// Will be provided by the ExtCore and might be used to register any service inside the DI.
        /// </param>
        /// <param name="serviceProvider">
        /// Will be provided by the ExtCore and might be used to get any service that is registered inside the DI at this moment.
        /// </param>
        public void Execute(IServiceCollection services, IServiceProvider serviceProvider);
    }
}
