using System;
using System.Linq;

namespace ru.emlsoft.WMS.Data.EF.Exceptions
{
    public class NoUserDefinedException : Exception
    {
        public override string Message => "UserId not defined";
    }
}
