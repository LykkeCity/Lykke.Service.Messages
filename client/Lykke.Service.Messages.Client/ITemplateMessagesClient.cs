
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Messages.Core.Domain;
using Lykke.Service.Messages.Services;


namespace Lykke.Service.Messages.Client
{
    public interface ITemplateMessagesClient
    {
        Task<TemplateResponseModel> SaveTemplate(TemplateModel template);
        Task<ITemplateModel> GetTemplateById(Guid templateId);
        Task<List<ITemplateModel>> GetTemplatesByMerchantId(string merchantIds);
        Task<List<ITemplateModel>> GetTemplates();
    }
}