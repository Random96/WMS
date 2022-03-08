﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using ru.emlsoft.WMS.Data.EF.Identity;
using System;
using System.Linq;

namespace ru.emlsoft.WMS.Data.EF
{
    public static class Register
    {
        public static void RegisterBase(IServiceCollection services, ConfigurationManager configuration, ServiceLifetime injection = ServiceLifetime.Scoped)
        {
            services.AddIdentity<User, Position>().AddUserStore<UserStore>().AddRoleStore<RoleStore>()
                .AddUserManager<UserManager<User>>();

            Db factoryDb(IServiceProvider serviceProvider)
            {
                var log = serviceProvider.GetRequiredService<ILogger<Db>>();

                var db = new MsSqlDb(configuration.GetConnectionString("MsSqlLocalConnection"), log);

                return db;
            }

            object factory(IServiceProvider serviceProvider) => factoryDb(serviceProvider);

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
                    services.AddSingleton(typeof(Db), (Func<IServiceProvider, Db>)factoryDb);

                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient(typeof(IUserStore), typeof(UserStore));
                    services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
                    services.AddTransient(typeof(IWMSDataProvider), factory);
                    services.AddTransient(typeof(Db), (Func<IServiceProvider, Db>)factoryDb);
                    break;
            }
            /*
                        Func<IServiceProvider, Db> factoryDb = serviceProvider =>
                        {
                            var log = serviceProvider.GetRequiredService<ILogger<Db>>();

                            var db = new OracleDb(configuration.GetConnectionString("OracleConnection"), log);

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
            */
        }

    }
}
