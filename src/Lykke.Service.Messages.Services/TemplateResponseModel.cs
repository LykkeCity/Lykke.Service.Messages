using System;
using Lykke.Service.Messages.Core.Domain;


namespace Lykke.Service.Messages.Services
{
    public class TemplateResponseModel : ITemplateResponseModel
    {
        public TemplatePostRequestStatus TemplatePostRequestStatus { get; set; }
        public Guid? MessagePostId { get; set; }
    }
}
