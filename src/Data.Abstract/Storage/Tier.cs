using ru.emlsoft.WMS.Data.Abstract.Access;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Abstract.Storage
{
    [Display(Name = "STORAGE", Description = "TIER")]
    public class Tier : Entity
    {
        public Tier() : base(EntityType.Tier)
        {
            Code = string.Empty;
            Cells = new List<Cell>();
        }

        public string Code { get; set; }
        public int RowId { get; set; }
        public virtual Row? Row { get; set; }
        public virtual ICollection<Cell> Cells { get; set; }
    }
}
