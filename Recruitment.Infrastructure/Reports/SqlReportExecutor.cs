using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Services.Common;
using System.Data;
using System.Data.Common;

namespace Recruitment.Infrastructure.Reports
{
    public class SqlReportExecutor : IReportExecutor
    {
        private readonly DbConnectionFactory _factory;

        public SqlReportExecutor(DbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<DataTable> ExecuteAsync(
            string storedProcedure,
            Dictionary<string, object?> parameters)
        {
            using var connection = _factory.Create();
            await ((DbConnection)connection).OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = storedProcedure;
            command.CommandType = CommandType.StoredProcedure;

            var dbCommand = (DbCommand)command;

            foreach (var param in parameters)
            {
                var dbParam = dbCommand.CreateParameter();
                dbParam.ParameterName = "@" + param.Key;
                dbParam.Value = param.Value ?? DBNull.Value;

                dbCommand.Parameters.Add(dbParam);
            }

            using var reader = await dbCommand.ExecuteReaderAsync();
            var table = new DataTable();
            table.Load(reader);

            return table;
        }
    }
}
