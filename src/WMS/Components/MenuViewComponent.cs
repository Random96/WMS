using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using ru.emlsoft.WMS.Data.Dto;
using ru.emlsoft.WMS.Tools;
using ru.emlsoft.WMS.Localization;

namespace WMS.Components
{
    [ViewComponent]
    public class MenuViewComponent : ViewComponent
    {
        private readonly ILogger<MenuViewComponent> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserStore _userStore;
        private readonly IWMSDataProvider _db;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public MenuViewComponent(IWMSDataProvider db, IUserStore userStore, SignInManager<User> signInManager, IStringLocalizer<SharedResource> localizer, ILogger<MenuViewComponent> logger)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userStore = userStore;
            _db = db;
            _localizer = localizer;
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

            foreach( var menu in menus)
            {
                menu.Name = _localizer[menu.Name];
                foreach ( var item in menu.Items)
                    item.Description = _localizer[item.Description];

                menu.Items = menu.Items.OrderBy(x=>x.Description).ToArray();
            }

            return View(menus.OrderBy(x=>x.Name).ToArray());

        }
    }
}
