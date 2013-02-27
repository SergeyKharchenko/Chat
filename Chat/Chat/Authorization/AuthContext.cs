using System.Data.Entity;

namespace Chat.Authorization
{
    public class AuthContext : DbContext
    {
        public AuthContext() : base("AuthContext")
        {
        }

        public DbSet<User> Users { get; set; }
    }
}