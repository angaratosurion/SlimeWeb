using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using SlimeWeb.Core.SDK.Interfaces;
using System;

namespace HelloWorld.Actions
{
    public class EndpointsAction : IUseEndpointsAction
    {
        int IUseEndpointsAction.Priority => 1;

      public   void Execute(IEndpointRouteBuilder endpointRouteBuilder, IServiceProvider serviceProvider)
        {
            try
            {

                endpointRouteBuilder.MapControllerRoute(
                        name: "default2",
                        pattern: "{controller=HelloWorld}");
                endpointRouteBuilder.MapRazorPages();

                //Console.WriteLine("testt HelloWorld.Actions.EndpointsAction");
            }
            catch (Exception ex )
            {
                Console.WriteLine(ex.ToString());

                throw;
            }
        }
    }
}
