using SlimeWeb.Core.SDK.Interfaces;

namespace HelloWorld.Actions
{
    public class AddMvcAction : IAddMvcAction
    {
        int IAddMvcAction.Priority => 1;

        void IAddMvcAction.Execute(IMvcCoreBuilder mvcBuilder, IServiceProvider serviceProvider)
        {
            try
            {
                mvcBuilder.AddControllersAsServices()
                    .AddRazorPages();
                 
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
