using Oracle.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ru.EmlSoft.WMS.Data.Abstract.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ru.EmlSoft.WMS.Data.Abstract.Access;
using ru.EmlSoft.WMS.Data.Abstract.Storage;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using ru.EmlSoft.WMS.Data.Abstract.Database;

namespace ru.EmlSoft.WMS.Data.EF
{
    public class Db : DbContext, IWMSDataProvider
    {
        private readonly string _connectionString;
        public Db(string connectionString)
        {
            _connectionString = connectionString;

            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Access
            modelBuilder.Entity<AccessRight>().ToTable(nameof(AccessRight).ToUpper());
            modelBuilder.Entity<AccessRight>().HasOne(x => x.Entity).WithMany(x => x.Rights).HasForeignKey(x => x.EntityListId);
            modelBuilder.Entity<AccessRight>().HasOne(x => x.Position).WithMany(x => x.Rights).HasForeignKey(x => x.PositionId);

            modelBuilder.Entity<Appointment>().ToTable(nameof(Appointment).ToUpper());
            modelBuilder.Entity<Appointment>().HasOne(x => x.Position).WithMany(x => x.Appointments).HasForeignKey(x => x.PositionId);

            modelBuilder.Entity<Abstract.Access.Entity>().ToTable(nameof(Abstract.Access.Entity).ToUpper());
            modelBuilder.Entity<Abstract.Access.Entity>().HasMany(x => x.Entities).WithOne(x => x.ParentEntity).HasForeignKey(x => x.ParentId).IsRequired(false);

            modelBuilder.Entity<EntityList>().ToTable(nameof(EntityList).ToUpper());
            modelBuilder.Entity<EntityList>().Property(x => x.Name).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<EntityList>().Property(x => x.Label).HasMaxLength(200);
            modelBuilder.Entity<EntityList>().HasIndex(x => x.Name).IsUnique();

            modelBuilder.Entity<Position>().ToTable(nameof(Position).ToUpper());
            modelBuilder.Entity<Position>().Property(x => x.Name).HasMaxLength(30);

            //Identity
            modelBuilder.Entity<Company>().ToTable(nameof(Company).ToUpper());
            modelBuilder.Entity<Company>().HasMany(x => x.Appointments).WithOne(x => x.Company).HasForeignKey(x => x.CompanyId);
            modelBuilder.Entity<Company>().HasMany(x => x.Rights).WithOne(x => x.Company).HasForeignKey(x => x.CompanyId);
            modelBuilder.Entity<Company>().HasMany(x => x.Positions).WithOne(x => x.Company).HasForeignKey(x => x.CompanyId);
            modelBuilder.Entity<Company>().HasMany(x => x.Users).WithOne(x => x.Company).HasForeignKey(x => x.CompanyId);
            modelBuilder.Entity<Company>().HasMany(x => x.Entities).WithOne(x => x.Company).HasForeignKey(x => x.CompanyId);
            modelBuilder.Entity<Company>().Property(x => x.Name).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<Company>().HasIndex(x => x.Name).IsUnique();

            modelBuilder.Entity<User>().ToTable(nameof(User).ToUpper());
            modelBuilder.Entity<User>().HasIndex(x => x.LoginName).IsUnique();
            modelBuilder.Entity<User>().Property(x => x.Email).HasMaxLength(300).IsRequired();
            modelBuilder.Entity<User>().Property(x => x.LoginName).HasMaxLength(60).IsRequired();
            modelBuilder.Entity<User>().Property(x => x.Phone).HasMaxLength(80);
            modelBuilder.Entity<User>().Property(x => x.PasswordHash).HasMaxLength(32);
            modelBuilder.Entity<User>().HasMany(x=>x.Logins).WithOne(x => x.User).HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Logins>().ToTable(nameof(Logins).ToUpper());
            modelBuilder.Entity<Logins>().HasOne(x => x.User).WithMany(x => x.Logins).HasForeignKey(x => x.UserId);
            modelBuilder.Entity<Logins>().Property(x => x.PasswordHash).HasMaxLength(32).IsRequired();
            modelBuilder.Entity<Logins>().HasIndex(x => new { x.UserId, x.PasswordHash, x.Date });
            modelBuilder.Entity<Logins>().Property(x => x.PasswordHash).HasMaxLength(32);

            // Storage
            modelBuilder.Entity<Cell>().ToTable(nameof(Cell).ToUpper());

            modelBuilder.Entity<Good>().ToTable(nameof(Good).ToUpper());
            modelBuilder.Entity<Good>().Property(x => x.Article).HasMaxLength(20);

            modelBuilder.Entity<Pack>().ToTable(nameof(Pack).ToUpper());
            modelBuilder.Entity<Pallet>().ToTable(nameof(Pallet).ToUpper());

            modelBuilder.Entity<ScanCode>().ToTable(nameof(ScanCode).ToUpper());
            modelBuilder.Entity<ScanCode>().HasMany(x => x.Packs).WithOne(x => x.Code).HasForeignKey(x => x.CodeId);
            modelBuilder.Entity<ScanCode>().HasMany(x => x.Goods).WithOne(x => x.Code).HasForeignKey(x => x.CodeId);
            modelBuilder.Entity<ScanCode>().HasMany(x => x.Pallets).WithOne(x => x.Code).HasForeignKey(x => x.CodeId);
            modelBuilder.Entity<ScanCode>().HasMany(x => x.Cells).WithOne(x => x.Code).HasForeignKey(x => x.CodeId);

            modelBuilder.Entity<Storage>().ToTable(nameof(Storage).ToUpper());
            modelBuilder.Entity<Storage>().Property(x => x.Name).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<Storage>().HasMany(x => x.Cells).WithOne(x => x.Storage).HasForeignKey(x => x.StorageId);


            var arr = modelBuilder.Model.GetEntityTypes().Select(x => new EntityList() { Name = x.GetTableName(), Label = x.ClrType.GetCustomAttributes(false).Select(c => c as DisplayAttribute).Where(x => x != null).FirstOrDefault()?.Description }).
                  Where(x => !string.IsNullOrWhiteSpace(x.Label)).ToArray();

            for (int i = 0; i < arr.Length; ++i)
                arr[i].Id = i + 1;

            modelBuilder.Entity<EntityList>().HasData(arr);

            base.OnModelCreating(modelBuilder);
        }

