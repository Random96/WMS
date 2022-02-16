using ru.EmlSoft.WMS.Data.Abstract.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ru.EmlSoft.WMS.Data.Abstract.Database
{
    public interface IRepository<T> : IDisposable  where T : class, IHaveId 
    {
        T Add(T item);
        Task<T> AddAsync(T item, CancellationToken cancellationToken = default);

        public bool Any(IEnumerable<FilterObject> filters);
        public Task<bool> AnyAsync(IEnumerable<FilterObject> filters, CancellationToken cancellationToken = default);
        IEnumerable<T> GetList(IEnumerable<FilterObject> filters);
        Task<IEnumerable<T>> GetListAsync(FilterObject[] filterObjects, CancellationToken cancellationToken = default);
        Task UpdateAsync(T item, CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
