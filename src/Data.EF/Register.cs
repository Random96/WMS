using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Doc;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using ru.emlsoft.WMS.Data.EF.Identity;
using System;
using System.Linq;

namespace ru.emlsoft.WMS.Data.EF
{
    public static class Register
    {
        private static Db OracleDbFactory(IServiceProvider serviceProvider, string connectionString)
        {
            return new OracleDb(connectionString, serviceProvider.GetRequiredService<ILogger<OracleDb>>());
        }

        private static Db MsSqlFactory(IServiceProvider serviceProvider, string connectionString)
        {
            return new MsSqlDb(connectionString, serviceProvider.GetRequiredService<ILogger<MsSqlDb>>());
        }

        public static void RegisterBase(IServiceCollection services, ConfigurationManager configuration, ServiceLifetime injection = ServiceLifetime.Scoped)
        {

            services.AddAutoMapper(option =>
            {
                option.AddProfile<DomainProfile>();
            });

            services.AddIdentity<User, Position>().AddUserStore<UserStore>().AddRoleStore<RoleStore>()
                .AddUserManager<UserManager<User>>();

            var dbConnect = configuration["Database"];

            Func<IServiceProvider, Db> factoryDb;

            switch (dbConnect)
            {
                case "Oracle":
                    {
                        string connectionString = configuration.GetConnectionString("OracleConnection");
                        factoryDb = x => OracleDbFactory(x, connectionString);
                    }
                    break;
                case "MsSqlLocal":
                    {
                        string connectionString = configuration.GetConnectionString("MsSqlLocalConnection");
                        factoryDb = x => MsSqlFactory(x, connectionString);
                    }
                    break;
                case "MsSqlServer":
                    {
                        string connectionString = configuration.GetConnectionString("MsSqlConnection");
                        factoryDb = x => MsSqlFactory(x, connectionString);
                    }
                    break;
                case "MsSqlCompact":
                    {
                        string connectionString = configuration.GetConnectionString("MsSqlCompactConnection");
                        factoryDb = x => MsSqlFactory(x, connectionString);
                    }
                    break;
                default:
                    throw new Exception("Illegal configuration");
            }

            object factory(IServiceProvider serviceProvider) => factoryDb(serviceProvider);

            switch (injection)
            {
                case ServiceLifetime.Scoped:
                    services.AddScoped(typeof(IUserStore), typeof(UserStore));
                    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    services.AddScoped(typeof(IWMSDataProvider), factory);
                    services.AddScoped(typeof(Db), factory);
                    services.AddScoped(typeof(DocStorage), typeof(DocStorage));
                    break;

                case ServiceLifetime.Singleton:
                    services.AddSingleton(typeof(IUserStore), typeof(UserStore));
                    services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
                    services.AddSingleton(typeof(IWMSDataProvider), factory);
                    services.AddSingleton(typeof(Db), factoryDb);
                    services.AddSingleton(typeof(DocStorage), typeof(DocStorage));
                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient(typeof(IUserStore), typeof(UserStore));
                    services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
                    services.AddTransient(typeof(IWMSDataProvider), factory);
                    services.AddTransient(typeof(Db), factoryDb);
                    services.AddTransient(typeof(DocStorage), typeof(DocStorage));
                    break;
            }
        }

    }
}
