using System.Data.Entity;

namespace Entities.Authorization
{
    public class AuthContext : DbContext
    {
        public AuthContext() : base("AuthContext")
        {
        }

        public DbSet<User> Users { get; set; }
    }
}