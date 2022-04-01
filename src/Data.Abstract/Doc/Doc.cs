using ru.emlsoft.WMS.Data.Abstract.Access;
using ru.emlsoft.WMS.Data.Dto.Doc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Abstract.Doc
{

    [Display(Name = "DOC", Description = "DOC")]
    public class Doc : Entity
    {
#pragma warning disable CS8618
        public Doc() : base(EntityType.Doc)
        {
            DocType = DocType.none;
        }

        public Doc(DocType docType) : base(EntityType.Doc)
        {
            DocType = docType;
        }
        public DocType DocType { get; }
        public string DocNumber { get; set; } = string.Empty;
        public DateTime? StartProcess { get; set; }
        public DateTime? EndProcess { get; set; }
        public DateTime DocDate { get; set; }
        public bool Accepted { get; set; }
        public int PartnerId { get; set; }
        public int StorageId { get; set; }
        public bool RowLevelApprove { get; set; }

        public virtual ICollection<DocSpec> DocSpecs { get; set; } = new List<DocSpec>();
        public virtual Partner Partner { get; set; }
        public virtual Storage.Storage Storage { get; set; }


        public virtual Input Input { get; set; }
        public virtual Accept Accept { get; set; }
    }
#pragma warning restore CS8618
}
