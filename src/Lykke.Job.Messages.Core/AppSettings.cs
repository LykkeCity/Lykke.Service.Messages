namespace Lykke.Job.SMS.Core
{
    public class AppSettings
    {
        public MessageTemplateJobSettings MessageTemplateJob { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }

        public class MessageTemplateJobSettings
        {
            public DbSettings Db { get; set; }
            public ServicesUrl Services { get; set; }
        }

       

        public class DbSettings
        {
            public string LogsConnString { get; set; }
            public string ClientPersonalInfoConnString { get; set; }
        }

        public class SlackNotificationsSettings
        {
            public AzureQueueSettings AzureQueue { get; set; }
        }

        public class AzureQueueSettings
        {
            public string ConnectionString { get; set; }

            public string QueueName { get; set; }
        }

        public class ServicesUrl
        {
            public string MessageServiceUrl { get; set; }
        }
    }

    
}