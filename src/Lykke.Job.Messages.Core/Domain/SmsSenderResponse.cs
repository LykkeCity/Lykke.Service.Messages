using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Job.SMS.Core.Domain
{
    public class SmsSenderResponse
    {
        public SmsSenderStatus SmsSenderStatus { get; set; }
        public string Description { get; set; }
    }
}
