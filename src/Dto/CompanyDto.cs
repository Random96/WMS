using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ru.EmlSoft.WMS.Data.Dto
{
    public class CompanyDto
    {
        [Display(Name = "COMPANYNAME")]
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string ? Name { get; set; }
    }
}
