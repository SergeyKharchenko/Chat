using System.Data;
using System.Data.Objects;
using Chat.Infrastructure.Abstract;
using Entities.Core.Concrete;
using Entities.Models;
using WebMatrix.WebData;

namespace Chat.Infrastructure.Concrete
{
    public class WebSecurityAuthorizationService : IAuthorizationService
    {
        private readonly ChatContext context;

        public WebSecurityAuthorizationService(ChatContext context)
        {
            this.context = context;
        }

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
            var user = context.Users.Find(WebSecurity.CurrentUserId);
            context.Entry(user).State = EntityState.Detached;
            return user;
        }

        public int GetCurrentUserId()
        {
            return WebSecurity.CurrentUserId;
        }
    }
}