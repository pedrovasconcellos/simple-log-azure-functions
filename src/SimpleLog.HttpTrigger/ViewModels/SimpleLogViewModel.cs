
using System;

namespace SimpleLog.HttpTrigger.ViewModels
{
    public class SimpleLogViewModel
    {
        public Guid OurClientId { get; set; }
        public Guid ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string Description { get; set; }
        public string Json { get; set; }
    }
}
