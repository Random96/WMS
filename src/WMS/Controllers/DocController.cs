using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Doc;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using ru.emlsoft.WMS.Data.Abstract.Storage;
using ru.emlsoft.WMS.Data.Dto;
using ru.emlsoft.WMS.Data.Dto.Doc;
using ru.emlsoft.WMS.Localization;

namespace ru.emlsoft.WMS.Controllers
{
    public class DocController : BaseController
    {
        readonly IRepository<Doc> _repoDoc;
        readonly IRepository<DocSpec> _repoDocSpec;
        readonly DocStorage _docStorage;
        readonly IRepository<Storage> _repoStorage;
        readonly IRepository<Cell> _repoCell;
        readonly IRepository<Partner> _repoPartner;
        readonly IRepository<ScanCode> _repoScan;
        readonly IRepository<Good> _repoGood;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public DocController(DocStorage docStorage, IRepository<Doc> repoDoc, IRepository<DocSpec> repoDocSpec, IRepository<Partner> repoPartner,
            IRepository<Good> repoGood, IRepository<ScanCode> repoScan, IRepository<Storage> repoStorage, IRepository<Cell> repoCell, IUserStore userStore, SignInManager<User> signInManager, IMapper mapper,
            ILogger<DocController> logger, IStringLocalizer<SharedResource> localizer)
            : base(userStore, signInManager, logger)
        {
            _docStorage = docStorage;
            _repoDoc = repoDoc;
            _repoDocSpec = repoDocSpec;
            _repoPartner = repoPartner;
            _mapper = mapper;
            _localizer = localizer;
            _repoStorage = repoStorage;
            _repoCell = repoCell;
            _repoScan = repoScan;
            _repoGood = repoGood;
        }


        // GET: DocController
        public async Task<ActionResult> Index(int pageNum, int pageSize, IEnumerable<FilterObject> filters, CancellationToken cancellationToken)
        {
            if (pageSize == 0)
                pageSize = 3;

            _repoDoc.UserId = await GetUserIdAsync(cancellationToken);

            var items = await _repoDoc.GetPageAsync(pageNum, pageSize, filters, null, cancellationToken, true);

            var docs = items.Select(x => _mapper.Map<Doc, DocDto>(x)).ToArray();

            var page = new PageDto<DocDto>()
            {
                PageSize = pageSize,
                PageNumber = pageNum,
                Items = docs,
                TotalRows  = 10
            };

            return View(page);
        }

        // GET: DocController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DocController/CreateInput
        public ActionResult CreateInput()
        {
            return View(new InputDto());
        }

        // POST: DocController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateInput(InputDto item, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(item.PartnerName))
            {
                ModelState.AddModelError(string.Empty, "ERROR_PARTNER_IS_EMPTY");
                return View(item);
            }

            if(string.IsNullOrWhiteSpace(item.StorageName))
            {
                ModelState.AddModelError(string.Empty, "ERROR_STORAGE_NOT_SET");
                return View(item);
            }
            if (string.IsNullOrWhiteSpace(item.InputCell))
            {
                ModelState.AddModelError(string.Empty, "ERROR_CELL_NOT_SET");
                return View(item);
            }


            var userId = await GetUserIdAsync(cancellationToken);

            _repoDoc.UserId = userId;
            _repoPartner.UserId = userId;
            _repoStorage.UserId = userId;
            _repoCell.UserId = userId;
            _repoScan.UserId = userId;

            try
            {
                var doc = _mapper.Map<InputDto, Doc>(item);

                var partners = await _repoPartner.GetListAsync(new FilterObject[] { new FilterObject(nameof(Partner.Name), FilterOption.Equals, item.PartnerName) }, null, cancellationToken);
                if (partners.Any())
                {
                    if (partners.Count() == 1)
                    {
                        doc.PartnerId = partners.First().Id;
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "ERROR_TOO_MANY_PARTNERS");
                        return View(item);
                    }
                }
                else
                {
                    _logger.LogTrace("Partner not found");
                    ModelState.AddModelError(string.Empty, _localizer["PARTNER_NOT_FOUND"].Value);
                    return View();
                }

