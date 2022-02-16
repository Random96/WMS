using System;
using System.Collections.Generic;
using System.Text;

namespace ru.EmlSoft.WMS.Data.Abstract.Database
{
    public interface IHaveId<T>
    { 
        public T Id { get; }

    }
    public interface IHaveId : IHaveId<int>
    {
    }
}
