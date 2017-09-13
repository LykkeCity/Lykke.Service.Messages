using Lykke.Service.Messages.Client;
using Xunit;

namespace Lykke.Service.Messages.Tests
{
    public class ClientTest
    {
        [Fact]
        public void Test1()
        {
            var client = new TemplateMessagesClient("http://52.233.198.75:2340", null);
            var result = client.GetTemplates().Result;
            Assert.NotEmpty(result);;
        }
    }

}
