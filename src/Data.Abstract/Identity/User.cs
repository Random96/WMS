using System;
using System.Collections.Generic;
using System.Text;

namespace ru.EmlSoft.WMS.Data.Abstract.Identity
{
    public class User
    {
        public int Id { get; set; }
        public int ? CompanyId { get; set; }
        public string LoginName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
