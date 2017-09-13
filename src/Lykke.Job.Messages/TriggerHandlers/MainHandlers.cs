using System.Threading.Tasks;
using Lykke.Job.SMS.Core.Services;
using Lykke.JobTriggers.Triggers.Attributes;

namespace Lykke.Job.SMS.TriggerHandlers
{
    // NOTE: This is the trigger handlers class example.
    // All triger's handlers are founded and added to container by JobTriggers infrastructure, 
    // when you call builder.AddTriggers() in Startup. Further, JobTriggers infrastructure manages handlers execution.
    public class MainHandlers
    {
        private readonly ISendSmsService _sendSmsService;
        private readonly IHealthService _healthService;

        // NOTE: The object is instantiated using DI container, so registered dependencies are injects well
        public MainHandlers(ISendSmsService sendSmsService, IHealthService healthService)
        {
            _sendSmsService = sendSmsService;
            _healthService = healthService;
        }

        [TimerTrigger("00:00:01")]
        public async Task TimeTriggeredHandler()
        {
            try
            {
                _healthService.TraceSendSmsStarted();

                await _sendSmsService.ProcessMessageAsync();

                _healthService.TraceSendSmsCompleted();
            }
            catch
            {
                _healthService.TraceSendSmsFailed();
            }
        }

       
    }
}