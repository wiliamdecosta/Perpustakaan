using Microsoft.EntityFrameworkCore;
using Perpustakaan.Data.Entities;

namespace Perpustakaan.Configurations.Db
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<EndpointPath> Endpoints { get; set; }
        public DbSet<EndpointRole> EndpointRoles { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<ImageCover> ImageCovers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserRole)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.UserRole)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<EndpointPath>()
                .HasMany(e => e.EndpointRoles)
                .WithOne(er => er.EndpointPath)
                .HasForeignKey(er => er.EndpointId);

            modelBuilder.Entity<EndpointRole>()
                .HasOne(er => er.Role)
                .WithMany()
                .HasForeignKey(er => er.RoleId);
        }
    }
}
