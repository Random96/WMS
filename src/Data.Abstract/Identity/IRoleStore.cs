using ru.emlsoft.WMS.Data.Abstract.Access;
using System;
using System.Collections.Generic;
using System.Text;

namespace ru.emlsoft.WMS.Data.Abstract.Identity
{
    public interface IRoleStore : 
        Microsoft.AspNetCore.Identity.IRoleStore<Position>, 
        Microsoft.AspNetCore.Identity.IRoleClaimStore<Position>,
        Microsoft.AspNetCore.Identity.IRoleValidator<Position>
    {
    }
}
