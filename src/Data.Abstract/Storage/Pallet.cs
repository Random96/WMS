using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Data.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ru.emlsoft.WMS.Localization.Resources;

namespace ru.emlsoft.WMS.Data.Abstract.Storage
{
    public enum PalletType {
        Euro,
        Custom
    }


    [Display(Name = "Хранение", Description = "Палеты", ResourceType=typeof(SharedResource))]
    public class Pallet : Entity
    {
        public Pallet() : base(EntityType.Pallet) { }

        public PalletType PalletType { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Depth { get; set; }

        public int ? CodeId { get; set; }
        public virtual ScanCode ? Code {get; set; }
    }
}
