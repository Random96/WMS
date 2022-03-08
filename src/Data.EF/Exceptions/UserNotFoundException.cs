using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ru.EmlSoft.WMS.Data.EF.Exceptions
{
    internal class UserNotFoundException : Exception
    {
        public override string Message => "User not found";
    }
}
