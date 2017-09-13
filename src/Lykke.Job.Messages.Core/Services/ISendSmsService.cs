using System.Threading.Tasks;

namespace Lykke.Job.SMS.Core.Services
{
    // NOTE: This is job service interface example
    public interface ISendSmsService
    {
        Task ProcessMessageAsync();
    }
}