using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ru.emlsoft.WMS.Data.Dto
{
    public class UserDto
    {
        [Display(Name = "USERNAME", ResourceType = typeof(SharedResource))]
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? UserName { get; set; }

        [Display(Name = "PASSWORD", ResourceType = typeof(SharedResource))]
        [Required]
        public string? Password1 { get; set; }

        [Display(Name = "PASSWORD_CONFIRM", ResourceType = typeof(SharedResource))]
        [Required]
        [Compare(nameof(Password1))]
        public string? Password2 { get; set; }

        [Display(Name ="PHONE", ResourceType = typeof(SharedResource))]
        [Phone(ErrorMessage = "VALIDATE_PHONE")]
        public string? Phone { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "EMAIL", ResourceType = typeof(SharedResource))]
        public string? Email { get; set; }

        [Display(Name = "COMPANY", ResourceType = typeof(SharedResource))]
        [StringLength(200, MinimumLength = 3)]
        public string? Company { get; set; }
    }
}
