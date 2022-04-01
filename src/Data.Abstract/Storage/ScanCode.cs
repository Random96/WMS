using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace ru.emlsoft.WMS.Data.Abstract.Storage
{
    [Display(Name = "Сканирование", Description = "Коды сканирования", ResourceType = typeof(SharedResource))]
    public class ScanCode : IHaveId, ICompany
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string? Code { get; set; }
        public virtual ICollection<Good>? Goods { get; set; }
        public virtual ICollection<Pack>? Packs { get; set; }
        public virtual Pallet Pallet { get; set; }
        public virtual Cell Cell { get; set; }

        public virtual Company Company { get; set; }
    }
}
