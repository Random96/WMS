using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace ru.emlsoft.WMS.Data.Abstract.Storage
{
    [Display(Name = "GOOD", Description = "PACK")]
    public class Pack : Entity
    {
        public Pack() : base(EntityType.Pack) { }

        public int Qty { get; set; }
        public int CodeId { get; set; }
        public virtual ScanCode? Code { get; set; }
    }
}
