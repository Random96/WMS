using ru.emlsoft.WMS.Data.Abstract.Access;
using System;
using System.ComponentModel.DataAnnotations;

namespace ru.emlsoft.WMS.Data.Abstract.Storage
{
    public enum PalletType
    {
        Euro,
        Custom
    }


    [Display(Name = "GOOD", Description = "PALLET")]
    public class Pallet : Entity
    {
        public Pallet() : base(EntityType.Pallet) { }

        public PalletType PalletType { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Depth { get; set; }

        public int? CodeId { get; set; }
        public virtual ScanCode? Code { get; set; }
    }
}
