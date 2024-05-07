using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Broker.EntityFrameworkCore
{
    public static class BrokerDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<BrokerDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<BrokerDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
