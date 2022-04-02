using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Dto.Storage
{
    public class StoreOrdDto
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        [Display(Name = "GOOD_NAME", ResourceType = typeof(SharedResource))]
        [StringLength(200, MinimumLength = 3)]
        [Required]
        public string Good { get; set; } = String.Empty;

        [Display(Name = "CELL_CODE", ResourceType = typeof(SharedResource))]
        [StringLength(200, MinimumLength = 3)]
        [Required]
        public string Cell { get; set; } = String.Empty;

        [Display(Name = "PALLET_CODE", ResourceType = typeof(SharedResource))]
        [StringLength(200, MinimumLength = 3)]
        [Required]
        public string Pallet { get; set; } = null!;

        [Display(Name = "QUANTITY", ResourceType = typeof(SharedResource))]
        public int Qty { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.MinValue;
    }
}
