using Microsoft.AspNetCore.Identity;
using ru.EmlSoft.WMS.Data.Abstract.Identity;
using ru.EmlSoft.WMS.Data.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using ru.EmlSoft.WMS.Data.Abstract.Access;

namespace ru.EmlSoft.WMS.Entity.Identity
{
    public class UserStore : IUserStore// , IUserPasswordStore<User>
    {
        private IRepository<User>? _repo;
        private IRepository<Position>? _positionRepo;
        private IRepository<Appointment>? _appointmentRepo;
        private readonly ILogger<UserStore> _logger;

        public UserStore(ILogger<UserStore> logger, IRepository<User> repo, IRepository<Position> positionRepo, IRepository<Appointment> appointmentRepo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _positionRepo = positionRepo ?? throw new ArgumentNullException(nameof(positionRepo));
            _appointmentRepo = appointmentRepo ?? throw new ArgumentNullException(nameof(appointmentRepo));
        }

        private bool disposedValue;

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken = default)
        {
            if (_repo == null || disposedValue)
                throw new ObjectDisposedException(nameof(UserStore));

            try
            {
                // get exist user
                var exist = await _repo.AnyAsync(new FilterObject[] { new FilterObject(nameof(User.LoginName),
                    FilterOption.Equals, user.LoginName,
                    StringComparison.CurrentCultureIgnoreCase)}, cancellationToken);

                if (exist)
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
                await _repo.AddAsync(user, cancellationToken);

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

        public async Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            if (_repo == null || disposedValue)
                throw new ObjectDisposedException(nameof(UserStore));

            try
            {
                // get exist user
                var exist = await _repo.GetListAsync(new FilterObject[] { new FilterObject(nameof(User.LoginName),
                    FilterOption.Equals, normalizedUserName,
                    StringComparison.CurrentCultureIgnoreCase)}, cancellationToken, true);

                if (exist.Any())
                {
                    return exist.Single();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR_FIND_BY_NAME_ASYNC");
            }

            return null;
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(user.LoginName.ToUpper());
        }

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken = default)
        {
            if (_repo == null || disposedValue)
                throw new ObjectDisposedException(nameof(UserStore));

            try
            {
                var ret = await _repo.GetByIdAsync(user.Id, cancellationToken);

                return ret.Id.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR_FIND_BY_ID_ASYNC");
                throw;
            }
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken = default)
        {
            if (_repo == null || disposedValue)
                throw new ObjectDisposedException(nameof(UserStore));

            try
            {
                var ret = await _repo.GetByIdAsync(user.Id, cancellationToken);

                return ret.LoginName;
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
            if (_repo == null || disposedValue)
                throw new ObjectDisposedException(nameof(UserStore));

            try
            {
                await _repo.UpdateAsync(user, cancellationToken);

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR_FIND_BY_NAME_ASYNC");
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

            _repo = null;
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
            if (_repo == null || disposedValue)
                throw new ObjectDisposedException(nameof(UserStore));

            try
            {
                var ret = await _repo.GetByIdAsync(user.Id, cancellationToken);

                return ret.Email;
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
            if (_repo == null || _appointmentRepo == null || _positionRepo == null || disposedValue)
                throw new ObjectDisposedException(nameof(UserStore));

            try
            {
                var ret = await _repo.GetByIdAsync(user.Id, cancellationToken);

                var appoints = await _appointmentRepo.GetListAsync(new[] { new FilterObject(nameof(Appointment.UserId),
                    FilterOption.Equals, user.Id)}, cancellationToken);

                var positionIds = appoints.Where(x => x.FromDate >= DateTime.Now && (x.ToDate == null || x.ToDate <= DateTime.Now))
                    .Select(x => x.PositionId).Distinct().ToArray();

                var positionId = await _positionRepo.GetListAsync(new[] { new FilterObject(nameof(Appointment.UserId), FilterOption.In, positionIds) }, cancellationToken);

                return positionId.Select(x => x.Name).ToArray();
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
            if (_repo == null || disposedValue)
                throw new ObjectDisposedException(nameof(UserStore));

            if (user == null)
                return Array.Empty<Claim>();

            try
            {
                var userDb = await GetUserByIdAsync(user.Id, cancellationToken);

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
            if (_repo == null || disposedValue)
                throw new ObjectDisposedException(nameof(UserStore));

            try
            {
                var ret = await _repo.GetByIdAsync(sid, cancellationToken);

                return ret;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR_FIND_BY_ID_ASYNC");
                throw;
            }
        }
    }
}