                var sorages = await _repoStorage.GetListAsync(new FilterObject[] { new FilterObject(nameof(Storage.Name), FilterOption.Equals, item.StorageName) }, null, cancellationToken);
                if (sorages.Any())
                {
                    if (sorages.Count() == 1)
                    {
                        doc.StorageId = sorages.First().Id;
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "TOO_MANY_STORAGES");
                        return View(item);
                    }
                }
                else
                {
                    _logger.LogTrace("Store not found");
                    ModelState.AddModelError(string.Empty, _localizer["STORE_NOT_FOUND"].Value);
                    return View();
                }

                var scans = await _repoScan.GetListAsync(new FilterObject[] { new FilterObject(nameof(ScanCode.Code), FilterOption.Equals, item.InputCell) }, null, cancellationToken);
                if (scans.Any())
                {
                    if (scans.Count() == 1)
                    {
                        var cells = await _repoCell.GetListAsync(new FilterObject[]
                            {
                                new FilterObject(nameof(Cell.CodeId), FilterOption.Equals, scans.First().Id)
                            }, null, cancellationToken);
                        if (cells.Any())
                        {
                            if (cells.Count() == 1)
                            {
                                doc.Input.InputCellId = cells.First().Id;
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "ERROR_TOO_MANY_STORAGES");
                                return View(item);
                            }
                        }
                        else
                        {
                            _logger.LogTrace("Store not found");
                            ModelState.AddModelError(string.Empty, _localizer["ERROR_STORE_NOT_FOUND"].Value);
                            return View();
                        }

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "ERROR_TOO_MANY_CELLS");
                        return View(item);
                    }
                }
                else
                {
                    _logger.LogTrace("Scan of cell not found");
                    ModelState.AddModelError(string.Empty, _localizer["ERROR_CELL_NOT_FOUND"].Value);
                    return View();
                }

                doc = await _repoDoc.AddAsync(doc, cancellationToken);

                return RedirectToAction(nameof(CreateGood), new { DocId = doc.Id });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CreateGood(int DocId)
        {
            return View(new DocSpecDto { DocId = DocId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateGood(int docId, DocSpecDto model, CancellationToken cancellationToken)
        {
            _repoDocSpec.UserId = _repoGood.UserId = _repoDoc.UserId = await GetUserIdAsync(cancellationToken);
            try
            {
                var docSpec = new DocSpec() { DocId = docId, Qty = model.Qty };
                var goods = await _repoGood.GetListAsync(new FilterObject[] { new FilterObject(nameof(Good.Name), FilterOption.Equals, model.GoodName) }, null, cancellationToken);
                if (goods.Any())
                {
                    if (goods.Count() == 1)
                    {
                        docSpec.GoodId = goods.First().Id;
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "TOO_MANY_GOODS");
                        return View(model);
                    }
                }
                else
                {
                    _logger.LogTrace("Store not found");
                    ModelState.AddModelError(string.Empty, _localizer["TOO_MANY_GOODS"].Value);
                    return View();
                }

                var doc = await _repoDoc.GetByIdAsync(docId);

                _repoDocSpec.Add(docSpec);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on create doc spec");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            return View(new DocSpecDto { DocId = docId });
        }


        // GET: DocController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DocController/Edit/5
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

        // GET: DocController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DocController/Delete/5
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



        // GET: DocController/Delete/5
        public async Task<ActionResult> Apply(int id, CancellationToken cancellationToken)
        {
            _repoDoc.UserId = await GetUserIdAsync(cancellationToken);

            var item = await _repoDoc.GetByIdAsync(id, cancellationToken);

            return View(item);
        }

        // POST: DocController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Apply(int id, IFormCollection collection, CancellationToken cancellationToken)
        {
            try
            {
                _docStorage.UserId = await GetUserIdAsync(cancellationToken);

                await _docStorage.ApplyDoc(id, cancellationToken);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on Apply doc");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

    }
}
