using ru.emlsoft.WMS.Data.Abstract.Database;
using System;

namespace ru.emlsoft.WMS.Data.Abstract.Access
{
    public class EntityList : IHaveId
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Label { get; set; } = String.Empty;
        public string GroupLabel { get; set; } = String.Empty;
        public virtual ICollection<AccessRight>? Rights { get; set; }
    }
}
