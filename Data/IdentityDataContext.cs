using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace jwtAuthWebAPI.Data
{
    public class IdentityDataContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityDataContext(DbContextOptions<IdentityDataContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserLogin<Guid>>().HasNoKey();
            builder.Entity<IdentityUserClaim<Guid>>().HasNoKey();
            builder.Entity<IdentityUserToken<Guid>>().HasNoKey();
            builder.Entity<IdentityRoleClaim<Guid>>().HasNoKey();
             base.OnModelCreating(builder);
        }
    }
}
