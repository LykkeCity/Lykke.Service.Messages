namespace Lykke.Service.Messages.Core
{
    public class AppSettings
    {
        public TemplateMessageSettings TemplateMessageSettings { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }

    public class TemplateMessageSettings
    {
        public DbSettings Db { get; set; }
    }

    public class DbSettings
    {
        public string LogsConnString { get; set; }
        public string TemplateMessageConnString { get; set; }
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
