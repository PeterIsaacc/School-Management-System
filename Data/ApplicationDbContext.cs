using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using SchoolManagementSystem.Repositories;

namespace SchoolManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<School> Schools { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        
            modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur =>  ur.UserRoleId );
        
                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
        
                userRole.HasOne(ur => ur.School)
                    .WithMany(s => s.UserRoles)
                    .HasForeignKey(ur => ur.SchoolId)
                    .IsRequired(false);
        
                userRole.HasOne(ur => ur.Activity)
                    .WithMany(a => a.UserRoles)
                    .HasForeignKey(ur => ur.ActivityId)
                    .IsRequired(false);
        
                userRole.HasIndex(ur => new { ur.UserId, ur.RoleId, ur.SchoolId, ur.ActivityId })
                    .IsUnique()
                    .HasFilter("[SchoolId] IS NOT NULL AND [ActivityId] IS NOT NULL");
        
                userRole.HasIndex(ur => new { ur.UserId, ur.RoleId, ur.SchoolId })
                    .IsUnique()
                    .HasFilter("[SchoolId] IS NOT NULL AND [ActivityId] IS NULL");
        
                userRole.HasIndex(ur => new { ur.UserId, ur.RoleId })
                    .IsUnique()
                    .HasFilter("[SchoolId] IS NULL AND [ActivityId] IS NULL");
            });
        
            modelBuilder.Entity<Activity>()
                .HasOne(a => a.School)
                .WithMany(s => s.Activities)
                .HasForeignKey(a => a.SchoolId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=sms;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}