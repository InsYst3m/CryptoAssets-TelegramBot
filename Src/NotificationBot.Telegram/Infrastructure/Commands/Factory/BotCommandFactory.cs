using Microsoft.Extensions.Caching.Memory;

using NotificationBot.Telegram.Infrastructure.Commands.Interfaces;
using NotificationBot.Telegram.Infrastructure.Commands.Portfolio;
using NotificationBot.Telegram.Infrastructure.Generators;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Models;

namespace NotificationBot.Telegram.Infrastructure.Commands.Factory
{
	public class BotCommandFactory : IBotCommandFactory
	{
		private const string PERIODIC_NOTIFICATION_COMMAND_CACHE_KEY = "PeriodicNotificationCommandCache";

		private readonly IServiceProvider _serviceProvider;
		private readonly IMemoryCache _memoryCache;

		public BotCommandFactory(
			IServiceProvider serviceProvider,
			IMemoryCache memoryCache)
		{
			ArgumentNullException.ThrowIfNull(serviceProvider);
			ArgumentNullException.ThrowIfNull(memoryCache);

			_serviceProvider = serviceProvider;
			_memoryCache = memoryCache;
		}

		public async Task<IBotCommand?> GetOrCreateAsync(CommandMessage commandMessage)
		{
			IGraphService graphService = _serviceProvider.GetRequiredService<IGraphService>();
			string[] supportedCryptoAssets = await graphService.GetSupportedCryptoAssetsAsync();

			if (string.IsNullOrWhiteSpace(commandMessage.Command))
			{
				return null;
			}

			IBotCommand? botCommand = commandMessage.Command switch
			{
				"/favourites" or
				"/favorites" => new FavoriteCryptoAssetsCommand(
					commandMessage,
					_serviceProvider.GetRequiredService<IGraphService>(),
					_serviceProvider.GetRequiredService<IMessageGenerator>(),
					_serviceProvider.GetRequiredService<INotificationService>()),

				"/start" => new BotStartCommand(
					commandMessage,
					_serviceProvider.GetRequiredService<INotificationService>()),

				"/stop" => new BotStopCommand(
					commandMessage,
					_serviceProvider.GetRequiredService<INotificationService>()),

				"/portfolio" => new PortfolioCommand(
					commandMessage,
					_serviceProvider.GetRequiredService<INotificationService>()),

				"/get" => new CryptoAssetInfoCommand(
						commandMessage,
						_serviceProvider.GetRequiredService<IGraphService>(),
						_serviceProvider.GetRequiredService<IMessageGenerator>(),
						_serviceProvider.GetRequiredService<INotificationService>()),

				"/createportfolio" => new PortfolioCreateCommand(
					commandMessage,
					_serviceProvider.GetRequiredService<INotificationService>()),

				_ => new NotSupportedCommand(
					commandMessage,
					_serviceProvider.GetRequiredService<INotificationService>())
			};

			return botCommand;
		}

		public IBotCommand GetOrCreatePeriodicNotificationCommand()
		{
			return _memoryCache.GetOrCreate(
				PERIODIC_NOTIFICATION_COMMAND_CACHE_KEY,
				cacheEntry =>
				{
					cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(360);

					return new PeriodicNotificationCommand(
						_serviceProvider.GetRequiredService<IGraphService>(),
						_serviceProvider.GetRequiredService<IMessageGenerator>(),
						_serviceProvider.GetRequiredService<INotificationService>());
				});
		}
	}
}
