using System;


namespace Lykke.Service.Messages.Core.Domain
{
    public interface ITemplateResponseModel
    {
        TemplatePostRequestStatus TemplatePostRequestStatus { get; set; }
        Guid? MessagePostId { get; set; }
    }
}
