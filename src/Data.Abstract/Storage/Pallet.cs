using ru.EmlSoft.WMS.Data.Abstract.Access;
using ru.EmlSoft.WMS.Data.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ru.EmlSoft.WMS.Data.Abstract.Storage
{
    public enum PalletType {
        Euro,
        Custom
    }


    [Display(Name = "Хранение", Description = "Палеты")]
    public class Pallet : Entity
    {
        public PalletType PalletType { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Depth { get; set; }

        public int ? CodeId { get; set; }
        public virtual ScanCode ? Code {get; set; }
    }
}
