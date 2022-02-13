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
        Task<T> AddAsync(T item, CancellationToken cancellationToken);

        public bool Any(IEnumerable<FilterObject> filters);
        public Task<bool> AnyAsync(IEnumerable<FilterObject> filters, CancellationToken cancellationToken);
        IEnumerable<T> GetList(IEnumerable<FilterObject> filters);
        Task<IEnumerable<T>> GetListAsync(FilterObject[] filterObjects, CancellationToken cancellationToken);
        Task UpdateAsync(T item, CancellationToken cancellationToken);
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);
    }
}
