using ru.EmlSoft.WMS.Data.Abstract.Database;
using ru.EmlSoft.WMS.Data.Abstract.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ru.EmlSoft.WMS.Data.Abstract.Personnel
{
    public enum DocumentType
    {
        Pachport,
        Voennik,
        MilitaryPass,
        LiluDallasMultyPass
    }

    public class PersonDoc : IHaveId, ICompany
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public DocumentType DocumentType { get; set; }
        public string ? Number { get; set; }
        public DateTime Date { get; set; }
        public Company ? Company { get; set; }
    }
}
