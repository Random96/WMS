using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Doc;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using ru.emlsoft.WMS.Data.Dto.Storage;

namespace ru.emlsoft.WMS.Controllers
{
    public class RemainsController : ReadController<RemainsDto, Remains>
    {
        public RemainsController(IRepository<Remains> repo, IMapper mapper, IUserStore userStore, SignInManager<User> signInManager, ILogger<BaseController> logger) 
            : base(repo, mapper, userStore, signInManager, logger)
        {
        }

        protected override IEnumerable<RemainsDto> GetDtoEnumerable(IEnumerable<Remains> items)
        {
            return items.Select(x => DomainProfile.RemainToDto(_mapper, _repo.DataProvider, x)).ToArray();
        }
    }
}
