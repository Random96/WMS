using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ru.EmlSoft.WMS.Data.Abstract.Database;
using ru.EmlSoft.WMS.Data.Abstract.Identity;
using ru.EmlSoft.WMS.Models;
using System.Diagnostics;
using System.Security.Claims;
using WMS.Tools;

namespace ru.EmlSoft.WMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserStore _userStore;
        private readonly IWMSDataProvider _db;

        public HomeController(IWMSDataProvider db, IUserStore userStore, SignInManager<User> signInManager, ILogger<HomeController> logger)
        {
            _logger = logger;
            _userStore = userStore;
            _signInManager = signInManager;
            _db = db;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            _logger.LogTrace("home/index begin");

            // get current user 
            var user = await _userStore.GetUserAsync(_signInManager, cancellationToken);
            
            if (user.Id != 0)
            {
                // get current db user
                var companyId = user.CompanyId;
                
                if (companyId == null)
                    return RedirectToAction("CreateCompany", "Account");

                // get current roles
                var menus = await _db.GetEntityListAsync(user.Id, cancellationToken);

                // GetMemoryCache.Set("menu_" + UserDtoCache?.UserId, menus);

                ViewData["menu"] = menus;
                return View();

            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}