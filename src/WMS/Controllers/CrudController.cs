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
    public abstract class CrudController<Dto, T> : BaseController
        where T : Entity
        where Dto : class
    {
        private readonly IMapper _mapper;

        protected readonly IRepository<T> _repo;


        public CrudController(IRepository<T> repo, IMapper mapper, IUserStore userStore, SignInManager<User> signInManager, ILogger<BaseController> logger) : base(userStore, signInManager, logger)
        {
            _repo = repo;
            _mapper = mapper;
        }

        protected abstract string CheckModel(Dto model);

        protected virtual T PrepareItem(Dto model)
        {
            return _mapper.Map<T>(model);
        }

        // GET: CrusController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CrusController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Dto model, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _repo.UserId = await GetUserIdAsync(cancellationToken);

                var checkResult = CheckModel(model);
                if (!string.IsNullOrWhiteSpace(checkResult))
                {
                    ModelState.AddModelError(string.Empty, checkResult);
                    return View(model);
                }

                var item = PrepareItem(model);

                await _repo.AddAsync(item, cancellationToken);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error on create");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        // GET: CrusController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CrusController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CrusController/Details/5
        public async Task<ActionResult> Details(int Id, CancellationToken cancellationToken = default)
        {
            _repo.UserId = await GetUserIdAsync(cancellationToken);

            var item = await _repo.GetByIdAsync(Id, cancellationToken);

            var ret = _mapper.Map<T, Dto>(item);

            return View(ret);
        }

        // GET: CrusController/Edit/5
        public async Task<ActionResult> Edit(int id, CancellationToken cancellationToken = default)
        {
            _repo.UserId = await GetUserIdAsync(cancellationToken);

            var item = await _repo.GetByIdAsync(id, cancellationToken);

            var ret = _mapper.Map<T, Dto>(item);

            return View(ret);
        }

        // POST: CrusController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Dto model, CancellationToken cancellationToken = default)
        {
            _repo.UserId = await GetUserIdAsync(cancellationToken);

            var checkResult = CheckModel(model);
            if (!string.IsNullOrWhiteSpace(checkResult))
            {
                ModelState.AddModelError(string.Empty, checkResult);
                return View(model);
            }

            try
            {
                var item = _mapper.Map<Dto, T>(model);

                await _repo.UpdateAsync(item, cancellationToken);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        // GET: CrusController
        public async Task<ActionResult> Index(int pageNum, int pageSize, IEnumerable<FilterObject> filters, CancellationToken cancellationToken)
        {
            if (pageSize == 0)
                pageSize = 3;

            _repo.UserId = await GetUserIdAsync(cancellationToken);

            var items = await _repo.GetPageAsync(pageNum, pageSize, filters, null, cancellationToken, true);

            var storages = items.Select(x => _mapper.Map<T, Dto>(x)).ToArray();

            var page = new PageDto<Dto>()
            {
                PageSize = pageSize,
                PageNumber = pageNum,
                Items = storages,
                TotalRows  = 10
            };
            return View(page);
        }
    }
}
