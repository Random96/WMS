using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using ru.emlsoft.WMS.Data.Dto;
using ru.emlsoft.WMS.Models;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using ru.emlsoft.WMS.Controllers;
using ru.emlsoft.WMS.Tools;


namespace ru.emlsoft.WMS.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IWMSDataProvider _db;


        public HomeController(IWMSDataProvider db, IUserStore userStore, SignInManager<User> signInManager, ILogger<HomeController> logger) :
            base(userStore, signInManager, logger)
        {
            _db = db;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogTrace("home/index begin");
                var ret = await CheckUserAsync(cancellationToken);

                return ret ?? View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Home/index");
                ViewBag.Error = $"Client Ip={await UserExtension.GetAddressAsync()}, ErrorMsg='{ex.Message}'";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> InitList(CancellationToken cancellationToken)
        {
            // get current user 
            var user = await _userStore.GetUserAsync(_signInManager, cancellationToken);

            if (user == null || user.Id == 0)
                return Json(Enumerable.Empty<MenuDto>());

            // get current db user
            var companyId = user.CompanyId;

            if (companyId == null)
                return Json(Enumerable.Empty<MenuDto>());

            // get current roles
            var menus = await _db.GetEntityListAsync(user.Id, cancellationToken);

            // GetMemoryCache.Set("menu_" + UserDtoCache?.UserId, menus);
            return Json(menus);
        }


        public IActionResult Privacy()
        {
            return View();
        }
    }
}