using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.EmlSoft.WMS.Localization.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ru.emlsoft.WMS.Data.Abstract.Storage
{
    [Display(Name = "Товары", Description = "Упакрвки", ResourceType = typeof(SharedResource))]
    public class Pack : Entity
    {
        public Pack() : base(EntityType.Pack) { }

        public int Qty { get; set; }
        public int CodeId { get; set; }
        public virtual ScanCode ? Code { get; set; }
    }
}
