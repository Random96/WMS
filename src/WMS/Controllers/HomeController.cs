using Microsoft.AspNetCore.Mvc;
using ru.EmlSoft.WMS.Data.Abstract.Database;
using ru.EmlSoft.WMS.Data.Abstract.Identity;
using ru.EmlSoft.WMS.Models;
using System.Diagnostics;

namespace ru.EmlSoft.WMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<User> _repo;
        public HomeController(IRepository<User> repo, ILogger<HomeController> logger)
        {
            _logger = logger;
            _repo = repo;
        }

        public IActionResult Index()
        {
            // var cnt = _repo.GetList( new FilterObject[] { new FilterObject("LoginName", FilterOption.Equals, "AAA", StringComparison.CurrentCultureIgnoreCase ) } );

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}