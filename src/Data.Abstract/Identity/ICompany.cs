using System;
using System.Collections.Generic;
using System.Text;

namespace ru.EmlSoft.WMS.Data.Abstract.Identity
{
    public interface ICompany
    {
        int CompanyId { get; set; }
        Company Company { get; set; }
    }
}
