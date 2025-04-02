using HelloWorld.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SlimeWeb.Core.SDK.Interfaces;
using System;

namespace HelloWorld.Actions
{
    public class ConfigureServicesAction : IConfigureServicesAction
    {
        public int Priority => 10;

        public void Execute(IServiceCollection services, IServiceProvider serviceProvider)
        {
            try
            {
              

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
