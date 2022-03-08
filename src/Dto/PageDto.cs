using System;
using System.Collections.Generic;
using System.Linq;

namespace ru.EmlSoft.WMS.Data.Dto
{
    public class PageDto<T> where T : class
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public IEnumerable<T> Items { get; set; } = new List<T>();

        public int TotalRows { get; set; }
    }
}
