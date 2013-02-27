using System.Data.Entity;
using Chat.Filters;
using WebMatrix.WebData;
using AuthContext = Chat.Authorization.AuthContext;

namespace Chat.Infrastructure.Concrete
{
    public class AuthDbSeeder : DropCreateDatabaseAlways<AuthContext>
    {
        protected override void Seed(AuthContext context)
        {       
            var initializeSimpleMembershipAttribute = new InitializeSimpleMembershipAttribute();
            initializeSimpleMembershipAttribute.OnActionExecuting(null);

            WebSecurity.CreateUserAndAccount("Sergey", "1234");
        }
    }
}