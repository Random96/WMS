using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using ru.emlsoft.WMS.Data.Abstract.Storage;
using ru.emlsoft.WMS.Data.Dto.Storage;


namespace ru.emlsoft.WMS.Controllers
{
    public class GoodController : CrudController<GoodDto, Good>
    {
        private readonly IRepository<ScanCode> _repoCode;
        public GoodController(IRepository<ScanCode> repoCode, IRepository<Good> repo, IMapper mapper, IUserStore userStore, SignInManager<User> signInManager, ILogger<BaseController> logger)
            : base(repo, mapper, userStore, signInManager, logger)
        {
            _repoCode = repoCode;
        }

        protected override Good PrepareItem(GoodDto model)
        {
            var ret = base.PrepareItem(model);

            _repoCode.UserId = GetUserId();

            if (!string.IsNullOrWhiteSpace(model.Code))
            {
                var codes = _repoCode.GetList(new FilterObject[] { new FilterObject(nameof(ScanCode.Code), FilterOption.Equals, model.Code) }, null, false);

                if (codes.Any())
                {
                    if (codes.Count() == 1)
                    {
                        ret.CodeId = codes.First().Id;
                    }
                    else
                    {
                        throw new Exception("ERROR_TOO_MANY_CODES");
                    }
                }
                else
                {
                    var code = _repoCode.Add(new ScanCode() { Code = model.Code });
                    ret.CodeId = code.Id;
                }
            }

            return ret;
        }


        protected override string CheckModel(GoodDto model)
        {
            return String.Empty;
        }
    }
}
