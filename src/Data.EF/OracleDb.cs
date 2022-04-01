using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ru.emlsoft.WMS.Data.EF
{
    internal class OracleDb : Db
    {
        public OracleDb(string connectionString, ILogger<Db> logger) : base(connectionString, logger)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle(_connectionString);
        }
    }
}
