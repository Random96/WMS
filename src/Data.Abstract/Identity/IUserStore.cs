using System;
using System.Collections.Generic;
using System.Text;

namespace ru.EmlSoft.WMS.Data.Abstract.Identity
{
    public interface IUserStore : Microsoft.AspNetCore.Identity.IUserStore<User>
    {
    }
}
