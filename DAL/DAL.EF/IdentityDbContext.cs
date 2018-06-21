using System.Data.Entity;
using Core.Identity.Entities;

namespace DAL.EF
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            
        }

        //Пользователи
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<UserRole> Roles { get; set; }
        public DbSet<UserClaim> Claims { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("CORE_USERS")
                .HasKey(x => x.Id);

            modelBuilder.Entity<UserRole>()
                .ToTable("CORE_USER_ROLES")
                .HasKey(x => new
                {
                    x.UserId,
                    x.RoleId
                })
                .HasRequired(x => x.User);

            modelBuilder.Entity<UserClaim>()
                .ToTable("CORE_USER_CLAIMS")
                .HasKey(x => new
                    {
                        x.UserId,
                        x.ClaimType
                    })
                .HasRequired(x => x.User);

            modelBuilder.Entity<RolePermission>()
                .ToTable("CORE_ROLE_PERMISSIONS")
                .HasKey(x => new
                {
                    x.RoleId,
                    x.Permission
                });

            base.OnModelCreating(modelBuilder);
        }        
    }
}
