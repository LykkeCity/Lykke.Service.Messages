using System;
using System.Threading.Tasks;
using AzureStorage.Queue;
using Lykke.Job.Messages.Contract.Sms;
using Lykke.Job.Messages.Core.Services;
using Lykke.Service.Messages.Core.Domain;


namespace Lykke.Job.Messages.QueueConsumers
{
    public class SmsQueueConsumer
    {
        private readonly IQueueReader _queueReader;
        private readonly ITemplateGenerator _templateGenerator;
        private readonly ISmsSender _smsSender;

        public SmsQueueConsumer(IQueueReader queueReader, ITemplateGenerator templateGenerator, ISmsSender smsSender)
        {
            _queueReader = queueReader;
            _templateGenerator = templateGenerator;
            _smsSender = smsSender;
            InitQueues();
        }

        private void InitQueues()
        {
            _queueReader.RegisterPreHandler(data =>
            {
                if (data == null)
                {
                    Console.WriteLine("Queue had unknown SMS send request");
                    return Task.FromResult(false);
                }
                return Task.FromResult(true);
            });

            _queueReader.RegisterHandler<SendSmsMerchantData<SmsConfirmationData>>(
                "SmsMerchantConfirmMessage", HandleSmsRequestAsync);
            _queueReader.RegisterHandler<SendSmsMerchantData<string>>("SimpleSmsMerchantMessage", HandleSimpleSmsRequestAsync);

            Console.WriteLine($"Registered:{_queueReader.GetComponentName()}");
        }

        private async Task HandleSimpleSmsRequestAsync(SendSmsMerchantData<string> request)
        {
            Console.WriteLine($"SMS: {request.MessageData}. Receiver: {request.PhoneNumber}, UTC: {DateTime.UtcNow}");
            await PrepareSmsToSend(request);
        }

        private async Task HandleSmsRequestAsync(SendSmsMerchantData<SmsConfirmationData> request)
        {
            Console.WriteLine($"SMS: Phone confirmation. Receiver: {request.PhoneNumber}, UTC: {DateTime.UtcNow}");

            var msgText = await _templateGenerator.GenerateAsync(request.MerchantId, (MessageType)request.MessageType, request.TemplateType, request.MessageData.ConfirmationCode);
            await PrepareSmsToSend(request.CloneWithMessage(msgText));
        }

        private async Task PrepareSmsToSend(SendSmsMerchantData<string> request)
        {
            await _smsSender.SendSms(request.PhoneNumber, request.MessageData);
        }

        public void Start()
        {
            _queueReader.Start();
            Console.WriteLine($"Started:{_queueReader.GetComponentName()}");
        }

       
    }
}