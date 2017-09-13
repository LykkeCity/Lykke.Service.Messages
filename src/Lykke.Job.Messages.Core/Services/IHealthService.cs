using System.Collections.Generic;
using Lykke.Job.SMS.Core.Domain.Health;

namespace Lykke.Job.SMS.Core.Services
{
    // NOTE: See https://lykkex.atlassian.net/wiki/spaces/LKEWALLET/pages/35755585/Add+your+app+to+Monitoring
    public interface IHealthService
    {
        string GetHealthViolationMessage();
        IEnumerable<HealthIssue> GetHealthIssues();


        void TraceSendSmsStarted();
        void TraceSendSmsCompleted();
        void TraceSendSmsFailed();
    }
}