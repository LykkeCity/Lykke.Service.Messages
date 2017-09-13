using Lykke.Job.SMS.Core.Domain;
using Lykke.Service.SMS.Client;
using Lykke.Service.SMS.Client.AutorestClient.Models;
using Xunit;

namespace Lykke.Service.Messages.Tests
{
    public class ClientTest
    {
        [Fact]
        public void Test1()
        {
            var client = new SMSClient("http://13.93.41.3:1070", null);
            var result = client.SendSms(new SmsRequestModel {Message = "Test message", PhoneNumber = "+79312600505"}).Result;
            Assert.Equal(SmsPostRequestStatus.Ok, result.SmsPostRequestStatus);
        }
    }

}
