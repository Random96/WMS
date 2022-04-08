using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Data.Abstract.Storage;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Abstract.Doc
{
    [Display(Name = "REMAINS", Description = "STOREREMAINS")]
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

        public virtual Good Good { get; set; } = null!;
        public virtual Cell? Cell { get; set; }
        public virtual Pallet? Pallet { get; set; }

        public virtual StoreOrd StoreOrd { get; set; } = null!;
    }
}
