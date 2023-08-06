namespace SlimeWeb.Core.SDK.Interfaces
{
    public interface ISlimeServiceExtension
    {
        public void Execute(IServiceCollection services, IServiceProvider serviceProvider);
         
    }
}
