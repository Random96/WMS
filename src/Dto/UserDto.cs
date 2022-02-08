using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ru.EmlSoft.WMS.Data.Dto
{
    public class UserDto
    {
        [Display(Name = "USERNAME")]
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? UserName { get; set; }

        [Display(Name = "PASSWORD")]
        [Required]
        public string? Passwd1 { get; set; }

        [Display(Name = "PASSWORD_CONFIRM")]
        [Required]
        [Compare(nameof(Passwd1))]
        public string? Passwd2 { get; set; }

        [Display(Name ="PHONE")]
        [Phone(ErrorMessage = "VALIDATE_PHONE")]
        public string? Phone { get; set; }

        [EmailAddress]
        [Display(Name = "EMAIL")]
        public string? Email { get; set; }
    }
}
