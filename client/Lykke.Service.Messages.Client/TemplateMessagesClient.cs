using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Messages.Core.Domain;
using Lykke.Service.Messages.Services;
using Newtonsoft.Json;
using swagger;


namespace Lykke.Service.Messages.Client
{
    public class TemplateMessagesClient : ITemplateMessagesClient, IDisposable
    {
        private readonly ILog _log;
        private readonly ITemplateMessagesAPI _service;

        public TemplateMessagesClient(string serviceUrl, ILog log)
        {
            _log = log;
            _service = new TemplateMessagesAPI(new Uri(serviceUrl));
        }

       
        public void Dispose()
        {
            //if (_service == null)
            //    return;
            //_service.Dispose();
            //_service = null;
        }

        public async Task<TemplateResponseModel> SaveTemplate(TemplateModel template)
        {
            try
            {
               await _service.ApiTemplatePostWithHttpMessagesAsync(new swagger.Models.TemplateModel
                {
                    MerchantId = template.MerchantId,
                    MessageType = (swagger.Models.MessageType)(int)template.MessageType,
                    Template = template.Template,
                    TemplateId = template.TemplateId,
                    TemplateType = template.TemplateType
                });
                return new TemplateResponseModel();
                //return result.Response as SmsResponseModel;
            }
            catch (Exception e)
            {
                await _log.WriteFatalErrorAsync("Template Message Serivce Client", "Store template", JsonConvert.SerializeObject(template),
                    e, DateTime.UtcNow);
                throw;
            }
        }

        public async Task<ITemplateModel> GetTemplateById(Guid templateId)
        {
            try
            {
                var result = await _service.ApiTemplateByTemplateIdGetWithHttpMessagesAsync(templateId);
                return JsonConvert.DeserializeObject<TemplateModel>(await result.Response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                await _log.WriteFatalErrorAsync("Template Message Serivce Client", "Get template by Id", string.Empty, e, DateTime.UtcNow);
                throw;
            }
        }

        public async Task<List<ITemplateModel>> GetTemplatesByMerchantId(string merchantIds)
        {
            try
            {
                var result = await _service.ApiTemplateMerchantIdByMerchantIdGetWithHttpMessagesAsync(merchantIds);
                return JsonConvert.DeserializeObject<List<TemplateModel>>(await result.Response.Content.ReadAsStringAsync())
                            .Cast<ITemplateModel>().ToList();
            }
            catch (Exception e)
            {
                await _log.WriteFatalErrorAsync("Template Message Serivce Client", "Get templates by merchantIds", string.Empty, e, DateTime.UtcNow);
                throw;
            }
        }

        public async Task<List<ITemplateModel>> GetTemplates()
        {
            try
            {
                var result = await _service.ApiTemplateGetWithHttpMessagesAsync();
                return JsonConvert.DeserializeObject<List<TemplateModel>>(await result.Response.Content.ReadAsStringAsync())
                    .Cast<ITemplateModel>().ToList();
            }
            catch (Exception e)
            {
                await _log.WriteFatalErrorAsync("Template Message Serivce Client", "Get all templates", string.Empty, e, DateTime.UtcNow);
                throw;
            }
        }
    }
}
