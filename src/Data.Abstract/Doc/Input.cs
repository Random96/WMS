using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using ru.emlsoft.WMS.Data.Dto.Doc;
using ru.emlsoft.WMS.Data.Abstract.Storage;

namespace ru.emlsoft.WMS.Data.Abstract.Doc
{
    public class Input : Doc
    {
        public Input() : base(DocType.Input) { }

        public int InputCellId { get; set; }


        public virtual Cell InputCell { get; set; }
    }
}
