using Chat.Infrastructure.Abstract;
using Entities.Core.Concrete;
using Entities.Models;
using WebMatrix.WebData;

namespace Chat.Infrastructure.Concrete
{
    public class WebSecurityAuthorizationService : IAuthorizationService
    {
        private readonly ChatContext context = new ChatContext();

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

        public User GetCurrentUser()
        {
            return context.Users.Find(WebSecurity.CurrentUserId);
        }
    }
}