namespace SlimeWeb.Core.Managers.Interfaces
{
    public interface IAccessManager
    {
        Task<bool> DoesUserHasAccess(string username, string blogname);
        Task<bool> DoesUserHasAccess(string username);
    }

}
