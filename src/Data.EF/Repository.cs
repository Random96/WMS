using Microsoft.Extensions.Logging;
using ru.EmlSoft.WMS.Data.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ru.EmlSoft.WMS.Data.EF
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private bool disposedValue;
        private Db ? _db;
        private ILogger<Repository<T>> _logger;

        public Repository(ILogger<Repository<T>> logger, Db db)
        {
            _logger = logger;

            _db = db ?? throw new ArgumentNullException(nameof(db));
        }


        public T Add(T item)
        {
            throw new NotImplementedException();
        }

        public async Task<T> AddAsync(T item)
        {
            _logger.LogTrace("Add async of item");
            if (_db == null || disposedValue)
                throw new Exception("Is disposed");

            await _db.Set<T>().AddAsync(item);
            _logger.LogTrace("Item was added");

            await _db.SaveChangesAsync();
            _logger.LogTrace("Added was commited");

            return item;
        }

        public IEnumerable<T> GetList()
        {
            _logger.LogTrace("Get list of items");
            if (_db == null || disposedValue)
                throw new Exception("Is disposed");

            return _db.Set<T>().ToArray();
        }

        public bool Any(IEnumerable<FilterObject> filters)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(IEnumerable<FilterObject> filters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                _db = null;

                disposedValue = true;
            }
        }

        // // TODO: переопределить метод завершения, только если "Dispose(bool disposing)" содержит код для освобождения неуправляемых ресурсов
        // ~Repository()
        // {
        //     // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
