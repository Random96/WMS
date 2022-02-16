using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using ru.EmlSoft.WMS.Data.Abstract.Access;
using ru.EmlSoft.WMS.Data.Abstract.Database;
using ru.EmlSoft.WMS.Data.Abstract.Identity;
using ru.EmlSoft.WMS.Data.EF;
using ru.EmlSoft.WMS.Entity.Identity;
using ru.EmlSoft.WMS.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization
    (options =>
    {
        options.ResourcesPath = "Resources";
    });

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    CultureInfo[] supportedCultures = new[]
    {
                    new CultureInfo("en"),
                    new CultureInfo("cn"),
                    new CultureInfo("ru")
                };

    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

RegisterBase(builder.Services, connectionString);

static void RegisterBase(IServiceCollection services, string connectionString, ServiceLifetime injection = ServiceLifetime.Scoped)
{
    Func<IServiceProvider, object> ss = (x)=> new object();
    switch (injection)
    {
        case ServiceLifetime.Scoped:
            services.AddScoped(typeof(IUserStore), typeof(UserStore));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IWMSDataProvider), (x) => new Db(connectionString));

            //TODO: написать регистрацию сервиса для доступа к данным
            services.AddScoped(typeof(Db), (x) => new Db(connectionString) );
            break;

        case ServiceLifetime.Singleton:
            services.AddSingleton(typeof(IUserStore), typeof(UserStore));
            services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
            services.AddSingleton(typeof(IWMSDataProvider), (x) => new Db(connectionString));
            services.AddSingleton(typeof(Db), (x) => new Db(connectionString));
            break;

        case ServiceLifetime.Transient:
            services.AddTransient(typeof(IUserStore), typeof(UserStore));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IWMSDataProvider), (x) => new Db(connectionString));
            services.AddTransient(typeof(Db), (x) => new Db(connectionString));
            break;
    }
}


// builder.Services.AddDbContext<db>(options => options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddUserStore<UserStore>();
builder.Services.AddIdentity<User, Position>().AddUserStore<UserStore>().AddRoleStore<RoleStore>()
    .AddUserManager<Microsoft.AspNetCore.Identity.UserManager<User>>();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(c =>
{
    c.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    c.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(cfg =>
{
    cfg.LoginPath = "/Login/Index";
    cfg.LogoutPath = "/Login/Logout";
    //cfg.ExpireTimeSpan = TimeSpan.FromDays(1);
    //cfg.Cookie.Expiration = TimeSpan.FromDays(1);
    cfg.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
    cfg.Cookie.Path = "/";
    cfg.Cookie.HttpOnly = true;
    //cfg.SlidingExpiration = true;
});

builder.Services.AddRazorPages().
    AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).
    AddDataAnnotationsLocalization(options => {
        options.DataAnnotationLocalizerProvider = 
            (type, factory) => factory.Create(typeof(SharedResource));
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

