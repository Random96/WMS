using ru.emlsoft.WMS.Data.Dto.Doc;
using System;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Abstract.Doc
{
    public class Accept : Doc
    {
        public Accept() : base(DocType.Accept) { }
        public int Qty { get; set; }
    }
}
