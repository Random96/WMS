using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ru.emlsoft.WMS.Data.Abstract.Personnel
{
    public class Person : IHaveId, ICompany
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string ? FirstName { get; set; }
        public string ? MiddleName { get; set; }
        public string ? LastName { get; set; }
        public bool Gender { get; set; }
        public DateTime ? BirthDay { get; set; }

        public virtual ICollection<Appointment> ? Appointments { get; set; }
        public virtual User ? User { get; set; }
        public Company ? Company { get; set; }
    }
}
