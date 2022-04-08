using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using System;

namespace ru.emlsoft.WMS.Data.Abstract.Access
{
    /// <summary>
    /// Должность (соответствует обычнчому понятию группы)
    /// Содержит информацию а правах, необходимых для выполнения обязанностей (Rights)
    /// И ссылки на наначения на должность (Appointments)
    /// </summary>
    public class Position : IHaveId, ICompany
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; } = String.Empty;
        public bool IsAdmin { get; set; }
        public virtual ICollection<AccessRight> Rights { get; set; } = new List<AccessRight>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual Company Company { get; set; } = null!;
    }
}
