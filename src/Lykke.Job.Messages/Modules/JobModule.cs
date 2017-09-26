using System;
using Autofac;
using AzureStorage.Queue;
using Common.Log;
using Lykke.Job.Messages.Core.Services;
using Lykke.Job.Messages.QueueConsumers;
using Lykke.Job.Messages.Services;
using Lykke.Job.SMS.Core;
using Lykke.Job.SMS.Services;
using Lykke.Service.SMS.Client;
using IHealthService = Lykke.Job.SMS.Core.Services.IHealthService;

namespace Lykke.Job.Messages.Modules
{
    public class JobModule : Module
    {
        private readonly AppSettings.MessageTemplateJobSettings _settings;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies

        public JobModule(AppSettings.MessageTemplateJobSettings settings, ILog log)
        {
            _settings = settings;
            _log = log;

        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SmsQueueConsumer>().SingleInstance();

            

            builder.RegisterInstance(_settings)
                .SingleInstance();

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();
            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance()
                .WithParameter(TypedParameter.From(TimeSpan.FromSeconds(30)));

            RegistermSmsServices(builder);

            // NOTE: Service registrations example:

            builder.RegisterType<SmsSender>()
                .As<ISmsSender>();

            var smsClient = new SMSClient(_settings.Services.MessageServiceUrl, _log);
            builder.RegisterInstance(smsClient).As<ISMSClient>();

        }

        private void RegistermSmsServices(ContainerBuilder builder)
        {
            var smsQueue = new AzureQueueExt(_settings.Db.ClientPersonalInfoConnString, "smsmerchantqueue");
            var smsQueueReader = new QueueReader(smsQueue, "SmsQueueReader", 3000, _log);

            builder.Register<IQueueReader>(x => smsQueueReader).SingleInstance();
            builder.RegisterType<MessageTemplateGenerator>().As<ITemplateGenerator>();
          
        }
    }
}