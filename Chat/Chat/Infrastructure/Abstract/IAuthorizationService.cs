using Entities.Models;

namespace Chat.Infrastructure.Abstract
{
    public interface IAuthorizationService
    {
        void Register(string login, string password);

        bool Login(string login, string password);

        void Logout();

        int GetCurrentuserId();
    }
}