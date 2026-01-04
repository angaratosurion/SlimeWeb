using Microsoft.Extensions.DependencyInjection;
using SlimeWeb.Core.SDK.Interfaces;
using System;

namespace HelloWorld.Actions
{
    public class AddMvcAction : IAddMvcAction
    {
        int IAddMvcAction.Priority => 1;

        void IAddMvcAction.Execute(IMvcBuilder mvcBuilder, IServiceProvider serviceProvider)
        {
            try
            {
                //mvcBuilder.AddControllersAsServices()
                //    .AddRazorPages();
                 
                Console.WriteLine("testt HelloWorld.Actions AddMvcAction");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                throw;
            }
        }
    }
}
