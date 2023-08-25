using SlimeWeb.Core.SDK.Interfaces;

namespace HelloWorld.Actions
{
    public class ConfigureServicesAction : IConfigureServicesAction
    {
        public int Priority => 10;

        public void Execute(IServiceCollection services, IServiceProvider serviceProvider)
        {
            try
            {
                services.AddMvcCore().AddControllersAsServices()
                    .AddRazorPages();
                services.AddControllersWithViews();

                services.AddControllers();
                Console.WriteLine("testt");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                throw;
            }
        }
    }
}
