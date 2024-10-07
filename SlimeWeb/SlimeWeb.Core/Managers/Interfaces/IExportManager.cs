using SlimeWeb.Core.Data.DBContexts;

namespace SlimeWeb.Core.Managers.Interfaces
{
    public interface IExportManager
    {
        public String  Name { get;  }
        public void Export(SlimeDbContext dbContext,string filename);
    }
}
