using ru.emlsoft.WMS.Data.Abstract.Database;
using System;
using System.Linq;
using ru.emlsoft.WMS.Data.Dto.Doc;


namespace ru.emlsoft.WMS.Data.Abstract.Doc
{
    public class DocStorage
    {
        private readonly IWMSDataProvider _db;

        public DocStorage(IWMSDataProvider db)
        {
            _db = db;
        }

        public int UserId { get; set; }

        public async Task ApplyDoc(int id, CancellationToken cancellationToken)
        {
            var doc = await _db.GetDocByIdAsync(id, UserId, cancellationToken);

            if (doc == null)
                throw new Exception();

            if(doc.RowLevelApprove)
                throw new Exception();

            switch (doc.DocType)
            {
                case DocType.Input:
                    await ApplyInputAsync(doc, cancellationToken);
                    break;
            }
        }


        private async Task ApplyInputAsync(Doc doc, CancellationToken cancellationToken)
        {
            var storeords = doc.DocSpecs.Select(x=> new StoreOrd() { CellId = doc.Input.InputCellId, GoodId = x.GoodId, PalletId = x.PalletId, Qty = x.Qty, DocSpecId = x.Id }).ToArray();

            await _db.ApplyDocAsync(doc.Id, UserId, storeords, cancellationToken);
        }
    }
}
