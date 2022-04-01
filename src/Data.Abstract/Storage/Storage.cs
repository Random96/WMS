using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace ru.emlsoft.WMS.Data.Abstract.Storage
{
    [Display(Name = "STORAGE", Description = "WAREHOUSE")]
    public class Storage : Entity
    {
        public Storage() : base(EntityType.Storage)
        {
            Rows = new List<Row>();
        }

        public string Name { get; set; } = String.Empty;

        public virtual ICollection<Row> Rows { get; set; } = new List<Row>();
    }
}
