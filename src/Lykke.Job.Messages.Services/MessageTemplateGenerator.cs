using System;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Job.Messages.Core.Services;
using Lykke.Job.SMS.Services;
using Lykke.Service.Messages.Core.Domain;
using Lykke.Service.Messages.Core.Services;
using RazorLight;
using RazorLight.Extensions;

namespace Lykke.Job.Messages.Services
{
    public class MessageTemplateGenerator : ITemplateGenerator
    {
        private readonly ITemplateMessagesService _templateMessagesService;
        public MessageTemplateGenerator(ITemplateMessagesService templateMessagesService)
        {
            _templateMessagesService = templateMessagesService;
        }
        public async Task<string> GenerateAsync<T>(string merchantId, MessageType messageType, string templateType, T templateVm)
        {
            var template = await GetTempate(merchantId, messageType, templateType);
            if (string.IsNullOrEmpty(template))
            {
                return templateVm.ToString();
            }

            var config = EngineConfiguration.Default;

            config.Namespaces.Add("Lykke.Job.Messages.Core.Extensions");

            var engine = EngineFactory.CreateEmbedded(typeof(HealthService), config);

            try
            {
                return await Task.FromResult(engine.ParseString(template, templateVm));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fail template \"{template}\" compilation: {ex.Message}");
                throw;
            }

        }

        private async Task<string> GetTempate(string merchantId, MessageType messageType, string templateType)
        {
            var templates = await _templateMessagesService.GetTemplatesByMerchantId(merchantId);
            var template = templates.FirstOrDefault(t=>(int)t.MessageType == (int)messageType && t.TemplateType.Equals(templateType));
            return template?.Template;
        }
    }
}