        // Access
        public DbSet<AccessRight> AccessRights => Set<AccessRight>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<Abstract.Access.Entity> Entities => Set<Abstract.Access.Entity>();
        public DbSet<EntityList> EntityLists => Set<EntityList>();
        public DbSet<Position> Positions => Set<Position>();

        // identity
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Logins> Logins => Set<Logins>();

        // Storage
        public DbSet<Cell> Cells => Set<Cell>();
        public DbSet<Good> Goods => Set<Good>();
        public DbSet<Pack> Packs => Set<Pack>();
        public DbSet<Pallet> Pallets => Set<Pallet>();
        public DbSet<ScanCode> ScanCodes => Set<ScanCode>();
        public DbSet<Storage> Storages => Set<Storage>();
        // public DbSet<>  => Set<>();
        // public DbSet<>  => Set<>();

        private bool IsChildOfEntity(IMutableEntityType entitytype, IMutableEntityType entytyType)
        {
            if (entitytype.BaseType == null)
                return false;
            if (entitytype.BaseType == entytyType)
                return true;

            return IsChildOfEntity(entitytype.BaseType, entytyType);
        }

        public async Task<User> CreateCompanyAsync(int sid, string companyName, CancellationToken cancellationToken)
        {
            var user = Users.Find(sid);

            if (user == null)
                throw new Exception("ERROR_USER_NOT_FOUND");

            if (user.CompanyId != null)
                throw new Exception("ERROR_USER_ASSIGNED_TO_COMPANY");

            var isExists = await Companies.AnyAsync(x => x.Name == companyName);

            if (isExists)
                throw new Exception("ERROR_COMPANY_ALREDY_EXIST");

            var newCompany = new Company() { Name = companyName };

            await Companies.AddAsync(newCompany);

            user.Company = newCompany;

            var newAdmin = new Position() { Name = "Admin", Company = newCompany, IsAdmin = true };

            var adminRights = await EntityLists.AsNoTracking().Select(x => new AccessRight()
            {
                Company = newCompany,
                Position = newAdmin,
                CanDelete = true,
                CanRead = true,
                CanWrite = true,
                EntityListId = x.Id
            }).ToListAsync();

            await AccessRights.AddRangeAsync(adminRights);

            Appointments.Add(new Appointment() { Position = newAdmin, UserId = sid, Company = newCompany, FromDate = DateTime.Now });

            await SaveChangesAsync();

            if (user.CompanyId == null)
            {
                user.CompanyId = newCompany.Id;

                await SaveChangesAsync();
            }

            return Users.AsNoTracking().Single(x => x.Id == sid);
        }
        public async Task<IEnumerable<EntityList>> GetEntityListAsync(int sid, CancellationToken cancellationToken)
        {
            var user = Users.Find(sid);

            if (user == null)
                throw new Exception("ERROR_USER_NOT_FOUND");

            var ret = await Appointments.Where(x => x.UserId == sid && DateTime.Now >= x.FromDate && ( x.ToDate == null || x.ToDate >= DateTime.Now ))
                .Select(x => x.Position).SelectMany(x=>x.Rights).Where(x=>x.CanRead).Select(x=>x.Entity).Distinct().AsNoTracking().ToArrayAsync();

            return ret;
        }
    }
}
