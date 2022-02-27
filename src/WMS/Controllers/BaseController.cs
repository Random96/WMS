﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ru.EmlSoft.WMS.Data.Abstract.Database;
using ru.EmlSoft.WMS.Data.Abstract.Identity;
using ru.EmlSoft.WMS.Data.Dto;
using WMS.Tools;

namespace ru.EmlSoft.WMS.Controllers
{
    public abstract class BaseController : Controller
    {

        protected readonly ILogger<BaseController> _logger;
        protected readonly SignInManager<User> _signInManager;
        protected readonly IUserStore _userStore;

        public BaseController(IUserStore userStore, SignInManager<User> signInManager, ILogger<BaseController> logger)
        {
            _logger = logger;
            _userStore = userStore;
            _signInManager = signInManager;
        }

        internal async Task<IActionResult ?> CheckUserAsync(CancellationToken cancellationToken)
        {
            // get current user 
            var user = await _userStore.GetUserAsync(_signInManager, cancellationToken);

            if (user == null || user.Id == 0)
                return RedirectToAction("Login", "Account");

            if (user.CompanyId == null)
                return RedirectToAction("CreateCompany", "Account");

            return null;
        }

    }
}
