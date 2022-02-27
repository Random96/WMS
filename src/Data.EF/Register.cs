using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ru.EmlSoft.WMS.Data.Abstract.Database;
using ru.EmlSoft.WMS.Data.EF.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ru.EmlSoft.WMS.Data.Abstract.Identity;
using ru.EmlSoft.WMS.Data.Abstract.Access;
using Microsoft.Extensions.Configuration;

namespace ru.EmlSoft.WMS.Data.EF
{
    public static class Register
    {
        public static void RegisterBase(IServiceCollection services, ConfigurationManager configuration, ServiceLifetime injection = ServiceLifetime.Scoped)
        {
            services.AddIdentity<User, Position>().AddUserStore<UserStore>().AddRoleStore<RoleStore>()
                .AddUserManager<UserManager<User>>();

#if DEBUG
            Func<IServiceProvider, Db> factoryDb = serviceProvider =>
            {
                var log = serviceProvider.GetRequiredService<ILogger<Db>>();

                var db = new MsSqlDb(configuration.GetConnectionString("MsSqlLocalConnection"), log);

                return db;
            };

            Func<IServiceProvider, object> factory = serviceProvider => factoryDb(serviceProvider);

            switch (injection)
            {
                case ServiceLifetime.Scoped:
                    services.AddScoped(typeof(IUserStore), typeof(UserStore));
                    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    services.AddScoped(typeof(IWMSDataProvider), factory);
                    services.AddScoped(typeof(Db), factory);
                    break;

                case ServiceLifetime.Singleton:
                    services.AddSingleton(typeof(IUserStore), typeof(UserStore));
                    services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
                    services.AddSingleton(typeof(IWMSDataProvider), factory);
                    services.AddSingleton(typeof(Db), factoryDb);

                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient(typeof(IUserStore), typeof(UserStore));
                    services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
                    services.AddTransient(typeof(IWMSDataProvider), factory);
                    services.AddTransient(typeof(Db), factoryDb);
                    break;
            }
#else
            Func<IServiceProvider, Db> factoryDb = serviceProvider =>
            {
                var log = serviceProvider.GetRequiredService<ILogger<Db>>();

                var db = new OracleDb(configuration.GetConnectionString("MsSqlLocalConnection"), log);

                return db;
            };

            Func<IServiceProvider, object> factory = serviceProvider => factoryDb(serviceProvider);

            switch (injection)
            {
                case ServiceLifetime.Scoped:
                    services.AddScoped(typeof(IUserStore), typeof(UserStore));
                    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    services.AddScoped(typeof(IWMSDataProvider), factory);
                    services.AddScoped(typeof(Db), factory);
                    break;

                case ServiceLifetime.Singleton:
                    services.AddSingleton(typeof(IUserStore), typeof(UserStore));
                    services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
                    services.AddSingleton(typeof(IWMSDataProvider), factory);
                    services.AddSingleton(typeof(Db), factoryDb);

                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient(typeof(IUserStore), typeof(UserStore));
                    services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
                    services.AddTransient(typeof(IWMSDataProvider), factory);
                    services.AddTransient(typeof(Db), factoryDb);
                    break;
            }
#endif
        }

    }
}
