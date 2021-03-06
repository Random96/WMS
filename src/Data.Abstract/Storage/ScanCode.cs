using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace ru.emlsoft.WMS.Data.Abstract.Storage
{
    [Display(Name = "SCAN", Description = "SCANCODE")]
    public class ScanCode : IHaveId, ICompany
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string? Code { get; set; }
        public virtual ICollection<Good>? Goods { get; set; }
        public virtual ICollection<Pack>? Packs { get; set; }
        public virtual Pallet Pallet { get; set; } = null!;
        public virtual Cell Cell { get; set; } = null!;

        public virtual Company Company { get; set; } = null!;
    }
}
