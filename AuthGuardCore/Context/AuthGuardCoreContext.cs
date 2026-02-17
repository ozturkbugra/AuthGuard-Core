using AuthGuardCore.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AuthGuardCore.Context
{
    public class AuthGuardCoreContext : IdentityDbContext<AppUser>
    {
        public AuthGuardCoreContext(DbContextOptions<AuthGuardCoreContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>()
             .HasOne(m => m.Sender)
             .WithMany()
             .HasForeignKey(m => m.SenderEmail)
             .HasPrincipalKey(u => u.Email)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.RecieverEmail)
                .HasPrincipalKey(u => u.Email)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
