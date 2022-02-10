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

namespace ru.EmlSoft.WMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserStore _userStore;
        private readonly IHtmlLocalizer<SharedResource> _localizer;

        public AccountController(IHtmlLocalizer<SharedResource> localizer, ILogger<AccountController> logger, IUserStore userStore)
        {
            _logger = logger;
            _userStore = userStore;
            _localizer = localizer;
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
        public async Task<IActionResult> Login([FromBody] ru.EmlSoft.WMS.Data.Dto.UserDto sys)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claimsPrincipal = new ClaimsPrincipal();

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
            {
                IssuedUtc = DateTime.Now,
                IsPersistent = true,
                ExpiresUtc = DateTime.Now.AddDays(1),
            });


            return View(); // Content(item.ToJsonL());
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(Data.Dto.UserDto model, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Register user begin");

            if(!ModelState.IsValid)
            {
                return View(model);
            }

            // register new user
            try
            {
                var ret = await _userStore.CreateAsync(user: new User()
                {
                    LoginName = model?.UserName,
                    PasswordHash = model?.Passwd1?.ToMd5(),
                    Email = model?.Email,
                    Phone = model?.Phone
                },
                cancellationToken: cancellationToken);

                if(ret.Succeeded)
                    return RedirectToAction("Index", "Home");

                foreach (var err in ret.Errors)
                    ModelState.AddModelError(string.Empty, _localizer[err.Description].Value );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in register user");
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
