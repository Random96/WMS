using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.EmlSoft.WMS.Localization.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ru.emlsoft.WMS.Data.Abstract.Storage
{
    [Display(Name = "Хранение", Description = "Ячейка хранения", ResourceType=typeof(SharedResource))]
    public class Cell : Entity
    {
        public Cell() : base(EntityType.Cell) { }

        public int CodeId { get; set; }
        public virtual ScanCode Code { get; set; }
        public int TierId { get; set; }
        public virtual Tier? Tier { get; set; }
    }
}
