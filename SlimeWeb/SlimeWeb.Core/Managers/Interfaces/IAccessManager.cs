namespace SlimeWeb.Core.Managers.Interfaces
{
    public interface IAccessManager : IDataManager
    {
        Task<bool> DoesUserHasAccess(string username, string blogname);
        Task<bool> DoesUserHasAccess(string username);
    }

}
