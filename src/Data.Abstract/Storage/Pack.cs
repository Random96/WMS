using ru.EmlSoft.WMS.Data.Abstract.Access;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ru.EmlSoft.WMS.Data.Abstract.Storage
{
    [Display(Name = "Товары", Description = "Упакрвки")]
    public class Pack : Entity
    {
        public int Qty { get; set; }
        public int CodeId { get; set; }
        public virtual ScanCode ? Code { get; set; }
    }
}
