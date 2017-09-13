namespace Lykke.Job.SMS.Core
{
    public class AppSettings
    {
        public SMSSettings SMSJob { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }

        public class SMSSettings
        {
            public DbSettings Db { get; set; }
            public TwilioSettings Twilio { get; set; }
            public NexmoSettings Nexmo { get; set; }
        }

        public class TwilioSettings
        {
            public string AccountSid { get; set; }
            public string AuthToken { get; set; }
            public string SwissSender { get; set; }
            public string UsSender { get; set; }
        }

        public class NexmoSettings
        {
            public string NexmoAppKey { get; set; }
            public string NexmoAppSecret { get; set; }
            public string UsCanadaSender { get; set; }
            public string DefaultSender { get; set; }
        }

        public class DbSettings
        {
            public string LogsConnString { get; set; }
            public string SmsConnString { get; set; }
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
    }
}