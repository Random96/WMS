using ru.emlsoft.WMS.Localization.Resources;
using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Data.Abstract.Storage;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Abstract.Doc
{
    [Display(Name = "Остатки", Description = "Движение товара", ResourceType = typeof(SharedResource))]
    public class StoreOrd : Entity
    {
        public StoreOrd() : base(EntityType.StoreOrd)
        {
        }
        public DateTime DateTime { get; set; }
        public int GoodId { get; set; }
        public int CellId { get; set; }
        public int? PalletId { get; set; }
        public int Qty { get; set; }

        // ссылка на документы
        public int DocSpecId { get; set; }

        public virtual Good Good { get; set; }
        public virtual Cell? Cell { get; set; }
        public virtual Pallet? Pallet { get; set; }

        public virtual DocSpec DocSpec { get; set; }
    }
}
