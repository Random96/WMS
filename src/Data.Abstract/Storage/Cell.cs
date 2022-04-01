using ru.emlsoft.WMS.Data.Abstract.Access;
using System;
using System.ComponentModel.DataAnnotations;

namespace ru.emlsoft.WMS.Data.Abstract.Storage
{
    [Display(Name = "STORAGE", Description = "CELL")]
    public class Cell : Entity
    {
        public Cell() : base(EntityType.Cell) { }

        public int CodeId { get; set; }
        public virtual ScanCode? Code { get; set; }
        public int TierId { get; set; }
        public virtual Tier? Tier { get; set; }
    }
}
