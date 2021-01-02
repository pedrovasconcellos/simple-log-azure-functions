using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace SimpleLog.HttpTrigger.Entities
{
    public class SimpleLogEntity
    {
        public SimpleLogEntity(
            Guid ourClientId, Guid applicationId, LogLevel logLevel, string description, string json)
        {
            this.Notifications = new Dictionary<string, string>();
            
            if (ourClientId == Guid.Empty)
                this.Notifications.Add(nameof(this.OurClientId), $"the {nameof(this.OurClientId)} invalid.");

            if (applicationId == Guid.Empty)
                this.Notifications.Add(nameof(this.ApplicationId), $"the {nameof(this.ApplicationId)} invalid.");

            if (!Enum.IsDefined(typeof(LogLevel), logLevel))
                this.Notifications.Add(nameof(logLevel), $"the {nameof(this.LogLevel)} invalid. {nameof(this.LogLevel)} available [Trace=0, Debug=1, Information=2, Warning=3, Error=4, Critical=5, None=6]");

            if (string.IsNullOrEmpty(description) || description.Length < 5 || description.Length > 80)
                this.Notifications.Add(nameof(this.Description), $"the {nameof(this.Description)} field cannot be null, the field must contain more than 5 characters and less than 81 characters.");

            if (string.IsNullOrEmpty(json) || json.Length < 5)
                this.Notifications.Add(nameof(this.Json), $"the {nameof(this.Json)} field cannot be null, the field must contain more than 5 characters.");

            if(this.Notifications.Count > 0)
                return;

            this.Id = Guid.NewGuid();
            this.OurClientId = ourClientId;
            this.ApplicationId = applicationId;
            this.LogLevel = logLevel;
            this.Description = description;
            this.Json = json;
            this.Created = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }
        public Guid OurClientId { get; set; }
        public Guid ApplicationId { get; set; }
        public LogLevel LogLevel { get; private set; }
        public string Description { get; private set; }
        public string Json { get; private set; }
        public DateTime Created { get; private set; }
        private Dictionary<string, string> Notifications { get; set; }

        public bool HasNotification() => this.Notifications.Count > 0;
        public Dictionary<string, string> GetNotification() => this.Notifications;
    }
}
