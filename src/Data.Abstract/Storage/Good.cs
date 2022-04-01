using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace ru.emlsoft.WMS.Data.Abstract.Storage
{
    [Display(Name = "GOOD", Description = "GOOD")]
    public class Good : Entity
    {
        public Good() : base(EntityType.Good) { }
        public string Name { get; set; }
        public int ? CodeId { get; set; }
        public virtual ScanCode ? Code { get; set; }
        public string? Article { get; set; }
    }
}
