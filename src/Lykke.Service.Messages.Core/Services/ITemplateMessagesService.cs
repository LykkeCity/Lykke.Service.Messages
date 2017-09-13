using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Messages.Core.Domain;


namespace Lykke.Service.Messages.Core.Services
{
    public interface ITemplateMessagesService
    {
        Task<bool> SaveTemplate(ITemplateModel template);
        Task<ITemplateModel> GetTemplateById(Guid templateId);
        Task<List<ITemplateModel>> GetTemplatesByMerchantId(string merchantIds);
        Task<List<ITemplateModel>> GetTemplates();
    }
}
