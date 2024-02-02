using Microsoft.EntityFrameworkCore;
using GlobalServ.DataModels.Models;
using GlobalServ.Common.Enum;
using Microsoft.Extensions.Configuration;

namespace GlobalServ.DataModels
{
    public class GlobalServDbContext : DbContext
    {
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ServiceModel> Services { get; set; }
        public GlobalServDbContext()
        {

        }
        public GlobalServDbContext(DbContextOptions<GlobalServDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=GlobarServ;Trusted_Connection=True");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleModel>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Name).IsUnique();
                entity.HasMany(x => x.Users).WithOne(x => x.Role);
            });

            modelBuilder.Entity<RoleModel>().HasData(
                new RoleModel[] {
                    new RoleModel {Id = (int)RolesEnum.Admin, Name = "Admin"},
                    new RoleModel {Id = (int)RolesEnum.User, Name = "User"},
                    new RoleModel {Id = (int)RolesEnum.Guest, Name = "Guest"},
                }
            );

            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.HasKey(entity => entity.Id);
                entity.HasIndex(x => x.Email).IsUnique();
                entity.Property(x => x.Email).IsRequired();
                entity.HasOne(x => x.Role).WithMany(x => x.Users);
                entity.HasMany(x => x.Services);
            });
            modelBuilder.Entity<ServiceModel>(entity =>
            {
                entity.HasKey(entity => entity.Id);
                entity.HasOne(x => x.User).WithMany(x => x.Services);
            });
        }

    }
}
