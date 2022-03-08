using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Data.Abstract.Database;
using System;

namespace ru.emlsoft.WMS.Data.Abstract.Identity
{
    public class Company : IHaveId
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<User>? Users { get; set; }
        public virtual ICollection<Entity>? Entities { get; set; }
        public virtual ICollection<Appointment>? Appointments { get; set; }
        public virtual ICollection<AccessRight>? Rights { get; set; }
        public virtual ICollection<Position>? Positions { get; set; }
        public virtual ICollection<Personnel.Person>? Persons { get; set; }
    }
}
