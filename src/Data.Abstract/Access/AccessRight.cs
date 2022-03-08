using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ru.emlsoft.WMS.Data.Abstract.Access
{
    public class AccessRight : IHaveId, ICompany
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int EntityListId { get; set; }
        public virtual EntityList ? Entity { get; set; }
        public int PositionId { get; set; }
        public virtual Position ? Position { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanDelete { get; set; }
        public virtual Company ? Company { get; set; }
    }
}