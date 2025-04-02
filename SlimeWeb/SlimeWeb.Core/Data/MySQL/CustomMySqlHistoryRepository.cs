using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Pomelo.EntityFrameworkCore.MySql.Migrations.Internal;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data.MySQL
{
    
    public class CustomMySqlHistoryRepository : MySqlHistoryRepository
    {
        private readonly RelationalConnection _connection;

        public CustomMySqlHistoryRepository(
            HistoryRepositoryDependencies dependencies,
            RelationalConnection connection)
            : base(dependencies)
        {
            _connection = connection;
        }

        // Override the migration history existence check
        public override async Task<bool> ExistsAsync(CancellationToken cancellationToken = default)
        {
            using var command = _connection.DbConnection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM information_schema.tables WHERE table_name = '__EFMigrationsHistory'";

            if (command.Connection.State != System.Data.ConnectionState.Open)
            {
                await command.Connection.OpenAsync(cancellationToken);
            }

            var result = await command.ExecuteScalarAsync(cancellationToken);
            return Convert.ToInt32(result) > 0;
        }

        // Override lock acquisition to disable locking
        //public override Task AcquireDatabaseLockAsync(CancellationToken cancellationToken = default)
        //{
        //    return Task.CompletedTask; // No-op to disable locking
        //}

        //// Override lock release to prevent lock logic
        //public override Task ReleaseDatabaseLockAsync(CancellationToken cancellationToken = default)
        //{
        //    return Task.CompletedTask; // No-op to disable unlocking
        //}
    }





}
