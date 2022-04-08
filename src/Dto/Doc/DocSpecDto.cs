using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Dto.Doc
{
    public class DocSpecDto
    {
        public int Id { get; set; }
        public int DocId { get; set; }
        public int GoodId { get; set; }

        [Display(Name = "GOODNAME", ResourceType = typeof(SharedResource))]
        [StringLength(200, MinimumLength = 3)]
        [Required]
        public string GoodName { get; set; } = string.Empty;


        [Display(Name = "CELLFROM", ResourceType = typeof(SharedResource))]
        public string FromCell { get; set; } = String.Empty;

        [Display(Name = "CELLTO", ResourceType = typeof(SharedResource))]
        public string ToCell { get; set; } = String.Empty;

        [Display(Name = "QUANTITY", ResourceType = typeof(SharedResource))]
        public int Qty { get; set; }

    }
}
