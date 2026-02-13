using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthGuardCore.Context
{
    public class AuthGuardCoreContext : IdentityDbContext
    {
        public AuthGuardCoreContext(DbContextOptions<AuthGuardCoreContext> options) : base(options)
        {
        }
    }
}
