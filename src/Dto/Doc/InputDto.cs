using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Dto.Doc
{
    public class InputDto : DocDto
    {
        [Display(Name = "INPUTDATE", ResourceType = typeof(SharedResource))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime InputDate { get; set; } = DateTime.Now;

        [Display(Name = "INPUTCELL", ResourceType = typeof(SharedResource))]
        [Required]
        public string ? InputCell { get; set; }
    }
}
