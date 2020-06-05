using System;

namespace SimpleLog.HttpTrigger
{
    public class SimpleLogEntity
    {
        public SimpleLogEntity(string appName, string description, string json)
        {
            this.Id = Guid.NewGuid();
            this.AppName = appName;
            this.Description = description;
            this.Json = json;
            this.Created = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }
        public string AppName { get; private set; }
        public string Description { get; private set; }
        public string Json { get; private set; }
        public DateTime Created { get; private set; }
    }
}
