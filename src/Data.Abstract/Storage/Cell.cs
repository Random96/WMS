using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace ru.emlsoft.WMS.Data.Abstract.Storage
{
    [Display(Name = "Хранение", Description = "Ячейка хранения", ResourceType = typeof(SharedResource))]
    public class Cell : Entity
    {
        public Cell() : base(EntityType.Cell) { }

        public int CodeId { get; set; }
        public virtual ScanCode ? Code { get; set; }
        public int TierId { get; set; }
        public virtual Tier? Tier { get; set; }
    }
}
