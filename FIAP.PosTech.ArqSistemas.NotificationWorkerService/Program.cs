using FIAP.PosTech.ArqSistemas.NotificationWS.Services;
using FIAP.PosTech.ArqSistemas.NotificationWS.Workers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<KafkaConsumerWorker>();
builder.Services.AddSingleton<IEmailService, EmailService>();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var host = builder.Build();
host.Run();
