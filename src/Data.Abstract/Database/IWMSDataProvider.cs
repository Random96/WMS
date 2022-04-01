using ru.emlsoft.WMS.Data.Abstract.Doc;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using ru.emlsoft.WMS.Data.Dto;
using System;

namespace ru.emlsoft.WMS.Data.Abstract.Database
{
    public interface IWMSDataProvider
    {
        Task<User> CreateCompanyAsync(int sid, CompanyDto company, CancellationToken cancellationToken);

        Task<IEnumerable<MenuDto>> GetEntityListAsync(int sid, CancellationToken cancellationToken);
        Task ApplyDocAsync(int docId, int userId, IEnumerable<StoreOrd> storeOrd, CancellationToken cancellationToken);
        Task<Doc.Doc> GetDocByIdAsync(int id, int userId, CancellationToken cancellationToken);
        void ClearDb();

        string GetGoodName(int goodId);
        string GetCellCode(int cellId);
    }
}
