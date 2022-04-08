using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ru.emlsoft.WMS.Data.EF
{
    internal class MsSqlDb : Db
    {
        public MsSqlDb(string connectionString, ILogger<Db> logger) : base(connectionString, logger)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
