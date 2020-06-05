using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SimpleLog.HttpTrigger
{
    public static class Function1
    {
        const string _errorMessageConnectionString = "the connection string is empty.";

        [FunctionName("SimpleLog")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest request,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function [SimpleLog] processed a request.");

            string _connectionString = Environment.GetEnvironmentVariable("ConnectionString.SimpleLog.HttpTrigger");
            if (string.IsNullOrEmpty(_connectionString))
            {         
                log.LogError(_errorMessageConnectionString);
                return new UnprocessableEntityObjectResult(_errorMessageConnectionString);      
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            var viewModel = JsonConvert.DeserializeObject<SimpleLogViewModel>(requestBody);

            var consistDic = Consist(viewModel);
            if (consistDic.Count > 0)
                return new BadRequestObjectResult(consistDic);

            var entity = new SimpleLogEntity(viewModel.AppName, viewModel.Description, viewModel.Json);

            var saved = InsertSimpleLog(entity, _connectionString, log);
            if (saved > 0)
                return new CreatedResult(string.Empty, "The object was saved in the database.");
            else
                return new UnprocessableEntityObjectResult("The object could not be saved to the database.");
        }

        private static Dictionary<string, string> Consist(SimpleLogViewModel viewModel) 
        {
            var consistDic = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(viewModel.AppName) || viewModel.AppName.Length < 5 || viewModel.AppName.Length > 150)
                consistDic.Add(nameof(viewModel.AppName), $"the {nameof(viewModel.AppName)} field cannot be null, the field must contain between 5 and 150 characters.");

            if (string.IsNullOrEmpty(viewModel.Description) || viewModel.Description.Length < 5)
                consistDic.Add(nameof(viewModel.Description), $"the {nameof(viewModel.Description)} field cannot be null, the field must contain more than 5 characters.");

            if (string.IsNullOrEmpty(viewModel.Json) || viewModel.Json.Length < 5)
                consistDic.Add(nameof(viewModel.Json), $"the {nameof(viewModel.Json)} field cannot be null, the field must contain more than 5 characters.");

            return consistDic;
        }

        private static int InsertSimpleLog(SimpleLogEntity entity, string connectionString, ILogger log)
        {
            var sql = "INSERT INTO SimpleLog (Id, AppName, Description, Json, Created) VALUES (@Id, @AppName, @Description, @Json, @Created)";

            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                using SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Id", entity.Id);
                command.Parameters.AddWithValue("@AppName", entity.AppName);
                command.Parameters.AddWithValue("@Description", entity.Description);
                command.Parameters.AddWithValue("@Json", entity.Json);
                command.Parameters.AddWithValue("@Created", entity.Created);
                connection.Open();

                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return 0;
            }
        }
    }
}
