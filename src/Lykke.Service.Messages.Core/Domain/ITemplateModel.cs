using System;

namespace Lykke.Service.Messages.Core.Domain
{
    public interface ITemplateModel
    {
        Guid? TemplateId { get; set; }
        string MerchantId { get; set; }
        string Template { get; set; }
        MessageType MessageType { get; set; }
        string TemplateType { get; set; }
    }
}