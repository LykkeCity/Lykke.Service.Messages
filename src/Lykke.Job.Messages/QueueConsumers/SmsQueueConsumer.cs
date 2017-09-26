using System;
using System.Threading.Tasks;
using AzureStorage.Queue;
using Common.Log;
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
        private readonly ILog _log;

        public SmsQueueConsumer(IQueueReader queueReader, ITemplateGenerator templateGenerator, ISmsSender smsSender, ILog log)
        {
            _queueReader = queueReader;
            _templateGenerator = templateGenerator;
            _smsSender = smsSender;
            _log = log;
            InitQueues();
        }

        private void InitQueues()
        {
            _queueReader.RegisterPreHandler(data =>
            {
                if (data == null)
                {
                    _log.WriteWarningAsync(
                            nameof(Messages),
                            nameof(SmsQueueConsumer),
                            nameof(InitQueues),
                            "Queue had unknown SMS send request")
                        .Wait();

                    return Task.FromResult(false);
                }
                return Task.FromResult(true);
            });

            _queueReader.RegisterHandler<SendSmsMerchantData<SmsConfirmationData>>(
                "SmsMerchantConfirmMessage", HandleSmsRequestAsync);
            _queueReader.RegisterHandler<SendSmsMerchantData<string>>("SimpleSmsMerchantMessage", HandleSimpleSmsRequestAsync);

            _log.WriteInfoAsync(
                nameof(Messages),
                nameof(SmsQueueConsumer),
                nameof(InitQueues),
                $"Registered:{_queueReader.GetComponentName()}").Wait();
        }

        private async Task HandleSimpleSmsRequestAsync(SendSmsMerchantData<string> request)
        {
            await _log.WriteInfoAsync(
                nameof(Messages),
                nameof(SmsQueueConsumer),
                nameof(InitQueues),
                $"SMS: {request.MessageData}. Receiver: {request.PhoneNumber}, UTC: {DateTime.UtcNow}");
            await PrepareSmsToSend(request);
        }

        private async Task HandleSmsRequestAsync(SendSmsMerchantData<SmsConfirmationData> request)
        {
            await _log.WriteInfoAsync(
                nameof(Messages),
                nameof(SmsQueueConsumer),
                nameof(InitQueues),
                $"SMS: Phone confirmation. Receiver: {request.PhoneNumber}, UTC: {DateTime.UtcNow}");

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
            _log.WriteInfoAsync(
                    nameof(Messages),
                    nameof(SmsQueueConsumer),
                    nameof(Start),
                    $"Started:{_queueReader.GetComponentName()}")
                .Wait();
        }

       
    }
}