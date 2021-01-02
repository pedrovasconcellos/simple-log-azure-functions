using System;
using System.Collections.Generic;

namespace SimpleLog.HttpTrigger.Entities
{
    public class SimpleLogEntity
    {
        public SimpleLogEntity(Guid ourClientId, Guid applicationId, string applicationName, string description, string json)
        {
            if (ourClientId == Guid.Empty)
                this.Notifications.Add(nameof(ourClientId), $"the {nameof(ourClientId)} invalid.");

            if (applicationId == Guid.Empty)
                this.Notifications.Add(nameof(applicationId), $"the {nameof(applicationId)} invalid.");

            this.Notifications = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(applicationName) || applicationName.Length < 5 || applicationName.Length > 150)
                this.Notifications.Add(nameof(applicationName), $"the {nameof(applicationName)} field cannot be null, the field must contain between 5 and 150 characters.");

            if (string.IsNullOrEmpty(description) || description.Length < 5)
                this.Notifications.Add(nameof(description), $"the {nameof(description)} field cannot be null, the field must contain more than 5 characters.");

            if (string.IsNullOrEmpty(json) || json.Length < 5)
                this.Notifications.Add(nameof(json), $"the {nameof(json)} field cannot be null, the field must contain more than 5 characters.");

            if(this.Notifications.Count > 0)
                return;

            this.Id = Guid.NewGuid();
            this.OurClientId = ourClientId;
            this.ApplicationId = applicationId;
            this.ApplicationName = applicationName;
            this.Description = description;
            this.Json = json;
            this.Created = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }
        public Guid OurClientId { get; set; }
        public Guid ApplicationId { get; set; }
        public string ApplicationName { get; private set; }
        public string Description { get; private set; }
        public string Json { get; private set; }
        public DateTime Created { get; private set; }
        private Dictionary<string, string> Notifications { get; set; }

        public bool HasNotification() => this.Notifications.Count > 0;
        public Dictionary<string, string> GetNotification() => this.Notifications;
    }
}
