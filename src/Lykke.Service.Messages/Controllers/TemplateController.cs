using System;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Messages.Core.Domain;
using Lykke.Service.Messages.Core.Services;
using Lykke.Service.Messages.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.Messages.Controllers
{
    [Route("api/[controller]")]
    public class TemplateController : Controller
    {
        private readonly ITemplateMessagesService _templateMessagesService;

        public TemplateController(ITemplateMessagesService templateMessagesService)
        {
            _templateMessagesService = templateMessagesService;
        }
       [HttpPost]
        public async Task<IActionResult> SaveTemplate([FromBody]TemplateModel template)
        {
            
            if (string.IsNullOrEmpty(template.Template))
            {
                return Json(new TemplateResponseModel {TemplatePostRequestStatus = TemplatePostRequestStatus.TemplateEmpty});
            }

            if (!template.TemplateId.HasValue)
            {
                template.TemplateId = Guid.NewGuid();
            }

            if (await _templateMessagesService.SaveTemplate(template))
            {
                return Json(new TemplateResponseModel {TemplatePostRequestStatus = TemplatePostRequestStatus.Ok, MessagePostId = template.TemplateId.Value});
            }

            return Json(new TemplateResponseModel { TemplatePostRequestStatus = TemplatePostRequestStatus.Error });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTemplate()
        {

            return Json(await _templateMessagesService.GetTemplates());
        }

        [HttpGet("{templateId}")]
        public async Task<IActionResult> GetTemplate(Guid templateId)
        {

            return Json(await _templateMessagesService.GetTemplateById(templateId));
        }

        [HttpGet("merchantId/{merchantId}")]
        public async Task<IActionResult> GetAllTemplateByMerchant(string merchantId)
        {

            return Json(from t in await _templateMessagesService.GetTemplates()
                        where t.MerchantId.Equals(merchantId)
                        select t);
        }
    }
}
