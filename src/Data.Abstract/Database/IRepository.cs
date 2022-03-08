using System;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Abstract.Database
{
    public interface IRepository<T> : IDisposable where T : class, IHaveId
    {
        T Add(T item);
        Task<T> AddAsync(T item, CancellationToken cancellationToken = default);

        public bool Any(IEnumerable<FilterObject> filters);
        public Task<bool> AnyAsync(IEnumerable<FilterObject> filters, CancellationToken cancellationToken = default);
        IEnumerable<T> GetList(IEnumerable<FilterObject> filters, IEnumerable<OrderElement>? orderByField, bool includeProperties = false);
        Task<IEnumerable<T>> GetListAsync(IEnumerable<FilterObject> filters, IEnumerable<OrderElement>? orderByField, CancellationToken cancellationToken, bool includeProperties = false);
        Task<T> UpdateAsync(T item, CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ICollection<T>> GetPageAsync(int pageNum, int pageSize, IEnumerable<FilterObject> filters, IEnumerable<OrderElement>? orderByField, CancellationToken cancellationToken, bool includeProperties = false);

        int UserId { get; set; }
    }
}
