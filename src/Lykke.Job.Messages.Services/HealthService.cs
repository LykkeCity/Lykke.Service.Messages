using System;
using System.Collections.Generic;
using Lykke.Job.SMS.Core.Domain.Health;
using Lykke.Job.SMS.Core.Services;

namespace Lykke.Job.SMS.Services
{
    // NOTE: See https://lykkex.atlassian.net/wiki/spaces/LKEWALLET/pages/35755585/Add+your+app+to+Monitoring
    public class HealthService : IHealthService
    {
        // NOTE: These are example properties
        private DateTime LastSendSmsStartedMoment { get; set; }
        private TimeSpan LastSendSmsDuration { get; set; }
        private TimeSpan MaxHealthySendSmsDuration { get; }
        private bool WasLastSendSmsFailed { get; set; }
        private bool WasLastSendSmsCompleted { get; set; }
        private bool WasClientsSendSmsEverStarted { get; set; }

        // NOTE: When you change parameters, don't forget to look in to JobModule

        public HealthService(TimeSpan maxHealthySendSmsDuration)
        {
            MaxHealthySendSmsDuration = maxHealthySendSmsDuration;
        }

        // NOTE: This method could stay in the real job, but will be modified
        public string GetHealthViolationMessage()
        {
            // TODO: Check gathered health statistics, and return appropriate health violation message, or NULL if job hasn't critical errors
            return null;
        }

        public IEnumerable<HealthIssue> GetHealthIssues()
        {
            var issues = new HealthIssuesCollection();

            if (WasLastSendSmsFailed)
            {
                issues.Add("SMSSendSmsProcessing", "Last SendSms was failed");
            }

            if (!WasLastSendSmsCompleted && !WasLastSendSmsFailed && !WasClientsSendSmsEverStarted)
            {
                issues.Add("SMSSendSmsProcessingNotStartedYet", "Waiting for first SendSms execution started");
            }

            if (!WasLastSendSmsCompleted && !WasLastSendSmsFailed && WasClientsSendSmsEverStarted)
            {
                issues.Add("SMSSendSmsProcessingNotCompletedYet", $"Waiting {DateTime.UtcNow - LastSendSmsStartedMoment} for first SendSms execution completed");
            }

            if (LastSendSmsDuration > MaxHealthySendSmsDuration)
            {
                issues.Add("SMSSendSmsProcessingLastedForToLong", $"Last SendSms was lasted for {LastSendSmsDuration}, which is too long");
            }

            return issues;
        }


        // NOTE: These are example methods
        public void TraceSendSmsStarted()
        {
            LastSendSmsStartedMoment = DateTime.UtcNow;
            WasClientsSendSmsEverStarted = true;
        }

        public void TraceSendSmsCompleted()
        {
            LastSendSmsDuration = DateTime.UtcNow - LastSendSmsStartedMoment;
            WasLastSendSmsCompleted = true;
            WasLastSendSmsFailed = false;
        }

        public void TraceSendSmsFailed()
        {
            WasLastSendSmsCompleted = false;
            WasLastSendSmsFailed = true;
        }
    }
}