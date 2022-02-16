using ru.EmlSoft.WMS.Data.Abstract.Access;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ru.EmlSoft.WMS.Data.Abstract.Storage
{
    [Display(Description = "Ячейка хранения")]
    public class Cell : Entity
    {
        public int CodeId { get; set; }
        public virtual ScanCode Code { get; set; }
        public int StorageId { get; set; }
        public virtual Storage Storage { get; set; }
    }
}
