using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Doc;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using ru.emlsoft.WMS.Data.Dto.Storage;

namespace ru.emlsoft.WMS.Controllers
{
    public class StoreOrdController : ReadController<StoreOrdDto, StoreOrd>
    {
        public StoreOrdController(IRepository<StoreOrd> repo, IMapper mapper, IUserStore userStore, SignInManager<User> signInManager, ILogger<BaseController> logger)
            : base(repo, mapper, userStore, signInManager, logger)
        {
        }

        protected override IEnumerable<StoreOrdDto> GetDtoEnum(IEnumerable<StoreOrd> items)
        {
            return items.Select(x => DomainProfile.StoreOrdToDto(_mapper, _repo.DataProvider, x )).ToArray();
        }
    }
}
