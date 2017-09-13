
using System.Threading.Tasks;
using Lykke.Service.SMS.Services;
using SmsRequestModel = Lykke.Service.SMS.Client.AutorestClient.Models.SmsRequestModel;

namespace Lykke.Service.Messages.Client
{
    public interface ISMSClient
    {
        Task<SmsResponseModel> SendSms(SmsRequestModel sms);
    }
}