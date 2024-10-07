using SlimeWeb.Core.Data.DBContexts;

namespace SlimeWeb.Core.Managers.Interfaces
{
    public interface IImportManager
    {
        public String Name { get; }
        public void Import(SlimeDbContext dbContext, string filename);
    }
}
