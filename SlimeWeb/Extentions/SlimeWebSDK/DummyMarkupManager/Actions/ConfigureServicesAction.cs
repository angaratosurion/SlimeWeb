 
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SlimeWeb.Core.Managers.Markups;
using SlimeWeb.Core.SDK.Interfaces;

namespace DummyMarkupManager.Actions
{
    public class ConfigureServicesAction : IConfigureServicesAction
    {
        public int Priority => 10;

        public void Execute(IServiceCollection services, IServiceProvider serviceProvider)
        {
            try
            {
                 
              
                DummyMarkupManager dummyMarkupManager = new DummyMarkupManager();
                MarkUpManager.RegisterMarkupManager("DummyMarkup",dummyMarkupManager);
                Console.WriteLine("testt HelloWorld.Actions ConfigureServicesAction");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                throw;
            }
        }
    }
}
