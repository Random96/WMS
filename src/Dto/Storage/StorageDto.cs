using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Dto.Storage
{
    public class StorageDto
    {
        public int Id { get; set; }

        [Display(Name = "STORAGENAME", ResourceType = typeof(SharedResource))]
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string ? StorageName { get; set; }

        [Display(Name = "STORAGEROWS", ResourceType = typeof(SharedResource))]
        [Range(1, int.MaxValue)]
        [Required]
        public int Rows { get; set; }

        [Display(Name = "STORAGETIERS", ResourceType = typeof(SharedResource))]
        [Range(1, int.MaxValue)]
        [Required]

        public int Tiers { get; set; }
        [Display(Name = "STORAGECELLS", ResourceType = typeof(SharedResource))]
        [Range(1, int.MaxValue)]
        [Required]
        public int Cells { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
