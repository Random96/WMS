using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Abstract.Doc
{
    [Display(Name = "Документы", Description = "Размещение", ResourceType = typeof(SharedResource))]
    public class Accept : Doc
    {
        public Accept() : base(DocType.Accept) { }
        public int Qty { get; set; }
    }
}
