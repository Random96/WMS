using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ru.emlsoft.WMS.Data.EF.Identity
{
    internal class RoleStore : IRoleStore
    {
        private bool disposedValue;
        private readonly Db? _db;
        private readonly ILogger<RoleStore> _logger;

        public RoleStore(ILogger<RoleStore> logger, Db db)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public SignInManager<User> ? SignInManager { get; set; }

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
        }

        // // TODO: переопределить метод завершения, только если "Dispose(bool disposing)" содержит код для освобождения неуправляемых ресурсов
        // ~RoleStore()
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

        public Task<IList<Claim>> GetClaimsAsync(Position role, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task AddClaimAsync(Position role, Claim claim, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimAsync(Position role, Claim claim, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> CreateAsync(Position role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(Position role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(Position role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(Position role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(Position role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(Position role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedRoleNameAsync(Position role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(Position role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Position> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Position> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            if (_db == null || disposedValue)
                throw new ObjectDisposedException(nameof(RoleStore));
            if (SignInManager != null)
            {
                var user = SignInManager.Context.User;

                if (user != null)
                {
                    // get current db user
                    if (int.TryParse(user.FindFirst(ClaimTypes.Sid)?.Value, out int sid))
                    {

                        var dbUser = await _db.Users.FindAsync(new object[] { sid }, cancellationToken);

                        if (dbUser != null)
                        {
                            var ret = await _db.Positions.AsNoTracking().SingleAsync(x => x.CompanyId == dbUser.CompanyId && x.Name != null &&
                                x.Name.ToUpper() == normalizedRoleName, cancellationToken);

                            return ret;
                        }
                    }
                }
            }

            return new Position();
        }

        public Task<IdentityResult> ValidateAsync(RoleManager<Position> manager, Position role)
        {
            throw new NotImplementedException();
        }
    }
}
