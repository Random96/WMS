using ru.emlsoft.WMS.Data.Abstract.Storage;
using ru.emlsoft.WMS.Data.Dto.Doc;
using System;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Abstract.Doc
{
    public class Input : Doc
    {
        public Input() : base(DocType.Input) { }

        public int InputCellId { get; set; }


        public virtual Cell InputCell { get; set; } = null!;
    }
}
