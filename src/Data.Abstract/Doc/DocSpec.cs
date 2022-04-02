using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Storage;
using System;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Abstract.Doc
{
    public class DocSpec : IHaveId
    {
        public int Id { get; set; }
        public int DocId { get; set; }
        public int GoodId { get; set; }
        public int ? FromCellId { get; set; }
        public int? ToCellId { get; set; }
        public int Qty { get; set; }
        public int? PalletId { get; set; }
        public DateTime ? Manufactured { get; set; }
        public DateTime ? Expired { get; set; }
        public bool Approved { get; set; }

        public virtual Doc Doc { get; set; } = null!;
        public virtual Good Good { get; set; } = null!;

        public virtual Cell ? FromCell { get; set; }
        public virtual Cell? ToCell { get; set; }
        public virtual Pallet? Pallet { get; set; }
    }
}
