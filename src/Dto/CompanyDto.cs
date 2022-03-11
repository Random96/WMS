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
    }
}
