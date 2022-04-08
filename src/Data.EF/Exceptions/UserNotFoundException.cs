using System;
using System.Linq;

namespace ru.emlsoft.WMS.Data.EF.Exceptions
{
    internal class UserNotFoundException : Exception
    {
        public override string Message => "User not found";
    }
}
