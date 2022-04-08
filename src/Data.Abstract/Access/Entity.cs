using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using System;

namespace ru.emlsoft.WMS.Data.Abstract.Access
{
    public enum EntityType
    {
        Cell = 1, Good, Pack, Pallet, Row, Storage, Tier, StoreOrd, Remains, Partner, Doc
    }
    public abstract class Entity : IHaveId, ICompany
    {
        public Entity(EntityType type)
        {
            EntityType = type;
        }

        public int Id { get; set; }
        public EntityType EntityType { get; }
        public int CompanyId { get; set; }
        public bool IsDel { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.MinValue;
        public int UserId { get; set; }
        public virtual Company Company { get; set; } = null!;
    }
}
