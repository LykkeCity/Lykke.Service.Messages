using System.Threading.Tasks;

namespace Lykke.Job.Messages.Core.Services
{
    public interface ISmsSender
    {
        Task SendSms(string phoneNumber, string message);
    }
}
