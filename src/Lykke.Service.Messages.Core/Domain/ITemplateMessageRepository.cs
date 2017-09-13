using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Messages.Core.Domain
{
    public interface ITemplateMessageRepository
    {
        Task<List<ITemplateModel>> GetAllAsync();
        Task<ITemplateModel> GetByIdAsync(Guid templateId);
        Task Write(ITemplateModel template);
    }
}
