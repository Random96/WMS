using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.Linq;

namespace ru.emlsoft.WMS.Data.EF.Exceptions
{
    internal class DatabaseNotInitException : Exception
    {
        private readonly string? _message;
        public DatabaseNotInitException(string? message)
        {
            _message = message;
        }
        public override string Message => string.IsNullOrWhiteSpace(_message)
            ? SharedResource.ERROR_DB_NOT_INIT
            : SharedResource.ERROR_DB_NOT_INIT + "(" + _message + ")";
    }
}
