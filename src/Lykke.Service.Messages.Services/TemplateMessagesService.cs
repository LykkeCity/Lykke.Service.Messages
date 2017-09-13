using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Messages.Core.Domain;
using Lykke.Service.Messages.Core.Services;
using Newtonsoft.Json;


namespace Lykke.Service.Messages.Services
{
    public class TemplateMessagesService : ITemplateMessagesService
    {
        private readonly ITemplateMessageRepository _templateMessageServiceRepository;
        private readonly ILog _log;


        public TemplateMessagesService(ITemplateMessageRepository templateMessageServiceRepository, ILog log)
        {
            _templateMessageServiceRepository = templateMessageServiceRepository;
            _log = log;
        }

        public async Task<bool> SaveTemplate(ITemplateModel template)
        {
            try
            {
                var templates = await GetTemplatesByMerchantId(template.MerchantId);
                var existTemplate = templates.FirstOrDefault(t => t.TemplateType.Equals(template.TemplateType));
                var canBeSaved = true;
                if (existTemplate?.TemplateId != null)
                {
                    canBeSaved = await _templateMessageServiceRepository.Remove(existTemplate.TemplateId.Value);
                }
                if (!canBeSaved)
                {
                    throw new ApplicationException("Can't delete existing template");
                }
                await _templateMessageServiceRepository.Write(template);
            }
            catch (Exception e)
            {
                await _log.WriteErrorAsync("Templete Message", "Saving a template",
                    template == null ? string.Empty : JsonConvert.SerializeObject(template), e);
                return false;
            }
            return true;

        }

        public async Task<ITemplateModel> GetTemplateById(Guid templateId)
        {
            return await _templateMessageServiceRepository.GetByIdAsync(templateId);
        }

        public async Task<List<ITemplateModel>> GetTemplatesByMerchantId(string merchantIds)
        {
            return (from t in await _templateMessageServiceRepository.GetAllAsync()
                    where t.MerchantId.Equals(merchantIds)
                    select t).ToList();
        }

        public async Task<List<ITemplateModel>> GetTemplates()
        {
            return await _templateMessageServiceRepository.GetAllAsync();
        }
    }
}
