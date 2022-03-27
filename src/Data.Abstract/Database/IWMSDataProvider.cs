using ru.emlsoft.WMS.Data.Dto;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ru.emlsoft.WMS.Data.Abstract.Doc;

namespace ru.emlsoft.WMS.Data.Abstract.Database
{
    public interface IWMSDataProvider
    {
        Task<User> CreateCompanyAsync(int sid, CompanyDto company, CancellationToken cancellationToken);

        Task<IEnumerable<MenuDto>> GetEntityListAsync(int sid, CancellationToken cancellationToken);
        Task ApplyDocAsync(int docId, int userId, IEnumerable<StoreOrd> storeOrd, CancellationToken cancellationToken);
        Task<Doc.Doc> GetDocByIdAsync(int id, int userId, CancellationToken cancellationToken);
        void ClearDb();
    }
}
