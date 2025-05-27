
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SlimeMarkUpManager.Managers.MarkupManager;
using SlimeWeb.Core.Managers.Markups;
using SlimeWeb.Core.SDK.Interfaces;
using System;

namespace SlimeMarkUpManager.Actions
{
    public class ConfigureServicesAction : IConfigureServicesAction
    {
        public int Priority => 10;

        public void Execute(IServiceCollection services, 
            IServiceProvider serviceProvider)
        {
            try
            {
                 
              
                SlimeMarManager slimeExtensiveMarkDownManager = 
                    new SlimeMarManager();
                MarkUpManager.RegisterMarkupManager("SlimeMarkUp",
                    slimeExtensiveMarkDownManager);
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
