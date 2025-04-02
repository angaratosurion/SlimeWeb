using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers.Interfaces
{
    public interface IGeneralSettingsManager<T,T2> : IDataManager
    {
        T Details();
        bool Exists();
        Task Edit(T2 genset);
        Task ClearSettings();
    }

}
