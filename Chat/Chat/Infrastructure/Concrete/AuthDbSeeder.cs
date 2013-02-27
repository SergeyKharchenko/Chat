﻿using System.Data.Entity;
using Chat.Filters;
using Entities.Authorization;
using Entities.Core;
using WebMatrix.WebData;
using System.Linq;

namespace Chat.Infrastructure.Concrete
{
    public class AuthDbSeeder : DropCreateDatabaseAlways<AuthContext>
    {
        protected override void Seed(AuthContext context)
        {       
            var initializeSimpleMembershipAttribute = new InitializeSimpleMembershipAttribute();
            initializeSimpleMembershipAttribute.OnActionExecuting(null);

            WebSecurity.CreateUserAndAccount("Sergey", "1234");
            var user = context.Users.First();

            context.Records.Add(new Record { Text = "lol", User = user });
        }
    }
}