using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using ru.emlsoft.WMS.Data.Abstract.Storage;
using ru.emlsoft.WMS.Data.Dto.Storage;

namespace ru.emlsoft.WMS.Controllers
{
    public class StorageController : CrudController<StorageDto, Storage>
    {
        public StorageController(IRepository<Storage> repoStorage, IMapper mapper, IUserStore userStore, SignInManager<User> signInManager,
            ILogger<BaseController> logger)
            : base(repoStorage, mapper, userStore, signInManager, logger)
        {
        }


        protected override string CheckModel(StorageDto model)
        {
            if (model.StorageName == null)
            {
                return "ERROR_STORE_EMPTY_NAME";
            }

            return string.Empty;
        }

        protected override Storage PrepareItem(StorageDto model)
        {
            if (model == null)
                throw new Exception();

            var storage = new Storage()
            {
                Name = model.StorageName ?? string.Empty
            };

            for (int i = 1; i <= model.Rows; i++)
            {
                var row = new Row() { Storage = storage, Code = i.ToString() };
                storage.Rows.Add(row);

                for (int j = 1; j <= model.Tiers; j++)
                {
                    var tier = new Tier()
                    {
                        Code = (j+1).ToString(),
                        Row = row
                    };
                    row.Tiers.Add(tier);

                    for (int z = 1; z<= model.Cells; ++z)
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

            return storage;
        }
    }
}
