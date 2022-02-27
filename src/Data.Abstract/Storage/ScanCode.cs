using ru.EmlSoft.WMS.Data.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ru.EmlSoft.WMS.Data.Abstract.Storage
{
    [Display(Name = "Сканирование", Description = "Коды сканирования")]
    public class ScanCode : IHaveId
    {
        public int Id { get; set; }
        public string ? Code { get; set; }
        public virtual ICollection<Good> ? Goods { get; set; }
        public virtual ICollection<Pack> ? Packs { get; set; }
        public virtual ICollection<Pallet> ? Pallets { get; set; }
        public virtual ICollection<Cell> ? Cells { get; set; }
    }
}
