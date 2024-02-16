using Azure.Messaging.ServiceBus;
using Mongo.EmailApi.Model.Dto;
using Mongo.EmailApi.NewFolder;
using Newtonsoft.Json;
using System.Text;

namespace Mongo.EmailApi.Messaging
{
    public class AzureServiceBusConsumer: IAzureServiceBusConsumer
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _emailCartQueue;
        private readonly ServiceBusProcessor _emailCartProcessor;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
            _serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            _emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCart");


            var client = new ServiceBusClient(_serviceBusConnectionString);
            _emailCartProcessor = client.CreateProcessor(_emailCartQueue);
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs arg)
        {
            // recive message here
            var message = arg.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body);
            try
            {
                await _emailService.EmailCartAndLog(objMessage);  // HANDLE EMAIL AND SAVE IN DB
                await arg.CompleteMessageAsync(arg.Message);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        
    }
}
