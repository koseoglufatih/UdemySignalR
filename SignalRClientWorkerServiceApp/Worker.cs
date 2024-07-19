using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRClientWorkerServiceApp
{
    public class Worker(ILogger<Worker> logger, IConfiguration configuration) : BackgroundService
    {
        //    private readonly ILogger<Worker> _logger;
        //    private readonly IConfiguration _configuration;

        //    public Worker(ILogger<Worker> logger, IConfiguration configuration)
        //    {
        //        _logger = logger;
        //        _configuration = configuration;
        //    }

        private HubConnection? _connection;

        public override Task StartAsync(CancellationToken cancellationToken)
        {


            _connection = new HubConnectionBuilder().WithUrl(configuration.GetSection("SignalR")["Hub"]!).Build();

            _connection?.StartAsync().ContinueWith((result) =>
            {
                logger.LogInformation(result.IsCompletedSuccessfully ? "Connected" : "Connection failed");
            });


            return base.StartAsync(cancellationToken);
        }



        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _connection!.StopAsync(cancellationToken);
            await _connection!.DisposeAsync();

            await base.StopAsync(cancellationToken);
        }




        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {


            _connection!.On<Product>("ReceiveMessageForAllClient", (product) =>
            {
                logger.LogInformation($"Received message: {product.Id}-{product.Name}-{product.Price}");

                return Task.CompletedTask;
            });

    




        }
    }
}
