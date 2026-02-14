using AuthGuardCore.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthGuardCore.Context
{
    public class AuthGuardCoreContext : IdentityDbContext<AppUser>
    {
        public AuthGuardCoreContext(DbContextOptions<AuthGuardCoreContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
