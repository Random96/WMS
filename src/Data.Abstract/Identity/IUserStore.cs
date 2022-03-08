using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ru.emlsoft.WMS.Data.Abstract.Identity
{
    public interface IUserStore :
        Microsoft.AspNetCore.Identity.IUserStore<User>,
        Microsoft.AspNetCore.Identity.IUserPasswordStore<User>,
        Microsoft.AspNetCore.Identity.IUserEmailStore<User>,
        Microsoft.AspNetCore.Identity.IUserPhoneNumberStore<User>,
        Microsoft.AspNetCore.Identity.IUserRoleStore<User>,
        Microsoft.AspNetCore.Identity.IUserClaimsPrincipalFactory<User>,
        Microsoft.AspNetCore.Identity.IUserClaimStore<User>
    {
        Task<User> GetUserByIdAsync(int sid, CancellationToken cancellationToken);
    }
}
