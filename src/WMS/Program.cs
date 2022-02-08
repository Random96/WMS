using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using ru.EmlSoft.WMS.Data.Abstract.Identity;
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
// builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddUserStore<UserStore>();
builder.Services.AddIdentity<User, Role>().AddUserStore<UserStore>().AddRoleStore<RoleStore>().AddUserManager<Microsoft.AspNetCore.Identity.UserManager<User>>();
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
