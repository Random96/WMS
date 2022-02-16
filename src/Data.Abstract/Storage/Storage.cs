using ru.EmlSoft.WMS.Data.Abstract.Access;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ru.EmlSoft.WMS.Data.Abstract.Storage
{
    [Display(Description = "Склад")]
    public class Storage : Entity
    {
        public string Name { get; set; }
        public virtual ICollection<Cell> Cells { get; set; }
    }
}
