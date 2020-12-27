using System;
using System.Collections.Generic;

namespace SimpleLog.HttpTrigger.Entities
{
    public class SimpleLogEntity
    {
        public SimpleLogEntity(string appName, string description, string json)
        {
            this.Notifications = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(appName) || appName.Length < 5 || appName.Length > 150)
                this.Notifications.Add(nameof(appName), $"the {nameof(appName)} field cannot be null, the field must contain between 5 and 150 characters.");

            if (string.IsNullOrEmpty(description) || description.Length < 5)
                this.Notifications.Add(nameof(description), $"the {nameof(description)} field cannot be null, the field must contain more than 5 characters.");

            if (string.IsNullOrEmpty(json) || json.Length < 5)
                this.Notifications.Add(nameof(json), $"the {nameof(json)} field cannot be null, the field must contain more than 5 characters.");

            if(this.Notifications.Count > 0)
                return;

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
        private Dictionary<string, string> Notifications { get; set; }

        public bool HasNotification() => this.Notifications.Count > 0;
        public Dictionary<string, string> GetNotification() => this.Notifications;
    }
}
