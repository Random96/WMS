using ru.EmlSoft.WMS.Data.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ru.EmlSoft.WMS.Data.Abstract.Access
{
    public class EntityList : IHaveId
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }

        public virtual ICollection<AccessRight> Rights { get; set; }
    }
}
