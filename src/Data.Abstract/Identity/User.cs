using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Personnel;
using System;

namespace ru.emlsoft.WMS.Data.Abstract.Identity
{
    public class User : IHaveId
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public int? PersonId { get; set; }
        public string? LoginName { get; set; }
        public string? PasswordHash { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public bool IsLocked { get; set; }
        public DateTime? Expired { get; set; }
        public DateTime? LockedTo { get; set; }
        public virtual ICollection<Logins>? Logins { get; set; }
        public virtual Company? Company { get; set; }
        public virtual Person? Person { get; set; }
    }
}
