using Oracle.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ru.EmlSoft.WMS.Data.Abstract.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ru.EmlSoft.WMS.Data.EF
{
    public class Db : DbContext
    {
        public Db()
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle(@"User Id=WMS;Password=WMS;Data Source=OCI");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasMany(x => x.Users).WithMany(x => x.Roles);
            modelBuilder.Entity<Role>().HasIndex(x => new { x.Name, x.CompanyId }).IsUnique();

            modelBuilder.Entity<User>().HasIndex(x => x.LoginName).IsUnique();
            modelBuilder.Entity<User>().Property(x => x.Email).HasMaxLength(300).IsRequired();
            modelBuilder.Entity<User>().Property(x => x.LoginName).HasMaxLength(60).IsRequired();
            modelBuilder.Entity<User>().Property(x => x.Phone).HasMaxLength(80);
            modelBuilder.Entity<User>().Property(x => x.PasswordHash).HasMaxLength(32);

            modelBuilder.Entity<Logins>().HasOne(x => x.User).WithMany(x => x.Logins).HasForeignKey(x => x.UserId);
            modelBuilder.Entity<Logins>().Property(x => x.PasswordHash).HasMaxLength(32).IsRequired();
            modelBuilder.Entity<Logins>().HasIndex(x => new { x.UserId, x.PasswordHash, x.Date });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();

        public DbSet<Logins> Logins => Set<Logins>();
    }
}
