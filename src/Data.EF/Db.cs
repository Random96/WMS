using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using ru.emlsoft.WMS.Data.Abstract.Personnel;
using ru.emlsoft.WMS.Data.Abstract.Storage;
using ru.emlsoft.WMS.Data.Dto;
using ru.emlsoft.WMS.Data.EF.Exceptions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ru.emlsoft.WMS.Data.EF
{
    internal class Db : DbContext, IWMSDataProvider
    {
        readonly string? _error;
        readonly bool _inited;
        private readonly ILogger<Db> _logger;
        protected readonly string _connectionString;

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
                _inited = false;
                _error = ex.Message;
                _logger.LogError(ex, "connectionString string= {_connectionString}", new[] { _connectionString });
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

            modelBuilder.Entity<Entity>().ToTable(nameof(Entity).ToUpper());
            modelBuilder.Entity<Entity>().HasMany(x => x.Entities).WithOne(x => x.ParentEntity).HasForeignKey(x => x.ParentId).IsRequired(false);
            modelBuilder.Entity<Entity>().Property(x => x.EntityType).IsRequired();

            /* modelBuilder.Entity<Entity>()
                .HasDiscriminator<int>("ENTITY_TYPE")
                .HasValue<Entity>(0)
                .HasValue<Cell>(1)
                .HasValue<Good>(2)
                .HasValue<Pack>(3)
                .HasValue<Pallet>(4)
                .HasValue<Row>(5)
                .HasValue<Storage>(6)
                .HasValue<Tier>(7); */

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
            modelBuilder.Entity<ScanCode>().HasOne(x => x.Pallet).WithOne(x => x.Code).HasForeignKey<Pallet>(x => x.CodeId);

            modelBuilder.Entity<Storage>().ToTable(nameof(Storage).ToUpper());
            modelBuilder.Entity<Storage>().Property(x => x.Name).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<Storage>().HasMany(x => x.Rows).WithOne(x => x.Storage).HasForeignKey(x => x.StorageId);

            modelBuilder.Entity<Row>().ToTable(nameof(Row).ToUpper());
            modelBuilder.Entity<Row>().Property(x => x.Code).IsRequired().HasMaxLength(8);
            modelBuilder.Entity<Row>().HasMany(x => x.Tiers).WithOne(x => x.Row).HasForeignKey(x => x.RowId);

            modelBuilder.Entity<Tier>().ToTable(nameof(Tier).ToUpper());
            modelBuilder.Entity<Tier>().Property(x => x.Code).IsRequired().HasMaxLength(8);
            modelBuilder.Entity<Tier>().HasMany(x => x.Cells).WithOne(x => x.Tier).HasForeignKey(x => x.TierId);

            modelBuilder.Entity<Cell>().ToTable(nameof(Cell).ToUpper());
            modelBuilder.Entity<ScanCode>().HasOne(x => x.Cell).WithOne(x => x.Code).HasForeignKey<Cell>(x => x.CodeId);

            modelBuilder.Entity<Person>().ToTable(nameof(Person).ToUpper());
            modelBuilder.Entity<Person>().Property(x => x.FirstName).HasMaxLength(200);
            modelBuilder.Entity<Person>().Property(x => x.MiddleName).HasMaxLength(200);
            modelBuilder.Entity<Person>().Property(x => x.LastName).HasMaxLength(200);
            modelBuilder.Entity<Person>().HasOne(x => x.Company).WithMany(x => x.Persons).HasForeignKey(x => x.CompanyId);

            var arr = modelBuilder.Model.GetEntityTypes().Select(x =>
                    new EntityList()
                    {
                        Name = x.GetTableName() ?? String.Empty,
                        Label = x.ClrType.GetCustomAttributes(false).Select(c => c as DisplayAttribute).FirstOrDefault(x => x != null)?.Description ?? String.Empty,
                        GroupLabel = x.ClrType.GetCustomAttributes(false).Select(c => c as DisplayAttribute).FirstOrDefault(x => x != null)?.Name ?? String.Empty
                    }
                ).
                Where(x => !string.IsNullOrWhiteSpace(x.Label)).ToArray();

            for (int i = 0; i < arr.Length; ++i)
                arr[i].Id = i + 1;

            modelBuilder.Entity<EntityList>().HasData(arr);

            base.OnModelCreating(modelBuilder);
        }

        public async Task<User> CreateCompanyAsync(int sid, string companyName, CancellationToken cancellationToken)
        {
            var user = Users.Find(sid);

            if (user == null)
                throw new Exception("ERROR_USER_NOT_FOUND");

            if (user.CompanyId != null)
                throw new Exception("ERROR_USER_ASSIGNED_TO_COMPANY");

            var isExists = await Companies.AnyAsync(x => x.Name == companyName, cancellationToken);

            if (isExists)
                throw new Exception("ERROR_COMPANY_ALREDY_EXIST");

            var newCompany = new Company() { Name = companyName };

            await Companies.AddAsync(newCompany, cancellationToken);

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
            }).ToListAsync(cancellationToken);

            await AccessRights.AddRangeAsync(adminRights, cancellationToken);

            var newPerson = new Person()
            {
                Company = newCompany,
                FirstName = user.LoginName,
                User = user
            };

            user.Person = newPerson;

            Appointments.Add(new Appointment() { Position = newAdmin, Person = newPerson, Company = newCompany, FromDate = DateTime.UtcNow });

            await SaveChangesAsync(cancellationToken);

            if (user.CompanyId == null)
            {
                user.CompanyId = newCompany.Id;

                await SaveChangesAsync(cancellationToken);
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

        // Access
        public DbSet<AccessRight> AccessRights => _inited ? Set<AccessRight>() : throw new DatabaseNotInitException(Error);
        public DbSet<Appointment> Appointments => _inited ? Set<Appointment>() : throw new DatabaseNotInitException(Error);

        // Storage
        public DbSet<Cell> Cells => _inited ? Set<Cell>() : throw new DatabaseNotInitException(Error);

        // identity
        public DbSet<Company> Companies => _inited ? Set<Company>() : throw new DatabaseNotInitException(Error);
        public DbSet<Abstract.Access.Entity> Entities => _inited ? Set<Abstract.Access.Entity>() : throw new DatabaseNotInitException(Error);
        public DbSet<EntityList> EntityLists => _inited ? Set<EntityList>() : throw new DatabaseNotInitException(Error);

        public string? Error => _error;
        public DbSet<Good> Goods => _inited ? Set<Good>() : throw new DatabaseNotInitException(Error);
        public DbSet<Logins> Logins => _inited ? Set<Logins>() : throw new DatabaseNotInitException(Error);
        public DbSet<Pack> Packs => _inited ? Set<Pack>() : throw new DatabaseNotInitException(Error);
        public DbSet<Pallet> Pallets => _inited ? Set<Pallet>() : throw new DatabaseNotInitException(Error);
        public DbSet<Position> Positions => _inited ? Set<Position>() : throw new DatabaseNotInitException(Error);
        public DbSet<ScanCode> ScanCodes => _inited ? Set<ScanCode>() : throw new DatabaseNotInitException(Error);
        public DbSet<Storage> Storages => _inited ? Set<Storage>() : throw new DatabaseNotInitException(Error);
        public DbSet<User> Users => _inited ? Set<User>() : throw new DatabaseNotInitException(Error);
    }
}