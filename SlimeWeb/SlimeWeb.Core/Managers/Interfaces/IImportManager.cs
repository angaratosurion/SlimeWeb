namespace SlimeWeb.Core.Managers.Interfaces
{
    public interface IImportManager
    {
        public String Name { get; }
        public void Import(string filename);
    }
}
