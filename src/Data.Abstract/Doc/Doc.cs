using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Localization.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Abstract.Doc
{
    public enum DocType
    {
        Input,
        Accept,
        Picking,
        Output,
        Sort,
        Inventarization
    }

    [Display(Name = "Документы", Description = "Документы", ResourceType = typeof(SharedResource))]
    public abstract class Doc : Entity
    {
        public Doc(DocType docType) : base(EntityType.Doc)
        {
            DocType = docType;
        }
        public DocType DocType { get; }
        public string DocNumber { get; set; } = string.Empty;
        public DateTime StartProcess { get; set; }
        public DateTime ? EndProcess { get; set; }
        public DateTime DocDate { get; set; }
        public bool Accepted { get; }
        public int PartnerId { get; set; }
        public int Store { get; set; }
        public bool RowLevelApprove { get; set; }

        public virtual ICollection<DocSpec> DocSpecs { get; set; } = new List<DocSpec>();
        public virtual Partner Partner { get; set; }


        public virtual Input Input { get; set; }
        public virtual Accept Accept { get; set; }
    }
}
