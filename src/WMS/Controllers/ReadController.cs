using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using ru.emlsoft.WMS.Data.Dto;

namespace ru.emlsoft.WMS.Controllers
{
    public abstract class ReadController<Dto, T> : BaseController
        where T : Entity
        where Dto : class
    {
        protected readonly IRepository<T> _repo;
        protected readonly IMapper _mapper;

        public ReadController(IRepository<T> repo, IMapper mapper, IUserStore userStore, SignInManager<User> signInManager, ILogger<BaseController> logger) : base(userStore, signInManager, logger)
        {
            _repo = repo;
            _mapper = mapper;
        }

        protected virtual IEnumerable<Dto> GetDtoEnumerable(IEnumerable<T> items)
        {
            return items.Select(x => _mapper.Map<T, Dto>(x)).ToArray();
        }

        // GET: CrusController
        public async Task<ActionResult> Index(int pageNum, int pageSize, IEnumerable<FilterObject> filters, CancellationToken cancellationToken)
        {
            if (pageSize == 0)
                pageSize = 3;

            _repo.UserId = await GetUserIdAsync(cancellationToken);

            var items = await _repo.GetPageAsync(pageNum, pageSize, filters, null, cancellationToken, true);

            var dtoObjects = GetDtoEnumerable(items);

            var page = new PageDto<Dto>()
            {
                PageSize = pageSize,
                PageNumber = pageNum,
                Items = dtoObjects,
                TotalRows  = 10
            };
            return View(page);
        }
    }
}
