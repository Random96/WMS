using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ru.emlsoft.WMS.Data.Dto.Doc
{
    public enum DocType
    {
        none,

        [Display(Name = "DOCINPUT", ResourceType = typeof(SharedResource))]
        Input,
        Accept,
        Picking,
        Output,
        Sort,
        Inventory
    }

    public class DocDto
    {
        private int _id;

        public int Id 
        { 
            get => _id; 

            set
            {
                _id = value;
            }
        }
        
        [Display(Name = "DOCDATE", ResourceType = typeof(SharedResource))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Required]
        public DateTime DocDate { get; set; } = DateTime.Now;

        [Display(Name = "DOCNUMBER", ResourceType = typeof(SharedResource))]
        [StringLength(20)]
        [Required]
        public string? DocNumber { get; set; }


        [Display(Name = "DOCTYPE", ResourceType = typeof(SharedResource))]
        [Required]
        public DocType DocType { get; set; }

        public DateTime ? StartProcess { get; set; }

        public DateTime? EndProcess { get; set; }

        public bool Accepted { get; set; }

        public bool InWork => StartProcess != null && EndProcess == null;

        [Display(Name = "PARTNERNAME", ResourceType = typeof(SharedResource))]
        [StringLength(200, MinimumLength = 3)]
        [Required]
        public string? PartnerName { get; set; }


        [Display(Name = "STORAGENAME", ResourceType = typeof(SharedResource))]
        [StringLength(200, MinimumLength = 3)]
        [Required]
        public string? StorageName { get; set; }
    }
}
