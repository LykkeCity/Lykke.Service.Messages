using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.SMS.Client.AutorestClient;
using Lykke.Service.SMS.Services;
using SmsRequestModel = Lykke.Service.SMS.Client.AutorestClient.Models.SmsRequestModel;

namespace Lykke.Service.Messages.Client
{
    public class SMSClient : ISMSClient, IDisposable
    {
        private readonly ILog _log;
        private readonly ISMSAPI _service;

        public SMSClient(string serviceUrl, ILog log)
        {
            _log = log;
            _service = new SMSAPI(new Uri(serviceUrl));
        }

        public async Task<SmsResponseModel> SendSms(SmsRequestModel sms)
        {
            try
            {
                var result = await _service.ApiSmsComtrollerPostWithHttpMessagesAsync(sms);
                return new SmsResponseModel();
                //return result.Response as SmsResponseModel;
            }
            catch (Exception e)
            {
                await _log.WriteFatalErrorAsync("Send Sms Serivce CLient", "Store request", string.Empty, e, DateTime.UtcNow);
                throw;
            }
           
        }

        public void Dispose()
        {
            //if (_service == null)
            //    return;
            //_service.Dispose();
            //_service = null;
        }
    }
}
