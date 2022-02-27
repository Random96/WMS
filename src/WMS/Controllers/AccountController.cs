using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ru.EmlSoft.WMS.Data.Abstract.Identity;
using System.Security.Claims;
using ru.EmlSoft.Utilities;
using ru.EmlSoft.WMS.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Identity;
using ru.EmlSoft.WMS.Data.Abstract.Database;
using WMS.Tools;
using Microsoft.Extensions.Localization;

namespace ru.EmlSoft.WMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserStore _userStore;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly SignInManager<User> _signInManager;
        private IWMSDataProvider _db;

        public AccountController(IWMSDataProvider db, SignInManager<User> signInManager, IStringLocalizer<SharedResource> localizer, ILogger<AccountController> logger, IUserStore userStore)
        {
            _logger = logger;
            _userStore = userStore;
            _localizer = localizer;
            _signInManager = signInManager;
            _db = db;
        }

        //string returnUrl = null
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Data.Dto.UserDto model, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Login start");

            if (model == null)
                return View();

            try
            {
                // get user by name
                var userName = await _userStore.GetNormalizedUserNameAsync(GetUser(model), cancellationToken);
                var dbUser = await _userStore.FindByNameAsync(userName, cancellationToken);

                if (dbUser == null || dbUser.Id == 0)
                {
                    _logger.LogTrace("User not found");
                    ModelState.AddModelError(string.Empty, _localizer["ERROR_USER_NOT_FOUND"].Value);
                    return View(model);
                }

                if (dbUser.IsLocked)
                {
                    ModelState.AddModelError(string.Empty, _localizer["ERROR_USER_IS_LOCKED"].Value);
                    return View(model);
                }

                if (dbUser.Expired > DateTime.UtcNow)
                {
                    ModelState.AddModelError(string.Empty, _localizer["ERROR_USER_IS_EXPRIRED"].Value);
                    return View(model);
                }

                if (dbUser.LockedTo < DateTime.UtcNow)
                {
                    ModelState.AddModelError(string.Empty, _localizer["ERROR_USER_IS_TEMPORARY_LOCKED"].Value);
                    return View(model);
                }

                if (dbUser.PasswordHash != model?.Passwd1?.ToMd5())
                {
                    if (dbUser.Logins != null)
                    {
                        // check to lock
                        var lastLogins = dbUser.Logins.Where(x => x.Date >= DateTime.UtcNow.AddMinutes(-15)
                            && x.PasswordHash == dbUser.PasswordHash);

                        if (lastLogins.Count() > 10)
                        {
                            lastLogins = lastLogins.OrderByDescending(x => x.Date).Take(10);
                        }

                        if (!lastLogins.Any(x => x.Result == 0))
                        {
                            dbUser.LockedTo = DateTime.UtcNow.AddHours(1);
                        }
                    }

                    if (dbUser.Logins == null)
                        dbUser.Logins = new List<Logins>();

                    // save false login
                    dbUser.Logins.Add(new Logins() { PasswordHash = dbUser.PasswordHash, Date = DateTime.UtcNow, Result = 1 });
                    await _userStore.UpdateAsync(dbUser, cancellationToken);
                    return View(model);
                }

                // login success

                if (dbUser.Logins == null)
                    dbUser.Logins = new List<Logins>();

                dbUser.Logins.Add(new Logins() { PasswordHash = dbUser.PasswordHash, Date = DateTime.UtcNow, Result = 0 });
                await _userStore.UpdateAsync(dbUser, cancellationToken);

                await _signInManager.SignOutAsync();
                await _signInManager.Context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                var prop = new AuthenticationProperties
                {
                    IssuedUtc = DateTime.UtcNow,
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(1),
                };
                var claimsPrincipal = await _signInManager.ClaimsFactory.CreateAsync(dbUser);
                await _signInManager.Context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, prop);
                // await _signInManager.SignInAsync(dbUser, prop);
                /*
                if (!_signInManager.IsSignedIn(claimsPrincipal))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, dbUser.LoginName),
                        new Claim(ClaimTypes.Sid,dbUser.Id.ToString()),
                        new Claim(ClaimTypes.Surname,dbUser.LoginName),
                    };

                    if (dbUser.Roles != null)
                        claims.AddRange(dbUser.Roles.Select(x => new Claim(ClaimTypes.Role, x.Name)));

                    var claimsIdentitys = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    claimsPrincipal = new ClaimsPrincipal(claimsIdentitys);
                    await _signInManager.Context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, prop);

                }
                */
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in register user");
                ModelState.AddModelError(string.Empty, $"User Ip:{await UserExtension.GetAddrAsync()}, Message='{ex.Message}'");
                return View(model);
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(Data.Dto.UserDto model, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Register user begin");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // register new user
            try
            {
                var user = GetUser(model);
                var ret = await _userStore.CreateAsync(user: user, cancellationToken: cancellationToken);

                if (!ret.Succeeded)
                {
                    foreach (var err in ret.Errors)
                        ModelState.AddModelError(string.Empty, _localizer[err.Description].Value);
                }

                if (model.Company != null)
                {
                    var userName = await _userStore.GetNormalizedUserNameAsync(user, cancellationToken);

                    user = await _userStore.FindByNameAsync(userName, cancellationToken);

                    await _db.CreateCompanyAsync(user.Id, model.Company, cancellationToken);
                }
                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in register user");
                ModelState.AddModelError(string.Empty, $"User Ip:{await UserExtension.GetAddrAsync()}, Message='{ex.Message}'");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult CreateCompany()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCompany(Data.Dto.CompanyDto model, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Create Company begin");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = await _userStore.GetUserAsync(_signInManager, cancellationToken);

                if (model.Name != null)
                    await _db.CreateCompanyAsync(user.Id, model.Name, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in register user");
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            await _signInManager.Context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        private User GetUser(Data.Dto.UserDto model)
        {
            if (model == null)
                return new User();

            return new User()
            {
                LoginName = model.UserName,
                PasswordHash = model.Passwd1?.ToMd5(),
                Email = model.Email,
                Phone = model.Phone
            };
        }
    }
}