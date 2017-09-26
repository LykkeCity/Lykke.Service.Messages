using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Job.Messages.Core.Services;
using Lykke.Service.SMS.Client;
using Lykke.Service.SMS.Client.AutorestClient.Models;

namespace Lykke.Job.Messages.Services
{
    public class SmsSender : ISmsSender
    {
        private readonly ISMSClient _smsClient;

        public SmsSender(ISMSClient smsClient)
        {
            _smsClient = smsClient;
        }
        public async Task SendSms(string phoneNumber, string message)
        {
            await _smsClient.SendSms(new SmsRequestModel(phoneNumber, message));
        }
    }
}
