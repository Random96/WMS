using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ru.EmlSoft.WMS.Data.Abstract.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ru.EmlSoft.WMS.Data.EF.Identity
{
    internal class UserStore : IUserStore// , IUserPasswordStore<User>
    {
        private Db? _db;
        private readonly ILogger<UserStore> _logger;
        private static AutoMapper.MapperConfiguration _mapper_config = new AutoMapper.MapperConfiguration(cfg => { });

        public UserStore(ILogger<UserStore> logger, Db db)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        private bool disposedValue;

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken = default)
        {
            if (_db == null || disposedValue)
                throw new ObjectDisposedException(nameof(UserStore));

            try
            {
                var normaldy = await GetNormalizedUserNameAsync(user, cancellationToken);

                // get exist user
                var exist = await FindByNameAsync(normaldy, cancellationToken);

                if (exist.Id != 0)
                {
                    return IdentityResult.Failed(new IdentityError[]
                    {
                        new IdentityError()
                        {
                            Description = "ERROR_USER_ALREDY_EXIST"
                        }
                    });
                }

                // create user
                await _db.Users.AddAsync(user, cancellationToken);

                await _db.SaveChangesAsync(cancellationToken);

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError[] { new IdentityError() { Description = ex.Message } });
            }
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            if (_db == null || disposedValue)
                throw new ObjectDisposedException(nameof(UserStore));

            try
            {
                // get exist user
                var existedUsers = _db.Users.Where(x => x.LoginName != null && x.LoginName.ToUpper() == normalizedUserName );

                var exists = await existedUsers.AnyAsync(cancellationToken);

                if (exists)
                    return existedUsers.Single();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR_FIND_BY_NAME_ASYNC");
                throw;
            }

            return new User();
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken = default)
        {
            if(user.LoginName != null)
                return await Task.FromResult(user.LoginName.ToUpper());

            return String.Empty;
        }

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken = default)
        {
            if (_db == null || disposedValue)
                throw new ObjectDisposedException(nameof(UserStore));

            try
            {
                var ret = await _db.Users.AnyAsync(x => x.Id == user.Id, cancellationToken);

                return user.Id.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR_FIND_BY_ID_ASYNC");
                throw;
            }
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken = default)
        {
            if (_db == null || disposedValue)
                throw new ObjectDisposedException(nameof(UserStore));

            try
            {
                var ret = await _db.Users.FindAsync(new object?[] { user.Id }, cancellationToken);

                if (ret != null && ret.LoginName != null)
                    return ret.LoginName;

                throw new Exception("USER_NOT_FOUND");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR_FIND_BY_ID_ASYNC");
                throw;
            }
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            if (_db == null || disposedValue)
                throw new ObjectDisposedException(nameof(UserStore));

            try
            {
                var exsistUser = await _db.Users.Where(x => x.Id == user.Id).Include(x => x.Logins).SingleAsync(default);

                exsistUser = _mapper_config.CreateMapper().Map(user, exsistUser);

                await _db.SaveChangesAsync(cancellationToken);

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR_UPDATE_USER_ASYNC");
                return IdentityResult.Failed(new IdentityError[] { new IdentityError() { Description = "Error od update" } });
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты)
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить метод завершения
                // TODO: установить значение NULL для больших полей
                disposedValue = true;
            }

            _db = null;
        }

        // // TODO: переопределить метод завершения, только если "Dispose(bool disposing)" содержит код для освобождения неуправляемых ресурсов
        // ~Store()
        // {
        //     // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetEmailAsync(User user, CancellationToken cancellationToken = default)
        {
            if (_db == null || disposedValue)
                throw new ObjectDisposedException(nameof(UserStore));

            try
            {
                var ret = await _db.Users.FindAsync(new object?[] { user.Id }, cancellationToken);

                return ret?.Email ?? string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR_FIND_BY_ID_ASYNC");
                throw;
            }
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default)
        {
            if (_db == null || disposedValue)
                throw new ObjectDisposedException(nameof(UserStore));

            var ret = new List<string>();
            try
            {
                var userDb = await _db.Users.FindAsync(new object?[] { user.Id }, cancellationToken);

                if (userDb == null || userDb.PersonId == null)
                    return ret;

                ret = await _db.Appointments.Where(x => x.PersonId == userDb.PersonId &&
                            x.FromDate >= DateTime.UtcNow && (x.ToDate == null || x.ToDate <= DateTime.UtcNow))
                    .Select(x => x.Position.Name).Where(x=>x!= null).Distinct().ToListAsync(cancellationToken);

                return ret;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR_FIND_BY_ID_ASYNC");
                throw;
            }
        }

        public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ClaimsPrincipal> CreateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken = default)
        {
            if (_db == null || disposedValue)
                throw new ObjectDisposedException(nameof(UserStore));

            if (user == null)
                return Array.Empty<Claim>();

            try
            {
                var userDb = await GetUserByIdAsync(user.Id, cancellationToken);

                if( userDb == null || userDb.LoginName == null)
                    return Array.Empty<Claim>();

                var ret = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, userDb.LoginName),
                        new Claim(ClaimTypes.Sid, userDb.Id.ToString()),
                        new Claim(ClaimTypes.Surname, userDb.LoginName)
                    };

                if (userDb.CompanyId != null)
                {
                    var companyIdstr = userDb.CompanyId.ToString();

                    if (companyIdstr != null)
                        ret.Add(new Claim("CompanyId", companyIdstr));
                }

                var roles = await GetRolesAsync(user, cancellationToken);
                if (roles != null)
                    ret.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

                return ret;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR_FIND_BY_ID_ASYNC");
                throw;
            }
        }

        public Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByIdAsync(int sid, CancellationToken cancellationToken = default)
        {
            if (_db == null || disposedValue)
                throw new ObjectDisposedException(nameof(UserStore));

            try
            {
                var ret = await _db.Users.FindAsync(new object?[] { sid }, cancellationToken);

                return ret ?? new User();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR_FIND_BY_ID_ASYNC");
                throw;
            }
        }
    }
}
