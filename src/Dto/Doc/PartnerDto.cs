using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ru.emlsoft.WMS.Data.Dto.Doc
{
    public class PartnerDto
    {
        public int Id { get; set; }

        [Display(Name = "PARTNER_FULL_NAME", ResourceType = typeof(SharedResource))]
        [StringLength(200, MinimumLength = 3)]
        [Required] 
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "PARTNER_NAME", ResourceType = typeof(SharedResource))]
        [StringLength(200, MinimumLength = 3)]
        [Required]
        public string PartnerName { get; set; } = string.Empty;

        [Display(Name = "PARTNER_OGRN", ResourceType = typeof(SharedResource))]
        [StringLength(55, MinimumLength = 0)]
        // [Required(AllowEmptyStrings =true)]
        public string ? OGRN { get; set; } = string.Empty;

        [Display(Name = "PARTNER_INN", ResourceType = typeof(SharedResource))]
        [StringLength(13, MinimumLength = 0)]
        //[Required(AllowEmptyStrings = true)]
        public string ? INN { get; set; } = string.Empty;

        [Display(Name = "PARTNER_KPP", ResourceType = typeof(SharedResource))]
        [StringLength(13, MinimumLength = 0)]
        // [Required(AllowEmptyStrings = true)]
        public string ? KPP { get; set; } = string.Empty;
    }
}
