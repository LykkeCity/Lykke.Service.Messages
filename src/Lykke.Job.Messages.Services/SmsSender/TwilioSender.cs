using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Job.SMS.Core;
using Lykke.Job.SMS.Core.Domain;
using Lykke.Job.SMS.Core.Services;
using Lykke.Job.SMS.Services.Twilio;

namespace Lykke.Job.SMS.Services.SmsSender
{
    public class TwilioSender : ISmsSender
    {
        private readonly ILog _log;
        private readonly AppSettings.TwilioSettings _twilioSettings;
        private readonly TwilioRestClient _twilioRestClient;

        public TwilioSender(AppSettings.SMSSettings settings, ILog log)
        {
            _log = log;
            _twilioSettings = settings.Twilio;
            _twilioRestClient = new TwilioRestClient(_twilioSettings.AccountSid, _twilioSettings.AuthToken);
        }

        public string GetSenderNumber(string recipientNumber)
        {
            return recipientNumber.IsUSCanadaNumber() ? _twilioSettings.UsSender : _twilioSettings.SwissSender;
        }

        public async Task<SmsSenderResponse> ProcessSmsAsync(string phoneNumber, SmsMessage message)
        {
            var msg = await _twilioRestClient.SendMessage(message.From, phoneNumber, message.Text);

            if (!msg.Success)
                await _log.WriteWarningAsync("TwilioSender", "ProcessSmsAsync", phoneNumber, msg.ErrorMesssage);
            return new SmsSenderResponse
            {
                SmsSenderStatus = msg.Success ? SmsSenderStatus.Success : SmsSenderStatus.Error,
                Description = msg.Success ? string.Empty : msg.ErrorMesssage
            };
        }
    }
}
