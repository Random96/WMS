using ru.emlsoft.WMS.Data.Abstract.Database;
using System;

namespace ru.emlsoft.WMS.Data.Abstract.Access
{
    public class EntityList : IHaveId
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Label { get; set; }
        public string? GroupLabel { get; set; }
        public virtual ICollection<AccessRight>? Rights { get; set; }
    }
}
