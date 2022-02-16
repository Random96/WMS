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

        public HomeController(IUserStore userStore, SignInManager<User> signInManager, ILogger<HomeController> logger)
        {
            _logger = logger;
            _userStore = userStore;
            _signInManager = signInManager;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            _logger.LogTrace("home/index begin");

            // get curent user 
            var user = await _userStore.GetUserAsync(_signInManager, cancellationToken);
            
            if (user != null)
            {
                // get curent db user
                var companyId = user.CompanyId;
                
                if (companyId == null)
                    return RedirectToAction("CreateCompany", "Account");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}