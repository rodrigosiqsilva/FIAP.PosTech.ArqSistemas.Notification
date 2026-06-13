using FIAP.PosTech.ArqSistemas.NotificationWorkerService;
using FIAP.PosTech.ArqSistemas.NotificationWS.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<IEmailService, EmailService>();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var host = builder.Build();
host.Run();
