using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Entities;
using Entities.Authorization;
using WebMatrix.WebData;

namespace Chat.Infrastructure.Concrete
{
    public class SimpleMembershipInitializer
    {
        public SimpleMembershipInitializer()
        {
            Database.SetInitializer<ChatContext>(null);

            try
            {
                using (var context = new ChatContext())
                {
                    if (!context.Database.Exists())
                        ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                }

                WebSecurity.InitializeDatabaseConnection("ChatContext", "User", "UserId", "Login",
                                                         autoCreateTables: true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized.",
                                                    ex);
            }
        }
    }
}