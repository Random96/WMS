using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Dto
{
    public class CompanyDto
    {
        [Display(Name = "COMPANYNAME", ResourceType = typeof(SharedResource))]
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? Name { get; set; }
        
        [Display(Name = "COMPANYCANNEGATIVE", ResourceType = typeof(SharedResource))]
        [Required]
        public bool CanNegativeStocks { get; set; }
        
        [Display(Name = "COMPANYNEEDSAMPLEDATA", ResourceType = typeof(SharedResource))]
        [Required]
        public bool NeedSampleData { get; set; }
    }
}
