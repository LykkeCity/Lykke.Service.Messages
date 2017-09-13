using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureStorage;
using Common;
using Lykke.Service.Messages.Core.Domain;
using Lykke.Service.Messages.Services;
using Newtonsoft.Json;

namespace Lykke.Service.Messages.Repositories
{
    public class TemplateMessageRepository : ITemplateMessageRepository
    {
        private readonly string _container;
        private readonly IBlobStorage _storage;

        public TemplateMessageRepository(IBlobStorage storage, string container)
        {
            _container = container;
            _storage = storage;
        }

        public async Task<List<ITemplateModel>> GetAllAsync()
        {
            var result = new List<ITemplateModel>();
            try
            {
                var keys = await _storage.GetListOfBlobKeysAsync(_container);
                foreach (var k in keys)
                {
                    result.Add(await GetByIdAsync(k));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
         
          
            return result;
        }

        private async Task<ITemplateModel> GetByIdAsync(string key)
        {
            if (await _storage.HasBlobAsync(_container, key))
            {
                return JsonConvert.DeserializeObject<TemplateModel>(
                    Encoding.UTF8.GetString(await(await _storage.GetAsync(_container, key)).ToBytesAsync()));
            }
            return null;
        }

        public async Task<ITemplateModel> GetByIdAsync(Guid templateId)
        {
            var key = templateId.ToString();
            return await GetByIdAsync(key);
        }


        public async Task Write(ITemplateModel template)
        {
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(template));
            template.TemplateId = template.TemplateId ?? Guid.NewGuid();
            await _storage.SaveBlobAsync(_container, template.TemplateId.Value.ToString(), data);
        }

        public async Task<bool> Remove(Guid existTemplateTemplateId)
        {
            try
            {
                await _storage.DelBlobAsync(_container, existTemplateTemplateId.ToString());
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}