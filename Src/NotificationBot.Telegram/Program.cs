using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationBot.Telegram.Configuration;
using NotificationBot.Telegram.Infrastructure;
using NotificationBot.Telegram.Infrastructure.HostedServices;
using NotificationBot.Telegram.Infrastructure.Services;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(configurationBuilder =>
    {
        configurationBuilder
            .AddJsonFile("appsettings.json", false, true)
            .Build();
    })
    .ConfigureServices((hostContext, services) => 
    {
        services.Configure<BotSettings>(hostContext.Configuration.GetSection(nameof(BotSettings)),
            options => options.BindNonPublicProperties = true);

        services.AddSingleton<ITelegramBotClientFactory, TelegramBotClientFactory>();
        services.AddSingleton<IBotService, BotService>();

        services.AddHostedService<TelegramBotHostedService>();
    })
    .Build();

await host.RunAsync();