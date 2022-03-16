using ru.emlsoft.WMS.Data.Abstract.Database;
using System;
using System.Linq;

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

        public async Task ApproveDoc(int id, CancellationToken cancellationToken)
        {
            var doc = await _db.GetDocByIdAsync(id, UserId, cancellationToken);

            if (doc == null)
                throw new Exception();

            if(doc.RowLevelApprove)
                throw new Exception();

            switch (doc.DocType)
            {
                case DocType.Input:
                    await ApproveInputAsync(doc, cancellationToken);
                    break;
            }
        }


        private async Task ApproveInputAsync(Doc doc, CancellationToken cancellationToken)
        {
            var storeords = doc.DocSpecs.Where(x=>x.ToCellId != null).Select(x=> new StoreOrd() { CellId = x.ToCellId ?? 0, GoodId = x.GoodId, PalletId = x.PalletId, Qty = x.Qty, DocSpecId = x.Id }).ToArray();

            await _db.ApplyDocAsync(UserId, storeords, cancellationToken);
        }
    }
}
