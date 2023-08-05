using ExtCore.Infrastructure.Actions;
using Microsoft.AspNetCore.Authentication;
using SlimeWeb.Core.Tools;

namespace SlimeWeb.OathLogin.Actions
{
	public class ConfigureServicesAction : IConfigureServicesAction
	{
		public int Priority => 1000;

		public void Execute(IServiceCollection services, IServiceProvider serviceProvider)
		{
			try
			{
                //  Microsoft.AspNetCore.Authentication.AuthenticationService

              
            }
            catch (Exception ex)
			{
				CommonTools.ErrorReporting(ex);

			}
		}
	}
	}
