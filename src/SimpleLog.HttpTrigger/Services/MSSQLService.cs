using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SimpleLog.HttpTrigger.Entities;
using SimpleLog.HttpTrigger.Services.Interfaces;

namespace SimpleLog.HttpTrigger.Services
{
    public class MSSQLService : IDatabaseService
    {
        private readonly string _connectionString;

        public MSSQLService(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public async Task<bool> SaveAsync(ILogger log, SimpleLogEntity entity)
        {
            var sql = "INSERT INTO SimpleLog (Id, AppName, Description, Json, Created) VALUES (@Id, @AppName, @Description, @Json, @Created)";

            try
            {
                using SqlConnection connection = new SqlConnection(this._connectionString);
                using SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Id", entity.Id);
                command.Parameters.AddWithValue("@AppName", entity.AppName);
                command.Parameters.AddWithValue("@Description", entity.Description);
                command.Parameters.AddWithValue("@Json", entity.Json);
                command.Parameters.AddWithValue("@Created", entity.Created);
                
                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync() > 0;
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}