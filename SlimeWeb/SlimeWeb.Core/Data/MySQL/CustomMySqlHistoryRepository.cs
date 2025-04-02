using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Pomelo.EntityFrameworkCore.MySql.Migrations.Internal;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data.MySQL
{

    using Microsoft.EntityFrameworkCore.Migrations;
    using Microsoft.EntityFrameworkCore.Storage;
    using System;
    using System.Data.Common;
    using System.Threading;
    using System.Threading.Tasks;

    public class CustomMySqlHistoryRepository : MySqlHistoryRepository
    {
        private readonly IRelationalConnection _connection;

        public CustomMySqlHistoryRepository(
            HistoryRepositoryDependencies dependencies,
            IRelationalConnection connection)
            : base(dependencies)
        {
            _connection = connection;
        }

        public override IMigrationsDatabaseLock AcquireDatabaseLock()
        {
            return null;
        }
        public override async Task<bool> ExistsAsync(CancellationToken cancellationToken = default)
        {
            await using var command = _connection.DbConnection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM information_schema.tables WHERE table_name = '__EFMigrationsHistory'";

            if (command.Connection.State != System.Data.ConnectionState.Open)
            {
                await command.Connection.OpenAsync(cancellationToken);
            }

            var result = await command.ExecuteScalarAsync(cancellationToken);
            return Convert.ToInt32(result) > 0;
        }

        public override Task<IMigrationsDatabaseLock> AcquireDatabaseLockAsync(CancellationToken cancellationToken = default)
        {
            return (Task<IMigrationsDatabaseLock>)Task.CompletedTask;
        }
         
        //public override Task ReleaseDatabaseLockAsync(CancellationToken cancellationToken = default)
        //{
        //    return Task.CompletedTask; // No-op to disable unlocking
        //}

    }






}
