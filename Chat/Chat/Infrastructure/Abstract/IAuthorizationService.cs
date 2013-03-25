using Chat.Models;

namespace Chat.Infrastructure.Abstract
{
    public interface IAuthorizationService
    {
        void Register(string login, string password);
        bool Login(string login, string password);
        void Logout();

        User GetCurrentUser();
        int GetCurrentUserId();

        void SaveImage(Image image);

        void Commit();
    }
}