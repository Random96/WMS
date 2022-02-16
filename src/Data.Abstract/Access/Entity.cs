using ru.EmlSoft.WMS.Data.Abstract.Database;
using ru.EmlSoft.WMS.Data.Abstract.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ru.EmlSoft.WMS.Data.Abstract.Access
{
    public abstract class Entity : IHaveId, ICompany
    {
        public int Id { get; set; }
        public int EntityType { get; set; }
        public int ? ParentId { get; set; }
        public int CompanyId { get; set; }
        public virtual Entity ParentEntity { get; set; }
        public virtual ICollection<Entity> Entities { get; set; }
        public bool IsDel { get; set; }
        public DateTime LastUpdated { get; set; }
        public int UserId { get; set; }
        public virtual Company Company { get; set; }
    }
}
