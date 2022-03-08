using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.EmlSoft.WMS.Localization.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ru.emlsoft.WMS.Data.Abstract.Storage
{
    [Display(Name = "Хранение", Description = "Ярус", ResourceType = typeof(SharedResource))]
    public class Tier : Entity
    {
        public Tier() : base(EntityType.Tier)
        {
            Code = string.Empty;
            Cells = new List<Cell>();
        }

        public string Code { get; set; }
        public int RowId { get; set; }
        public virtual Row ? Row { get; set; }
        public virtual ICollection<Cell> Cells { get; set; }
    }
}
