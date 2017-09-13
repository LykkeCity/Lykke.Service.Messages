using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureStorage.Blob;
using Common.Log;
using Lykke.Core;
using Lykke.Service.Messages.Core;
using Lykke.Service.Messages.Core.Domain;
using Lykke.Service.Messages.Core.Services;
using Lykke.Service.Messages.Repositories;
using Lykke.Service.Messages.Services;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Messages.Modules
{
    public class ServiceModule : Module
    {
        private readonly TemplateMessageSettings _settings;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;

        public ServiceModule(TemplateMessageSettings settings, ILog log)
        {
            _settings = settings;
            _log = log;

            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_settings)
                .SingleInstance();

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

           

            var templateMessageRepository = new TemplateMessageRepository(new AzureBlobStorage(_settings.Db.TemplateMessageConnString), "templatemessages");
            
            builder.RegisterInstance(templateMessageRepository)
                .As<ITemplateMessageRepository>()
                .SingleInstance();


            builder.RegisterType<TemplateMessagesService>()
                .As<ITemplateMessagesService>()
                .SingleInstance();

            builder.Populate(_services);
        }
    }
}
