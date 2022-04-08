using System;
using System.Linq;

namespace ru.emlsoft.WMS.Data.Dto.Storage
{
    public class RemainsDto
    {
        public int Id { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.MinValue;
        public DateTime Date { get; set; }
        public string? Good { get; set; }
        public string? Cell { get; set; }
        public string? Pallet { get; set; }
        public int Qty { get; set; }
        public bool Current { get; set; }
    }
}
