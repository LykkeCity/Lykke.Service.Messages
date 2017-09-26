namespace Lykke.Job.Messages.Contract.Sms
{
    public class SendSmsMerchantData<T>
    {
        public string PhoneNumber { get; set; }
        public T MessageData { get; set; }
        public string MerchantId { get; set; }
        public int MessageType { get; set; }
        public string TemplateType { get; set; }

        public SendSmsMerchantData<string> CloneWithMessage(string msg)
        {
            return new SendSmsMerchantData<string>
            {
                PhoneNumber = PhoneNumber,
                MessageData = msg,
                MerchantId = MerchantId,
                MessageType = MessageType,
                TemplateType = TemplateType
            };
        }
    }
}