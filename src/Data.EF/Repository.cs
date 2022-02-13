using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ru.EmlSoft.WMS.Data.Abstract.Database;
using ru.EmlSoft.WMS.Data.Abstract.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ru.EmlSoft.WMS.Data.EF
{
    public class Repository<T> : IRepository<T> where T : class, IHaveId
    {
        private bool disposedValue;
        private Db ? _db;
        private ILogger<Repository<T>> _logger;
        private static AutoMapper.MapperConfiguration _mapper_config = new AutoMapper.MapperConfiguration(cfg => { });

        public Repository(ILogger<Repository<T>> logger, Db db)
        {
            _logger = logger;

            _db = db ?? throw new ArgumentNullException(nameof(db));
        }


        public T Add(T item)
        {
            throw new NotImplementedException();
        }

        public async Task<T> AddAsync(T item, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Add async of item");
            if (_db == null || disposedValue)
                throw new Exception("Is disposed");

            await _db.Set<T>().AddAsync(item, cancellationToken);
            _logger.LogTrace("Item was added");

            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogTrace("Added was commited");

            return item;
        }

        public IEnumerable<T> GetList(IEnumerable<FilterObject> filters)
        {
            _logger.LogTrace("Get list of items");
            if (_db == null || disposedValue)
                throw new Exception("Is disposed");

            try
            {
                IQueryable<T> query = GetQuerable(_db.Set<T>().AsNoTracking(), filters);
                return query.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Method 'GetList' failed");
                return Array.Empty<T>();
            }
            finally
            {
                _logger.LogTrace("Method 'GetList' finished");
            }
        }

        public async Task<IEnumerable<T>> GetListAsync(FilterObject[] filterObjects, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Get list of items async");
            if (_db == null || disposedValue)
                throw new Exception("Is disposed");

            try
            {
                IQueryable<T> query = GetQuerable(_db.Set<T>().AsNoTracking(), filterObjects);

                var ret = await query.ToArrayAsync(cancellationToken);

                return ret;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Method 'GetListAsync' failed");
                return Array.Empty<T>();
            }
            finally
            {
                _logger.LogTrace("Method 'GetListAsync' finished");
            }
        }


        public bool Any(IEnumerable<FilterObject> filters)
        {
            if (_db == null || disposedValue)
                throw new Exception("Is disposed");

            _logger.LogTrace("Method 'Any' called...");
            try
            {
                IQueryable<T> query = GetQuerable(_db.Set<T>().AsNoTracking(), filters);
                return query.Any();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Method 'Any' failed");
                return false;
            }
            finally
            {
                _logger.LogTrace("Method 'Any' finished");
            }
        }

        public async Task<bool> AnyAsync(IEnumerable<FilterObject> filters, CancellationToken cancellationToken)
        {
            if (_db == null || disposedValue)
                throw new Exception("Is disposed");

            _logger.LogTrace("Method 'AnyAsync' called...");
            try
            {
                IQueryable<T> query = GetQuerable(_db.Set<T>().AsNoTracking(), filters);

                var ret = await query.AnyAsync(cancellationToken);

                return ret;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Method 'AnyAsync' failed");
                return false;
            }
            finally
            {
                _logger.LogTrace("Method 'AnyAsync' finished");
            }
        }

        public async Task UpdateAsync(T item, CancellationToken cancellationToken)
        {
            if (_db == null || disposedValue)
                throw new Exception("Is disposed");

            _logger.LogTrace("Method 'UpdateAsync' called...");
            try
            {
                if (item.Id == 0)
                    _db.Set<T>().Add(item);
                else
                {
                    var oldItem = _db.Set<T>().Find(item.Id);
                    
                    if (oldItem == null)
                        throw new Exception();

                    oldItem = _mapper_config.CreateMapper().Map<T, T>(item, oldItem);
                }

                await _db.SaveChangesAsync(cancellationToken);

                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Method 'UpdateAsync' failed");
            }
            finally
            {
                _logger.LogTrace("Method 'UpdateAsync' finished");
            }
        }

        public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            if (_db == null || disposedValue)
                throw new Exception("Is disposed");

            _logger.LogTrace("Method 'UpdateAsync' called...");
            try
            {


                var ret = await _db.Set<T>().FindAsync( new object[] { id },  cancellationToken : cancellationToken);

                return ret;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Method 'UpdateAsync' failed");
            }
            finally
            {
                _logger.LogTrace("Method 'UpdateAsync' finished");
            }

            return null;
        }


        private IQueryable<T> GetQuerable(IQueryable<T> source, IEnumerable<FilterObject> ? filters)
        {
            var actualFilters = filters?.ToArray() ?? Array.Empty<FilterObject>();

            var item = Expression.Parameter(typeof(T), "item");

            Expression<Func<T, bool>> ? filterExp = null;
            Expression<Func<T, bool>> ? lambda;

            foreach (var filter in actualFilters)
            {
                lambda = null;
                var property = Expression.Property(item, filter.PropertyName);

                switch (filter.Operation)
                {
                    case FilterOption.Equals:
                        {                          
                            if (filter.Comparation == StringComparison.CurrentCultureIgnoreCase && filter.Value is string strVal)
                            {
                                var value = Expression.Constant(strVal.ToUpper(), property.Type);

                                Type prop = typeof(string);
                                var methods = prop.GetMethods().Where(x => x.Name == "ToUpper").ToArray();
                                var method = methods.First(); //  prop? .GetMethod("Equals", new Type[] { prop, typeof(StringComparison) });
                                var upperProperty = Expression.Lambda(Expression.Call(property, method));
                                var expr = Expression.Equal(upperProperty.Body, value);
                                lambda = Expression.Lambda<Func<T, bool>>(expr, item);

                            }
                            else
                            {
                                var value = Expression.Constant(filter.Value, property.Type);
                                var expr = Expression.Equal(property, value);
                                lambda = Expression.Lambda<Func<T, bool>>(expr, item);
                            }
                        }
                        break;
                    case FilterOption.NotEquals:
                        {
                            var value = Expression.Constant(filter.Value, property.Type);
                            var expr = Expression.NotEqual(property, value);
                            lambda = Expression.Lambda<Func<T, bool>>(expr, item);
                        }
                        break;
                    case FilterOption.In:
                        {
                            var value = Expression.Constant(filter.Value);
                            var arrayType = GetArrayType(filter.Value);
                            if (arrayType != null)
                            {
                                var method = GetGenericMethod(typeof(Enumerable), "Contains",
                                    new[] { arrayType.Item2 },
                                    new[] { arrayType.Item1, arrayType.Item2 });
                                
                                if (method == null)
                                    throw new Exception("illegal method");

                                var expr = Expression.Call(method, value, property);
                                lambda = Expression.Lambda<Func<T, bool>>(expr, item);
                            }
                        }
                        break;
                    case FilterOption.NotIn:
                        {
                            var value = Expression.Constant(filter.Value);
                            var arrayType = GetArrayType(filter.Value);
                            if (arrayType != null)
                            {
                                var method = GetGenericMethod(typeof(Enumerable), "Contains",
                                    new[] { arrayType.Item2 },
                                    new[] { arrayType.Item1, arrayType.Item2 });

                                if (method == null)
                                    throw new Exception("illegal method");

                                var expr = Expression.Not(Expression.Call(method, value, property));
                                lambda = Expression.Lambda<Func<T, bool>>(expr, item);
                            }
                        }
                        break;
                    case FilterOption.GreaterThanOrEqual:
                        {
                            var value = Expression.Constant(filter.Value, property.Type);
                            var expr = Expression.GreaterThanOrEqual(property, value);
                            lambda = Expression.Lambda<Func<T, bool>>(expr, item);
                        }
                        break;
                    case FilterOption.LessThanOrEqual:
                        {
                            var value = Expression.Constant(filter.Value, property.Type);
                            var expr = Expression.LessThanOrEqual(property, value);
                            lambda = Expression.Lambda<Func<T, bool>>(expr, item);
                        }
                        break;
                }

                if (lambda != null)
                {
                    if (filterExp == null)
                        filterExp = lambda;
                    else
                    {
                        var andExp = Expression.AndAlso(filterExp.Body, lambda.Body);
                        filterExp = Expression.Lambda<Func<T, bool>>(andExp, item);
                    }
                }
            }
            //var spisok = source.Where(filterExp).ToList();
            //return source.Where(filterExp);
            return filterExp != null ? source.Where(filterExp) : source;
        }

        private static Tuple<Type, Type> ? GetArrayType(object value)
        {
            TypeInfo type = (TypeInfo)value.GetType();
            foreach (var interfaces in type.ImplementedInterfaces)
            {
                if (interfaces.IsGenericType && interfaces.GenericTypeArguments.Any())
                {
                    return new Tuple<Type, Type>(interfaces, interfaces.GenericTypeArguments[0]);
                }
            }
            return null;
        }

        private static MethodInfo ? GetGenericMethod(Type type, string name, Type[] genericTypeArgs, Type[] paramTypes)
        {
            //выбираем у типа все методы
            var abstractGenericMethod = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            
            //фильтруем по имени
            abstractGenericMethod = abstractGenericMethod.Where(x => x.Name == name).ToArray();
            
            //интересуют только generic-методы
            abstractGenericMethod = abstractGenericMethod.Where(x => x.IsGenericMethod).ToArray();

            //выбираем параметры, которые принимает метод
            //отбрасываем методы принимающие иное количество параметров
            abstractGenericMethod = abstractGenericMethod.Where(x => x.GetParameters().Length == paramTypes.Length).ToArray();
            //создаем конкретный метод, указывая типы шаблона
            var concreteGenericMethod = abstractGenericMethod.Select(x => x.MakeGenericMethod(genericTypeArgs)).ToArray();
            //у созданого метода проверяем типы параметров, чтобы имеющиеся у нас типы могли быть назначены параметрам метода
            concreteGenericMethod = concreteGenericMethod.Where(x => x.GetParameters().Select(p => p.ParameterType).SequenceEqual(paramTypes, new TestAssignable())).ToArray();
            //выбираем первый удовлетворяющий всем условиям метод. 
            return concreteGenericMethod.FirstOrDefault();

            /*            var methods =
                            //выбираем у типа все методы
                            from abstractGenericMethod in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                                //фильтруем по имени
                            where abstractGenericMethod.Name == name
                            //интересуют только generic-методы
                            where abstractGenericMethod.IsGenericMethod
                            //выбираем параметры, которые принимает метод
                            let pa = abstractGenericMethod.GetParameters()
                            //отбрасываем методы принимающие иное количество параметров
                            where pa.Length == paramTypes.Length
                            //создаем конкретный метод, указывая типы шаблона
                            select abstractGenericMethod.MakeGenericMethod(genericTypeArgs) into concreteGenericMethod
                            //у созданого метода проверяем типы параметров, чтобы имеющиеся у нас типы могли быть назначены параметрам метода
                            where concreteGenericMethod.GetParameters().Select(p => p.ParameterType).SequenceEqual(paramTypes, new TestAssignable())
                            select concreteGenericMethod;
            */
            //выбираем первый удовлетворяющий всем условиям метод. 
            //return methods.FirstOrDefault();
        }

        private IEnumerable<OrderElement> PrepareOrder(IEnumerable<OrderElement> orderByField)
        {

            if (orderByField == null)
            {
                return new OrderElement[] { new OrderElement(Ordering.Asc, "Id") };
            }

            if (orderByField.Any(x => x.Name == "Id"))
                return orderByField;

            var ret = orderByField.ToList();

            ret.Add(new OrderElement(Ordering.Asc, "Id"));

            return ret;
        }

        private IQueryable<T> SetOrder(IQueryable<T> dbSet, IEnumerable<OrderElement> orderByField)
        {
            if (dbSet == null)
                throw new ArgumentNullException(nameof(dbSet));

            int i = 0;

            orderByField = PrepareOrder(orderByField);

            foreach (var order in orderByField)
            {
                string methodName = string.Empty;

                switch (order.Order)
                {
                    case Ordering.Asc:
                        {
                            if (i++ == 0)
                            {
                                methodName = "OrderBy";
                            }
                            else
                            {
                                methodName = "ThenBy";
                            }
                        }
                        break;
                    case Ordering.Dsc:
                        {
                            if (i++ == 0)
                            {
                                methodName = "OrderByDescending";
                            }
                            else
                            {
                                methodName = "ThenByDescending";
                            }
                        }
                        break;
                }

                var arg = Expression.Parameter(typeof(T), "x");

                Expression expr = arg;

                expr = Expression.Property(expr, order.Name);

                var lambda = Expression.Lambda(expr, arg);

                var method = GetGenericMethod(typeof(Queryable), methodName,
                    new[] { typeof(T), expr.Type },
                    new[] { dbSet.GetType(), lambda.GetType() });

                if (method == null)
                    return dbSet;

                var ret = method.Invoke(null, new object[] { dbSet, lambda });

                if (ret is IOrderedQueryable<T> retQuerable)
                    dbSet = retQuerable;
            }

            return dbSet;
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

    class TestAssignable : IEqualityComparer<Type>
    {
        public bool Equals(Type? x, Type? y)
        {
            if (x == null)
                throw new ArgumentNullException(nameof(x));

            //Если тип значение типа y может использовано в качестве значения типа x, то нас это устраивает
            return x.IsAssignableFrom(y);
        }
        public int GetHashCode(Type obj) => obj.GetHashCode();
    }

}

