using Azure.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using ru.emlsoft.WMS.Data.EF;
using ru.emlsoft.WMS.Localization;
using System;
using System.Globalization;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

var VaultUri = Environment.GetEnvironmentVariable("VaultUri");
if (VaultUri != null)
{
    var keyVaultEndpoint = new Uri(VaultUri);
    builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
}

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
Register.RegisterBase(builder.Services, builder.Configuration);


// builder.Services.AddDbContext<db>(options => options.UseSqlServer(connectionString));

// builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddUserStore<UserStore>();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(c =>
{
    c.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    c.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(cfg =>
{
    cfg.LoginPath = "/Account/Login";
    cfg.LogoutPath = "/Account/Logout";
    cfg.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
    cfg.Cookie.Path = "/";
    cfg.Cookie.HttpOnly = true;
    //cfg.SlidingExpiration = true;
});

builder.Services.AddRazorPages().
    AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).
    AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider =
            (type, factory) => factory.Create(typeof(SharedResource));
    });
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPINSIGHTS_CONNECTIONSTRING"]);


var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseRequestLocalization(new RequestLocalizationOptions
{
    ApplyCurrentCultureToResponseHeaders = true
});

app.UseExceptionHandler("/Home/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();


// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

