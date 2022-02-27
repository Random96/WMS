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
using Microsoft.Extensions.Logging;
using ru.EmlSoft.WMS.Data.Abstract.Personnel;
using ru.EmlSoft.WMS.Data.EF.Exceptions;
using ru.EmlSoft.WMS.Data.Dto;

namespace ru.EmlSoft.WMS.Data.EF
{
    internal class Db : DbContext, IWMSDataProvider
    {
        private readonly ILogger<Db> _logger;
        protected readonly string _connectionString;
        private bool _inited = false;
        private string? _error;
        public Db(string connectionString, ILogger<Db> logger)
        {
            _connectionString = connectionString;
            _logger = logger;

            try
            {
                // Database.EnsureDeleted();
                Database.EnsureCreated();

                _inited = true;
            }
            catch (Exception ex)
            {
                _error = ex.Message;
                _logger.LogError(ex, "Error creating data {0}", new[] { this.Database.GetConnectionString() });
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Access
            modelBuilder.Entity<AccessRight>().ToTable(nameof(AccessRight).ToUpper());
            modelBuilder.Entity<AccessRight>().HasOne(x => x.Entity).WithMany(x => x.Rights).HasForeignKey(x => x.EntityListId);
            modelBuilder.Entity<AccessRight>().HasOne(x => x.Position).WithMany(x => x.Rights).HasForeignKey(x => x.PositionId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Appointment>().ToTable(nameof(Appointment).ToUpper());
            modelBuilder.Entity<Appointment>().HasOne(x => x.Position).WithMany(x => x.Appointments).HasForeignKey(x => x.PositionId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Appointment>().HasOne(x => x.Person).WithMany(x => x.Appointments).HasForeignKey(x => x.PersonId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Abstract.Access.Entity>().ToTable(nameof(Abstract.Access.Entity).ToUpper());
            modelBuilder.Entity<Abstract.Access.Entity>().HasMany(x => x.Entities).WithOne(x => x.ParentEntity).HasForeignKey(x => x.ParentId).IsRequired(false);

            modelBuilder.Entity<EntityList>().ToTable(nameof(EntityList).ToUpper());
            modelBuilder.Entity<EntityList>().Property(x => x.GroupLabel).HasMaxLength(30);
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
            modelBuilder.Entity<Company>().HasMany(x => x.Users).WithOne(x => x.Company).HasForeignKey(x => x.CompanyId).IsRequired(false);
            modelBuilder.Entity<Company>().HasMany(x => x.Entities).WithOne(x => x.Company).HasForeignKey(x => x.CompanyId);
            modelBuilder.Entity<Company>().Property(x => x.Name).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<Company>().HasIndex(x => x.Name).IsUnique();

            modelBuilder.Entity<User>().ToTable(nameof(User).ToUpper());
            modelBuilder.Entity<User>().HasIndex(x => x.LoginName).IsUnique();
            modelBuilder.Entity<User>().Property(x => x.Email).HasMaxLength(300).IsRequired();
            modelBuilder.Entity<User>().Property(x => x.LoginName).HasMaxLength(60).IsRequired();
            modelBuilder.Entity<User>().Property(x => x.Phone).HasMaxLength(80);
            modelBuilder.Entity<User>().Property(x => x.PasswordHash).HasMaxLength(32);
            modelBuilder.Entity<User>().HasMany(x => x.Logins).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            modelBuilder.Entity<User>().HasOne(x => x.Person).WithOne(x => x.User).HasForeignKey<User>(x => x.PersonId).IsRequired(false);

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

            modelBuilder.Entity<Person>().ToTable(nameof(Person).ToUpper());
            modelBuilder.Entity<Person>().Property(x => x.FirstName).HasMaxLength(200);
            modelBuilder.Entity<Person>().Property(x => x.MiddleName).HasMaxLength(200);
            modelBuilder.Entity<Person>().Property(x => x.LastName).HasMaxLength(200);
            modelBuilder.Entity<Person>().HasOne(x => x.Company).WithMany(x => x.Persons).HasForeignKey(x => x.CompanyId);

            var arr = modelBuilder.Model.GetEntityTypes().Select(x => new EntityList()
            {
                Name = x.GetTableName(),
                Label = x.ClrType.GetCustomAttributes(false).Select(c => c as DisplayAttribute).FirstOrDefault(x => x != null)?.Description,
                GroupLabel = x.ClrType.GetCustomAttributes(false).Select(c => c as DisplayAttribute).FirstOrDefault(x => x != null)?.Name
            }).
                Where(x => !string.IsNullOrWhiteSpace(x.Label)).ToArray();

            for (int i = 0; i < arr.Length; ++i)
                arr[i].Id = i + 1;

            modelBuilder.Entity<EntityList>().HasData(arr);

            base.OnModelCreating(modelBuilder);
        }

        // Access
        public DbSet<AccessRight> AccessRights => _inited ? Set<AccessRight>() : throw new DatabaseNotInitException(Error);
        public DbSet<Appointment> Appointments => _inited ? Set<Appointment>() : throw new DatabaseNotInitException(Error);
        public DbSet<Abstract.Access.Entity> Entities => _inited ? Set<Abstract.Access.Entity>() : throw new DatabaseNotInitException(Error);
        public DbSet<EntityList> EntityLists => _inited ? Set<EntityList>() : throw new DatabaseNotInitException(Error);
        public DbSet<Position> Positions => _inited ? Set<Position>() : throw new DatabaseNotInitException(Error);

        // identity
        public DbSet<Company> Companies => _inited ? Set<Company>() : throw new DatabaseNotInitException(Error);
        public DbSet<User> Users => _inited ? Set<User>() : throw new DatabaseNotInitException(Error);
        public DbSet<Logins> Logins => _inited ? Set<Logins>() : throw new DatabaseNotInitException(Error);

        // Storage
        public DbSet<Cell> Cells => _inited ? Set<Cell>() : throw new DatabaseNotInitException(Error);
        public DbSet<Good> Goods => _inited ? Set<Good>() : throw new DatabaseNotInitException(Error);
        public DbSet<Pack> Packs => _inited ? Set<Pack>() : throw new DatabaseNotInitException(Error);
        public DbSet<Pallet> Pallets => _inited ? Set<Pallet>() : throw new DatabaseNotInitException(Error);
        public DbSet<ScanCode> ScanCodes => _inited ? Set<ScanCode>() : throw new DatabaseNotInitException(Error);
        public DbSet<Storage> Storages => _inited ? Set<Storage>() : throw new DatabaseNotInitException(Error);

        public string? Error => _error;

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

            var newPerson = new Person()
            {
                Company = newCompany,
                FirstName = user.LoginName,
                User = user
            };

            user.Person = newPerson;

            Appointments.Add(new Appointment() { Position = newAdmin, Person = newPerson, Company = newCompany, FromDate = DateTime.UtcNow });

            await SaveChangesAsync();

            if (user.CompanyId == null)
            {
                user.CompanyId = newCompany.Id;

                await SaveChangesAsync();
            }

            return Users.AsNoTracking().Single(x => x.Id == sid);
        }
        public async Task<IEnumerable<MenuDto>> GetEntityListAsync(int sid, CancellationToken cancellationToken)
        {
            var user = Users.Find(sid);

            if (user == null || user.PersonId == null)
                return Enumerable.Empty<MenuDto>();

            var ret = await Appointments.Where(x => x.PersonId == user.PersonId && DateTime.UtcNow >= x.FromDate && (x.ToDate == null || x.ToDate >= DateTime.UtcNow))
                .Select(x => x.Position).SelectMany(x => x.Rights).Where(x => x.CanRead).Select(x => x.Entity).Distinct().AsNoTracking().
                GroupBy(x => x.GroupLabel, x => new { x.Name, x.Label })
                .Select(x => new MenuDto(x.Key, x.Select(x => new MenuItemDto(x.Name, x.Label)))).ToArrayAsync(cancellationToken);

            return ret;

        }
    }
}