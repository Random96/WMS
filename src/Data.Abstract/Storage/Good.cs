using ru.emlsoft.WMS.Data.Abstract.Access;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;
using ru.EmlSoft.WMS.Localization.Resources;

namespace ru.emlsoft.WMS.Data.Abstract.Storage
{
    [Display(Name = "Товары", Description = "Товары", ResourceType = typeof(SharedResource))]
    public class Good : Entity
    {
        public Good() : base(EntityType.Good) { }

        public int CodeId { get; set; }
        public virtual ScanCode ? Code { get; set; }
        public string ? Article { get; set; }
    }
}
