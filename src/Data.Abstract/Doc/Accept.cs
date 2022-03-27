using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ru.emlsoft.WMS.Data.Dto.Doc;

namespace ru.emlsoft.WMS.Data.Abstract.Doc
{
    public class Accept : Doc
    {
        public Accept() : base(DocType.Accept) { }
        public int Qty { get; set; }
    }
}
