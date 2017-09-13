using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Job.SMS.Core.Domain;

namespace Lykke.Job.SMS.Core.Services
{
    public interface ISmsSender
    {
        string GetSenderNumber(string recipientNumber);
        Task<SmsSenderResponse> ProcessSmsAsync(string phoneNumber, SmsMessage message);
    }
}
