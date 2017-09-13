using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Job.SMS.Core;
using Lykke.Job.SMS.Core.Domain;
using Lykke.Job.SMS.Core.Services;
using Lykke.Job.SMS.Services.Nexmo;

namespace Lykke.Job.SMS.Services.SmsSender
{
    //https://docs.nexmo.com/messaging/sms-api/api-reference#keys

    public class NexmoSender : ISmsSender
    {
        private readonly AppSettings.NexmoSettings _settings;
        private readonly ILog _log;

        private const string NexmoSendSmsUrlFormat = "https://rest.nexmo.com/sms/json?api_key={0}&api_secret={1}&from={2}&to={3}&text={4}";

        public NexmoSender(AppSettings.SMSSettings settings, ILog log)
        {
            _settings = settings.Nexmo;
            _log = log;
        }

        public string GetSenderNumber(string recipientNumber)
        {
            return recipientNumber.IsUSCanadaNumber() ? _settings.UsCanadaSender : _settings.DefaultSender;
        }

        public async Task<SmsSenderResponse> ProcessSmsAsync(string phoneNumber, SmsMessage message)
        {
            var urlEncodedText = message.Text.EncodeUrl();
            var url = string.Format(NexmoSendSmsUrlFormat, _settings.NexmoAppKey, _settings.NexmoAppSecret, message.From, phoneNumber, urlEncodedText);
            var client = new HttpClient();
            var response = await client.GetAsync(url);

            HttpContent responseContent = response.Content;
            NexmoResponse responseObj = null;
            string responseString;

            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                responseString = await reader.ReadToEndAsync();
                responseObj = responseString.DeserializeJson<NexmoResponse>();
            }

            if (responseObj != null)
            {
                foreach (var msg in responseObj.Messages)
                {
                    if (msg.Status != NexmoStatusCode.Success)
                    {
                        await _log.WriteWarningAsync("NexmoSMS", "ProcessSms", responseString, "SMS was not sent", DateTime.UtcNow);
                        return new SmsSenderResponse
                        {
                            SmsSenderStatus = SmsSenderStatus.Error,
                            Description = msg.ErrorText
                        };
                    }
                }

                return new SmsSenderResponse
                {
                    SmsSenderStatus = SmsSenderStatus.Success
                };

            }

            return new SmsSenderResponse
            {
                SmsSenderStatus = SmsSenderStatus.Error,
                Description = "Network issue"
            };
        }
    }
}
