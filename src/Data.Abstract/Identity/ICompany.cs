using System;

namespace ru.emlsoft.WMS.Data.Abstract.Identity
{
    public interface ICompany
    {
        int CompanyId { get; set; }
        Company Company { get; set; }
    }
}
