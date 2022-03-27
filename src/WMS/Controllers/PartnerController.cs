using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Doc;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using ru.emlsoft.WMS.Data.Dto.Doc;

namespace ru.emlsoft.WMS.Controllers
{
    public class PartnerController : CrudController<PartnerDto, Partner>
    {
        public PartnerController(IRepository<Partner> repoStorage, IMapper mapper, IUserStore userStore, SignInManager<User> signInManager,
            ILogger<BaseController> logger)
            : base(repoStorage, mapper, userStore, signInManager, logger)
        {
        }

        protected override string CheckModel(PartnerDto model)
        {
            return string.Empty;
        }
    }
}
