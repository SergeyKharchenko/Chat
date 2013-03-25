using System.Data;
using System.Data.Objects;
using Chat.Infrastructure.Abstract;
using Entities.Models;
using WebMatrix.WebData;
using System.Linq;

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
            var userdId = WebSecurity.CurrentUserId;
            if (userdId == -1)
                userdId = context.Users.Max(u => u.Id);
            var user = context.Users.Find(userdId);
            return user;
        }

        public int GetCurrentUserId()
        {
            return WebSecurity.CurrentUserId;
        }

        public void SaveImage(Image image)
        {
            context.Images.Add(image);
        }

        public void Commit()
        {
            context.SaveChanges();
        }
    }
}