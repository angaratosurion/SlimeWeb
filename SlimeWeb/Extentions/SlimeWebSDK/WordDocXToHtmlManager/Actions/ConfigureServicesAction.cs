using Microsoft.Extensions.DependencyInjection;
using SlimeWeb.Core.Managers.Managment;
using SlimeWeb.Core.SDK.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordDocXToHtmlManager.Managers;

namespace WordDocXToHtmlManager.Actions
{
    class ConfigureServicesAction : IConfigureServicesAction
    {
        int IConfigureServicesAction.Priority => 10;

        void IConfigureServicesAction.Execute(IServiceCollection services, IServiceProvider serviceProvider)
        {
            try
            {
                ManagerManagment.RegisterPostManager(new WordPostManager(),
                    "WordPostManager"
                    );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                throw;
            }
        }
    }
}
