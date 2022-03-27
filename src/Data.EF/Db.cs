using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Doc;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using ru.emlsoft.WMS.Data.Abstract.Personnel;
using ru.emlsoft.WMS.Data.Abstract.Storage;
using ru.emlsoft.WMS.Data.Dto;
using ru.emlsoft.WMS.Data.Dto.Doc;
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
                //Database.EnsureDeleted();
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
            modelBuilder.Entity<Entity>().Property(x => x.EntityType).IsRequired();
            modelBuilder.Entity<Entity>().Property(x => x.LastUpdated);

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
            modelBuilder.Entity<Good>().Property(x => x.Name).HasMaxLength(200);

            modelBuilder.Entity<Pack>().ToTable(nameof(Pack).ToUpper());
            modelBuilder.Entity<Pallet>().ToTable(nameof(Pallet).ToUpper());

            modelBuilder.Entity<ScanCode>().ToTable(nameof(ScanCode).ToUpper());
            modelBuilder.Entity<ScanCode>().HasMany(x => x.Packs).WithOne(x => x.Code).HasForeignKey(x => x.CodeId);
            modelBuilder.Entity<ScanCode>().HasMany(x => x.Goods).WithOne(x => x.Code).HasForeignKey(x => x.CodeId);
            modelBuilder.Entity<ScanCode>().HasOne(x => x.Pallet).WithOne(x => x.Code).HasForeignKey<Pallet>(x => x.CodeId);
            modelBuilder.Entity<ScanCode>().HasOne(x => x.Company).WithMany(x => x.Codes).HasForeignKey(x => x.CompanyId);

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

            // partner
            modelBuilder.Entity<Partner>().ToTable(nameof(Partner).ToUpper());
            modelBuilder.Entity<Partner>().Property(x => x.FullName).HasMaxLength(200);
            modelBuilder.Entity<Partner>().Property(x => x.Name).HasMaxLength(80);
            modelBuilder.Entity<Partner>().Property(x => x.OGRN).HasMaxLength(20);
            modelBuilder.Entity<Partner>().Property(x => x.INN).HasMaxLength(13);
            modelBuilder.Entity<Partner>().Property(x => x.KPP).HasMaxLength(13);
            modelBuilder.Entity<Partner>().HasIndex(x => new { x.CompanyId, x.Name });
            modelBuilder.Entity<Partner>().HasMany(x => x.Docs).WithOne(x => x.Partner).HasForeignKey(x => x.PartnerId);

            // documents
            modelBuilder.Entity<Doc>().ToTable(nameof(Doc).ToUpper());
            modelBuilder.Entity<Doc>().HasOne(x => x.Storage).WithMany().HasForeignKey(x => x.StorageId);
            modelBuilder.Entity<Doc>().Property(x => x.DocNumber).HasMaxLength(80);
            modelBuilder.Entity<Doc>().HasMany(x => x.DocSpecs).WithOne(x => x.Doc).HasForeignKey(x => x.DocId);
            modelBuilder.Entity<Doc>().HasOne(x => x.Input).WithOne().HasForeignKey<Input>(x => x.Id);
            modelBuilder.Entity<Doc>().HasOne(x => x.Accept).WithOne().HasForeignKey<Accept>(x => x.Id);

            modelBuilder.Entity<Input>().ToTable(nameof(Input).ToUpper());
            modelBuilder.Entity<Input>().HasOne(x => x.InputCell).WithMany().HasForeignKey(x => x.InputCellId);

            modelBuilder.Entity<Accept>().ToTable(nameof(Accept).ToUpper());
            // modelBuilder.Entity<Accept>().HasKey(x => x.DocId);

            modelBuilder.Entity<DocSpec>().ToTable(nameof(DocSpec).ToUpper());
            modelBuilder.Entity<DocSpec>().HasOne(x => x.Good).WithMany().HasForeignKey(x => x.GoodId);
            modelBuilder.Entity<DocSpec>().HasOne(x => x.Pallet).WithMany().HasForeignKey(x => x.PalletId);
            modelBuilder.Entity<DocSpec>().HasOne(x => x.ToCell).WithMany().HasForeignKey(x => x.ToCellId);
            modelBuilder.Entity<DocSpec>().HasOne(x => x.FromCell).WithMany().HasForeignKey(x => x.FromCellId);

            modelBuilder.Entity<Remains>().ToTable(nameof(Remains).ToUpper());
            modelBuilder.Entity<Remains>().HasOne(x => x.Good).WithMany().HasForeignKey(x => x.GoodId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Remains>().HasOne(x => x.Pallet).WithMany().HasForeignKey(x => x.PalletId);
            modelBuilder.Entity<Remains>().HasOne(x => x.Cell).WithMany().HasForeignKey(x => x.CellId);
            modelBuilder.Entity<Remains>().HasOne(x => x.StoreOrd).WithMany().HasForeignKey(x => x.StoreOrdId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StoreOrd>().ToTable(nameof(StoreOrd).ToUpper());
            modelBuilder.Entity<StoreOrd>().HasOne(x => x.Good).WithMany().HasForeignKey(x => x.GoodId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<StoreOrd>().HasOne(x => x.Pallet).WithMany().HasForeignKey(x => x.PalletId);
            modelBuilder.Entity<StoreOrd>().HasOne(x => x.Cell).WithMany().HasForeignKey(x => x.CellId);
            modelBuilder.Entity<StoreOrd>().HasOne(x => x.DocSpec).WithMany().HasForeignKey(x => x.DocSpecId).OnDelete(DeleteBehavior.NoAction);

            var arr = modelBuilder.Model.GetEntityTypes().Select(x =>
                    new EntityList()
                    {
                        Name = FirstUpperConvert(x.GetTableName()),
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

        private static string FirstUpperConvert(string? source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return string.Empty;

            return source.Substring(0, 1).ToUpper() + source.Substring(1).ToLower();
        }
        public async Task ApplyDocAsync(int docId, int userId, IEnumerable<StoreOrd> storeOrd, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;

            var user = Users.Find(userId);

            if (user == null)
                throw new Exception();

            if (user.CompanyId == null)
                throw new Exception();

            var company = Companies.Find(user.CompanyId);
            if (company == null)
                throw new Exception();
            bool cantNegative = !company.CanNegativeStocks;

            if (user == null)
                throw new Exception();

            if (user.CompanyId == null)
                throw new Exception();

            int companyId = user.CompanyId.Value;

            var doc = await Docs.FindAsync(new object[] { docId }, cancellationToken);

            if (doc == null || doc.CompanyId != companyId)
                throw new Exception("DocNotFound");

            if( doc.Accepted)
                throw new Exception("Doc alredy accepted");

            foreach (var item in storeOrd)
            {
                item.UserId = userId;
                item.CompanyId = companyId;

                var curentRemains = Remains.Where(x => x.Current && x.CompanyId == companyId && x.GoodId == item.GoodId && x.CellId == item.CellId && ((x.PalletId == null && item.PalletId == null) || (x.PalletId == item.PalletId)))
                    .SingleOrDefault();

                int curentQty = 0;
                if (curentRemains != null)
                {
                    curentRemains.Current = false;
                    curentRemains.UserId = userId;
                    curentQty = curentRemains.Qty;
                }

                var newRemains = new Remains()
                {
                    CompanyId = companyId,
                    GoodId = item.GoodId,
                    CellId = item.CellId,
                    Current = true,
                    Qty = curentQty + item.Qty,
                    PrevRemainsId = curentRemains?.Id,
                    UserId = userId,
                    PalletId = item.PalletId,
                    StoreOrd = item
                };

                if (cantNegative && newRemains.Qty < 0)
                    throw new Exception();

                Remains.Add(newRemains);
            }

            if (doc != null)
            {
                doc.UserId = userId;
                doc.LastUpdated = now;
                doc.Accepted = true;
            }
            await SaveChangesAsync(cancellationToken);
        }

        public async Task<User> CreateCompanyAsync(int sid, CompanyDto company, CancellationToken cancellationToken)
        {
            var user = Users.Find(sid);

            if (user == null)
                throw new Exception("ERROR_USER_NOT_FOUND");

            if (user.CompanyId != null)
                throw new Exception("ERROR_USER_ASSIGNED_TO_COMPANY");

            var isExists = await Companies.AnyAsync(x => x.Name == company.Name, cancellationToken);

            if (isExists)
                throw new Exception("ERROR_COMPANY_ALREDY_EXIST");

            var newCompany = new Company() { Name = company.Name, CanNegativeStocks = company.CanNegativeStocks };

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

            if (company.NeedSampleData)
            {
                // create store
                // create good
                // create input document
            }

            return Users.AsNoTracking().Single(x => x.Id == sid);
        }

        public async Task<Doc> GetDocByIdAsync(int id, int userId, CancellationToken cancellationToken)
        {
            var user = Users.Find(userId);

            if (user == null)
                throw new Exception();

            if (user.CompanyId == null)
                throw new Exception();


            var doc = await Docs.Include(x => x.DocSpecs).ThenInclude(x => x.Good)
                .Include(x => x.Input)
                .FirstOrDefaultAsync(x => x.CompanyId == user.CompanyId && x.Id == id, cancellationToken);
            if (doc == null)
                throw new Exception();

            return doc;
        }

        public Task<DocType> GetDocTypeAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<MenuDto>> GetEntityListAsync(int sid, CancellationToken cancellationToken)
        {
            var user = Users.Find(sid);

            if (user == null || user.PersonId == null)
                return Enumerable.Empty<MenuDto>();

            var ret = await Appointments.Where(x => x.PersonId == user.PersonId && DateTime.UtcNow >= x.FromDate && (x.ToDate == null || x.ToDate >= DateTime.UtcNow))
                .Select(x => x.Position).SelectMany(x => x.Rights).Where(x => x.CanRead)
                .Select(x => x.Entity).Distinct().AsNoTracking()
                .GroupBy(x => x.GroupLabel, x => new { x.Name, x.Label })
                .Select(x => new MenuDto(x.Key, x.Select(x => new MenuItemDto(x.Name, x.Label)))).ToArrayAsync(cancellationToken);

            return ret;

        }

        public void ClearDb()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        // Access
        public DbSet<AccessRight> AccessRights => _inited ? Set<AccessRight>() : throw new DatabaseNotInitException(Error);
        public DbSet<Appointment> Appointments => _inited ? Set<Appointment>() : throw new DatabaseNotInitException(Error);

        // Storage
        public DbSet<Cell> Cells => _inited ? Set<Cell>() : throw new DatabaseNotInitException(Error);

        // identity
        public DbSet<Company> Companies => _inited ? Set<Company>() : throw new DatabaseNotInitException(Error);


        public DbSet<Doc> Docs => _inited ? Set<Doc>() : throw new DatabaseNotInitException(Error);
        public DbSet<Abstract.Access.Entity> Entities => _inited ? Set<Abstract.Access.Entity>() : throw new DatabaseNotInitException(Error);
        public DbSet<EntityList> EntityLists => _inited ? Set<EntityList>() : throw new DatabaseNotInitException(Error);

        public string? Error => _error;
        public DbSet<Good> Goods => _inited ? Set<Good>() : throw new DatabaseNotInitException(Error);
        public DbSet<Logins> Logins => _inited ? Set<Logins>() : throw new DatabaseNotInitException(Error);
        public DbSet<Pack> Packs => _inited ? Set<Pack>() : throw new DatabaseNotInitException(Error);
        public DbSet<Pallet> Pallets => _inited ? Set<Pallet>() : throw new DatabaseNotInitException(Error);
        public DbSet<Position> Positions => _inited ? Set<Position>() : throw new DatabaseNotInitException(Error);
        public DbSet<Remains> Remains => _inited ? Set<Remains>() : throw new DatabaseNotInitException(Error);
        public DbSet<ScanCode> ScanCodes => _inited ? Set<ScanCode>() : throw new DatabaseNotInitException(Error);
        public DbSet<Storage> Storages => _inited ? Set<Storage>() : throw new DatabaseNotInitException(Error);
        public DbSet<User> Users => _inited ? Set<User>() : throw new DatabaseNotInitException(Error);
    }
}