using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using System;
using System.Collections.Generic;
using System.Text;

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
        public string ? Name { get; set; }
        public bool IsAdmin { get; set; }
        public virtual ICollection<AccessRight> ? Rights { get; set; }
        public virtual ICollection<Appointment> ? Appointments { get; set; }
        public virtual Company ? Company { get; set; }
    }
}
