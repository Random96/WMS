using ru.EmlSoft.WMS.Data.Abstract.Access;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ru.EmlSoft.WMS.Data.Abstract.Storage
{
    [Display(Name = "Товары", Description = "Товары")]
    public class Good : Entity
    {
        public int CodeId { get; set; }
        public virtual ScanCode ? Code { get; set; }
        public string ? Article { get; set; }
    }
}
