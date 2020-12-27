using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SimpleLog.HttpTrigger.Entities;
using SimpleLog.HttpTrigger.Services;
using SimpleLog.HttpTrigger.ViewModels;

namespace SimpleLog.HttpTrigger
{
    public static class SimpleLog
    {
        const string _errorMessageConnectionString = "the connection string is empty.";

        [FunctionName("SimpleLog")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest request,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function [SimpleLog] processed a request.");

            string connectionStringMSSQL = Environment.GetEnvironmentVariable("ConnectionStringMSQSQL.SimpleLog.HttpTrigger");
            string connectionStringMongoDB = Environment.GetEnvironmentVariable("ConnectionStringMongoDB.SimpleLog.HttpTrigger");
;
            var toSaveMSSQL = !string.IsNullOrEmpty(connectionStringMSSQL);
            var toSaveMongoDB = !string.IsNullOrEmpty(connectionStringMongoDB);
            if (!toSaveMSSQL && !toSaveMongoDB)
            {         
                log.LogError(_errorMessageConnectionString);
                return new UnprocessableEntityObjectResult(_errorMessageConnectionString);      
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            var viewModel = JsonConvert.DeserializeObject<SimpleLogViewModel>(requestBody);

            var entity = new SimpleLogEntity(viewModel.AppName, viewModel.Description, viewModel.Json);
            if (entity.HasNotification())
                return new BadRequestObjectResult(entity.GetNotification());

            var mssql = new MSSQLService(connectionStringMSSQL);
            var mongoDB = new MongoDBService(connectionStringMongoDB);
            
            var savedMSSQL = false;
            if(toSaveMSSQL)
                savedMSSQL = await mssql.Save(log, entity);
            
            var savedMongoDB = false;
            if(toSaveMongoDB)
                savedMongoDB = await mongoDB.Save(log, entity);
            
            var conditionBoth = ((toSaveMSSQL && savedMSSQL) && (toSaveMongoDB && savedMongoDB));
            var conditionMSQSQL = !toSaveMongoDB && (toSaveMSSQL && savedMSSQL);
            var conditionMongoDB = !toSaveMSSQL && (toSaveMongoDB && savedMongoDB);

            if (conditionBoth || conditionMSQSQL || conditionMongoDB)
                return new CreatedResult(
                    GetResourceUrl(request, entity.Id), 
                    $"The object was saved in the database. Id={entity.Id};");
            else
                return new UnprocessableEntityObjectResult(
                    $"The object could not be saved to the database.");
        }

        private static string GetResourceUrl(HttpRequest request, Guid id)
            => $"{request.Scheme}://{request.Host}{request.Path.Value}/{id}";
    }
}
