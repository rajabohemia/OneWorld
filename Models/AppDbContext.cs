using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OneWorld.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var defaultdate = "GETDATE()";
            var seqid = "NEWSEQUENTIALID()";
            base.OnModelCreating(builder);

            //Seed Admin
            SeedAdmin(builder);
            
            //RefreshToken
            builder.Entity<RefreshToken>().Property(x => x.AddedDate).HasDefaultValueSql(defaultdate);
            builder.Entity<RefreshToken>().Property(x => x.Id).HasDefaultValueSql(seqid);

            //Service
            builder.Entity<Service>().Property(x => x.ServiceId).HasDefaultValueSql(seqid);
            builder.Entity<Service>().Property(x => x.DateStamp).HasDefaultValueSql(defaultdate);
            
        }

        private void SeedAdmin(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().Property(x => x.DateStamp).HasDefaultValueSql("getdate()");

            //Seed Application User
            var email = "hackstr@outlook.com";
            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "A6AE190D-3A8C-4CEF-BE35-2DEF2A582408",
                    UserName = email,
                    NormalizedUserName = email.ToUpper(),
                    Email = email,
                    NormalizedEmail = email.ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash =
                        "AQAAAAEAACcQAAAAEAy+lJePIeTEdC3QC4t8ZaN0ClmMfy/AhIawuSv7qqoXt2soGwABBcsGSViwxGu+tg=="
                });

            //Seed Roles
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "730C3F0F-F86C-4DE9-86A6-29A2B445DB36",
                    Name = ApplicationRoles.Admin.ToString(),
                    NormalizedName = ApplicationRoles.Admin.ToString().ToUpper()
                },
                new IdentityRole
                {
                    Id = "5FC21AEE-2353-4E9E-A534-45881E47D7A5",
                    Name = ApplicationRoles.User.ToString(),
                    NormalizedName = ApplicationRoles.User.ToString().ToUpper()
                });

            //Seed UserRoles
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = "A6AE190D-3A8C-4CEF-BE35-2DEF2A582408",
                RoleId = "730C3F0F-F86C-4DE9-86A6-29A2B445DB36"
            });
        }
        
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Servicefaq> Servicefaqs { get; set; }
    }
}