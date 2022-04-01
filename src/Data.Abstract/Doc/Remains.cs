using ru.emlsoft.WMS.Localization.Resources;
using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Data.Abstract.Storage;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ru.emlsoft.WMS.Data.Abstract.Doc
{
    [Display(Name = "Остатки", Description = "Отстатки товара", ResourceType = typeof(SharedResource))]
    public class Remains : Entity
    {
        public Remains() : base(EntityType.Remains) { }
        public DateTime Date { get; set; }
        public int GoodId { get; set; }
        public int CellId { get; set; }
        public int? PalletId { get; set; }
        public int Qty { get; set; }
        public bool Current { get; set; }
        // unique
        public int? PrevRemainsId { get; set; }
        public int StoreOrdId { get; set; }

        public virtual Good Good { get; set; }
        public virtual Cell? Cell { get; set; }
        public virtual Pallet? Pallet { get; set; }

        public virtual StoreOrd StoreOrd { get; set; }
    }
}
