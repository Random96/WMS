using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using ru.emlsoft.WMS.Data.Abstract.Storage;
using ru.EmlSoft.WMS.Data.Dto;
using ru.EmlSoft.WMS.Data.Dto.Storage;

namespace ru.emlsoft.WMS.Controllers
{
    public class StorageController : BaseController
    {
        readonly IRepository<Storage> _repoStorage;
        // readonly IRepository<Row> _repoRow;
        //readonly IRepository<Tier> _repoTier;
        //readonly IRepository<ScanCode> _repoCode;
        //readonly IRepository<Cell> _repoCell;


        public StorageController(IRepository<Storage> repoStorage, IRepository<Row> repoRow, IRepository<Tier> repoTier,
            IRepository<ScanCode> repoCode, IRepository<Cell> repoCell, IUserStore userStore, SignInManager<User> signInManager, 
            ILogger<BaseController> logger)
            : base(userStore, signInManager, logger)
        {
            _repoStorage = repoStorage;
            // _repoRow = repoRow;
            // _repoTier = repoTier;
            // _repoCode = repoCode;
            // _repoCell = repoCell;
        }

        // GET: StorageController
        public async Task<ActionResult> Index(int pageNum, int pageSize, IEnumerable<FilterObject> filters, CancellationToken cancellationToken)
        {
            if (pageSize == 0)
                pageSize = 3;

            var companyId = await GetUserIdAsync(cancellationToken);

            _repoStorage.UserId = companyId;
            // _repoRow.CompanyId = companyId;
            // _repoTier.CompanyId = companyId;

            var items = await _repoStorage.GetPageAsync(pageNum, pageSize, filters, null, cancellationToken, true);

            var storages = items.Select(x => new StorageDto() { Id = x.Id, StorageName = x.Name, Rows = x.Rows.Count, Tiers =  x.Rows.Any(y=>y.Tiers.Any())  ? x.Rows.Select(y => y.Tiers.Count).Max() : 0 }).ToArray();

            var page = new PageDto<StorageDto>()
            {
                PageSize = pageSize,
                PageNumber = pageNum,
                Items = storages,
                TotalRows  = 10
            };

            return View(page);
        }

        // GET: StorageController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StorageController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StorageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(StorageDto model, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // _repoCell.CompanyId = _repoCode.CompanyId = _repoTier.CompanyId = _repoRow.CompanyId = 
                _repoStorage.UserId = await GetUserIdAsync(cancellationToken);

                if( model.StorageName == null)
                {
                    ModelState.AddModelError(string.Empty, "ERROR_STORE_EMPTY_NAME");
                    return View(model);
                }

                var storage = new Storage()
                {
                    Name = model.StorageName
                };

                for( int i=1; i <= model.Rows; i++ )
                {
                    var row = new Row() { Storage = storage, Code = i.ToString() };
                    storage.Rows.Add(row);

                    for (int j =1; j <= model.Tiers; j++)
                    {
                        var tier = new Tier()
                        {
                            Code = (j+1).ToString(),
                            Row = row
                        };
                        row.Tiers.Add(tier);

                        for( int z=1; z<= model.Cells; ++z)
                        {
                            var code = new ScanCode()
                            {
                                Code = $"{i:000}{j:000}{z:000}"                                
                            };

                            var cell = new Cell()
                            {
                                Code = code,
                                TierId = tier.Id
                            };

                            tier.Cells.Add(cell);
                        }
                    }
                }

                storage = await _repoStorage.UpdateAsync(storage, cancellationToken);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StorageController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StorageController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: StorageController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StorageController/Delete/5
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
    }
}
