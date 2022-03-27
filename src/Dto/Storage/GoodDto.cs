using ru.emlsoft.WMS.Localization.Resources;
using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Dto.Storage
{
    public class GoodDto
    {
        public int Id { get; set; }
        [Display(Name = "GOOD_NAME", ResourceType = typeof(SharedResource))]
        [StringLength(200, MinimumLength = 3)]
        [Required]
        public string ? GoodName { get; set; }

        [Display(Name = "GOOD_ARTICLE", ResourceType = typeof(SharedResource))]
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string ? Article { get; set; }

        [Display(Name = "GOOD_CODE", ResourceType = typeof(SharedResource))]
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string ? Code { get; set; }

        [Display(Name = "GOOD_HEIGHT", ResourceType = typeof(SharedResource))]
        public int ? Heigh {get; set; }

        [Display(Name = "GOOD_WIDTH", ResourceType = typeof(SharedResource))]
        public int ? Width { get; set; }

        [Display(Name = "GOOD_DEPTH", ResourceType = typeof(SharedResource))]
        public int ? Depth { get; set; }

        [Display(Name = "GOOD_WEIGHT", ResourceType = typeof(SharedResource))]
        public int ? Weight { get; set; }
    }
}
