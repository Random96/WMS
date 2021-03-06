using ru.emlsoft.WMS.Data.Abstract.Access;
using System;
using System.ComponentModel.DataAnnotations;

namespace ru.emlsoft.WMS.Data.Abstract.Storage
{
    [Display(Name = "GOOD", Description = "GOOD")]
    public class Good : Entity
    {
        public Good() : base(EntityType.Good) { }
        public string Name { get; set; } = string.Empty;
        public int? CodeId { get; set; }
        public virtual ScanCode? Code { get; set; }
        public string? Article { get; set; }
    }
}
