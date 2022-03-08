using ru.emlsoft.WMS.Data.Dto;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ru.emlsoft.WMS.Data.Abstract.Database
{
    public interface IWMSDataProvider
    {
        Task<User> CreateCompanyAsync(int sid, string companyName, CancellationToken cancellationToken);

        Task<IEnumerable<MenuDto>> GetEntityListAsync(int sid, CancellationToken cancellationToken);
    }
}
