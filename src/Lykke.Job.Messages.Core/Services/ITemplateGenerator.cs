using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Messages.Core.Domain;

namespace Lykke.Job.Messages.Core.Services
{
    public interface ITemplateGenerator
    {
        Task<string> GenerateAsync<T>(string merchantId, MessageType messageType, string templateType, T templateVm);
    }
}

