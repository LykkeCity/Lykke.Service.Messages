using System;
using Lykke.Service.Messages.Core.Domain;

namespace Lykke.Service.Messages.Services
{
    public class TemplateModel : ITemplateModel
    {
        public Guid? TemplateId { get; set; }
        public string MerchantId { get; set; }
        public string Template { get; set; }
        public MessageType MessageType { get; set; }
        public string TemplateType { get; set; }
    }
}
