using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ru.EmlSoft.WMS.Localization.Resources;

namespace ru.EmlSoft.WMS.Data.EF.Exceptions
{
    internal class DatabaseNotInitException : Exception
    {
        private readonly string ? _message;
        public DatabaseNotInitException(string ? message)
        {
            _message = message;
        }
        public override string Message => string.IsNullOrWhiteSpace(_message)  
            ? SharedResource.ERROR_DB_NOT_INIT 
            : SharedResource.ERROR_DB_NOT_INIT + "(" + _message + ")";
    }
}
