using Chat.Infrastructure.Abstract;
using WebMatrix.WebData;

namespace Chat.Infrastructure.Concrete
{
    public class WebSecurityAuthorizationService : IAuthorizationService
    {
        public void Register(string login, string password)
        {
            WebSecurity.CreateUserAndAccount(login, password);
        }

        public bool Login(string login, string password)
        {
            return WebSecurity.Login(login, password, true);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }

        public int GetCurrentuserId()
        {
            return WebSecurity.CurrentUserId;
        }
    }
}