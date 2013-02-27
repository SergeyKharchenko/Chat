using System.Data.Entity;
using Entities.Core;

namespace Entities.Authorization
{
    public class AuthContext : DbContext
    {
        public AuthContext() : base("AuthContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Record> Records { get; set; }
    }
}