using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ru.emlsoft.WMS.Data.Abstract.Doc
{
    [Display(Name = "Документы", Description = "Приход", ResourceType = typeof(SharedResource))]
    public class Input : Doc
    {
        public Input() : base(DocType.Input) { }

        public int InputCell { get; set; }
    }
}
