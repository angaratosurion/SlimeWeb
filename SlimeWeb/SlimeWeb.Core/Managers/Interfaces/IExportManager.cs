namespace SlimeWeb.Core.Managers.Interfaces
{
    public interface IExportManager
    {
        public String  Name { get;  }
        public void Export(string filename);
    }
}
