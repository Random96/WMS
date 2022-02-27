using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ru.EmlSoft.WMS.Data.Abstract.Database;
using ru.EmlSoft.WMS.Data.Abstract.Identity;
using ru.EmlSoft.WMS.Data.Dto;
using WMS.Tools;

namespace WMS.Components
{
    [ViewComponent]
    public class MenuViewComponent : ViewComponent
    {
        private readonly ILogger<MenuViewComponent> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserStore _userStore;
        private readonly IWMSDataProvider _db;

        public MenuViewComponent(IWMSDataProvider db, IUserStore userStore, SignInManager<User> signInManager, ILogger<MenuViewComponent> logger)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userStore = userStore;
            _db = db;
        }

    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            // get current user 
            var user = await _userStore.GetUserAsync(_signInManager, cancellationToken);

            if (user == null || user.Id == 0)
                return View(Enumerable.Empty<MenuDto>());

            // get current db user
            var companyId = user.CompanyId;

            if (companyId == null)
                return View(Enumerable.Empty<MenuDto>());

            // get current roles
            var menus = await _db.GetEntityListAsync(user.Id, cancellationToken);

            // GetMemoryCache.Set("menu_" + UserDtoCache?.UserId, menus);
            return View(menus);

        }
    }
}
