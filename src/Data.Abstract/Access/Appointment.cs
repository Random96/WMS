using ru.emlsoft.WMS.Data.Abstract.Database;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using ru.emlsoft.WMS.Data.Abstract.Personnel;
using System;

namespace ru.emlsoft.WMS.Data.Abstract.Access
{
    /// <summary>
    /// Назначение на должность (ченство в группах)
    /// ToDate определяет окончание срока действия
    /// </summary>
    public class Appointment : IHaveId, ICompany
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        public int PersonId { get; set; }
        public virtual Person? Person { get; set; }
        public int PositionId { get; set; }
        public virtual Position Position { get; set; } = new Position();
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
