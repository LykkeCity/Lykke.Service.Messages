using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.AzureRepositories;
using Lykke.Core;
using Lykke.Job.SMS.Core.Domain;
using Lykke.Job.SMS.Core.Services;
using Lykke.Job.SMS.Services.SmsSender;

namespace Lykke.Job.SMS.Services
{
    // NOTE: This is job service class example
    public class SendSmsService : ISendSmsService
    {
        private readonly ISmsServiceRepository _smsServiceRepository;
        private readonly ISmsSender _nexmoSender;
        private readonly ISmsSender _twilioSender;
        private readonly ILog _log;
        public SendSmsService(ISmsServiceRepository smsServiceRepository, NexmoSender nexmoSender, TwilioSender twilioSender, ILog log)
        {
            _smsServiceRepository = smsServiceRepository;
            _nexmoSender = nexmoSender;
            _twilioSender = twilioSender;
            _log = log;
        }
        public async Task ProcessMessageAsync()
        {
            var messageToSend = await _smsServiceRepository.GetSmsRequestsByStatusAsync(SmsServiceStatus.ReadyToSend);
            foreach (var message in messageToSend)
            {
                var sender = GetSender(message);
                Console.WriteLine($"SMS: {message.Message}. Receiver: {message.PhoneNumer}, UTC: {DateTime.UtcNow}");
                var result = await sender.ProcessSmsAsync(message.PhoneNumer,
                    SmsMessage.Create(sender.GetSenderNumber(message.PhoneNumer), message.Message));
                message.ETag = "*";
                if (!await _smsServiceRepository.DeleteSmsRequestAsync(message))
                {
                    await _log.WriteErrorAsync("SendSmsService", "Deleting message model", string.Empty, null,
                        DateTime.UtcNow);
                    continue;
                }

                if (result.SmsSenderStatus == SmsSenderStatus.Success)
                {
                    message.SmsServiceStatus = (int)SmsServiceStatus.Sent;

                }
                else
                {
                    if (message.Atempt == 0)
                    {
                        message.Atempt = 1;
                        message.PhoneOperator = message.PhoneOperator == (int) PhoneOperator.Nexmo
                            ? (int) PhoneOperator.Twilio
                            : (int) PhoneOperator.Nexmo;
                        message.ErrorDescription = result.Description;
                    }
                    else
                    {
                        message.SmsServiceStatus = (int)SmsServiceStatus.GotError;
                        message.ErrorDescription = result.Description;
                    }
                    
                }

                if (!await _smsServiceRepository.SaveSmsRequestAsync(message))
                {
                    await _log.WriteErrorAsync("SendSmsService", "Saving message model", string.Empty, null,
                        DateTime.UtcNow);
                }

            }
        }

        

        private ISmsSender GetSender(ISmsEntity message)
        {
            return message.PhoneOperator == (int)PhoneOperator.Nexmo ? _nexmoSender : _twilioSender;
        }
    }
}