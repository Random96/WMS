using ru.EmlSoft.WMS.Data.Abstract.Access;
using ru.EmlSoft.WMS.Data.Abstract.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ru.EmlSoft.WMS.Data.Abstract.Database
{
    public interface IWMSDataProvider
    {
        Task<User> CreateCompanyAsync(int sid, string companyName, CancellationToken cancellationToken);

        Task<IEnumerable<EntityList>> GetEntityListAsync(int sid, CancellationToken cancellationToken);
    }
}
