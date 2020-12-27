using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SimpleLog.HttpTrigger.Entities;

namespace SimpleLog.HttpTrigger.Services.Interfaces
{
    public interface IDatabaseService
    {
        Task<bool> SaveAsync(ILogger log, SimpleLogEntity entity);
    }
}