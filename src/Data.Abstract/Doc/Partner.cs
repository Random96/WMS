using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Abstract.Doc
{
    [Display(Name = "Контрагенты", Description = "Контрагент", ResourceType = typeof(SharedResource))]
    public class Partner : Entity
    {
        public Partner() : base(EntityType.Partner) { }

        public string FullName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string OGRN { get; set; } = string.Empty;
        public string INN { get; set; } = string.Empty;
        public string KPP { get; set; } = string.Empty;

        public virtual ICollection<Doc> Docs { get; set; } = new List<Doc>();
    }
}
