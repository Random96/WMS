using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ru.emlsoft.WMS.Data.Abstract.Access
{
    public enum EntityType
    {
        Cell = 1, Good, Pack,Pallet,Row, Storage,Tier
    }
    public abstract class Entity : IHaveId, ICompany
    {
        public Entity(EntityType type)
        {
            EntityType = type;
        }

        public int Id { get; set; }
        public EntityType EntityType { get; }
        public int ? ParentId { get; set; }
        public int CompanyId { get; set; }
        public virtual Entity ? ParentEntity { get; set; }
        public virtual ICollection<Entity> ? Entities { get; set; }
        public bool IsDel { get; set; }
        public DateTime LastUpdated { get; set; }
        public int UserId { get; set; }
        public virtual Company ? Company { get; set; }
    }
}
