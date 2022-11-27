using Microsoft.Extensions.Caching.Memory;

using NotificationBot.Telegram.Infrastructure.Commands.Interfaces;
using NotificationBot.Telegram.Infrastructure.Commands.Processors;
using NotificationBot.Telegram.Infrastructure.Generators;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;

namespace NotificationBot.Telegram.Infrastructure.Commands.Factory
{
	public class BotCommandFactory : IBotCommandProcessorFactory
	{
		private const string PERIODIC_NOTIFICATION_COMMAND_CACHE_KEY = "PeriodicNotificationCommandCache";

		private readonly IServiceProvider _serviceProvider;
		private readonly IMemoryCache _memoryCache;

		public BotCommandFactory(
			IServiceProvider serviceProvider,
			IMemoryCache memoryCache)
		{
			_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			_memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
		}

		public IBotCommandProcessor Create(Command command)
		{
			IBotCommandProcessor botCommand = command switch
			{
				//StartCommand => new StartCommand(
				//	command,
				//	_serviceProvider.GetRequiredService<INotificationService>()),
				//CommandTypes.Stop => new StopCommand(
				//	command,
				//	_serviceProvider.GetRequiredService<INotificationService>()),
				GetAssetCommand getAssetCommand => new GetAssetCommandProcessor(
					getAssetCommand,
					_serviceProvider.GetRequiredService<IGraphService>(),
					_serviceProvider.GetRequiredService<IMessageGenerator>(),
					_serviceProvider.GetRequiredService<INotificationService>()),
				_ => new NotSupportedCommandProcessor(
					command,
					_serviceProvider.GetRequiredService<INotificationService>())
			};

			return botCommand;

			//IBotCommand? botCommand = commandMessage.Command switch
			//{
			//	"/favourites" or
			//	"/favorites" => new FavoriteCryptoAssetsCommand(
			//		commandMessage,
			//		_serviceProvider.GetRequiredService<IGraphService>(),
			//		_serviceProvider.GetRequiredService<IMessageGenerator>(),
			//		_serviceProvider.GetRequiredService<INotificationService>()),

			//	"/start" => new BotStartCommand(
			//		commandMessage,
			//		_serviceProvider.GetRequiredService<INotificationService>()),

			//	"/stop" => new BotStopCommand(
			//		commandMessage,
			//		_serviceProvider.GetRequiredService<INotificationService>()),

			//	"/portfolio" => new PortfolioCommand(
			//		commandMessage,
			//		_serviceProvider.GetRequiredService<INotificationService>()),

			//	"/get" => new CryptoAssetInfoCommand(
			//			commandMessage,
			//			_serviceProvider.GetRequiredService<IGraphService>(),
			//			_serviceProvider.GetRequiredService<IMessageGenerator>(),
			//			_serviceProvider.GetRequiredService<INotificationService>()),

			//	"/createportfolio" => new PortfolioCreateCommand(
			//		commandMessage,
			//		_serviceProvider.GetRequiredService<INotificationService>()),

			//	_ => new NotSupportedCommand(
			//		commandMessage,
			//		_serviceProvider.GetRequiredService<INotificationService>())
			//};
		}

		//public IBotCommandProcessor GetOrCreatePeriodicNotificationCommand()
		//{
		//	return _memoryCache.GetOrCreate(
		//		PERIODIC_NOTIFICATION_COMMAND_CACHE_KEY,
		//		cacheEntry =>
		//		{
		//			cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(360);

		//			return new PeriodicNotificationCommand(
		//				_serviceProvider.GetRequiredService<IGraphService>(),
		//				_serviceProvider.GetRequiredService<IMessageGenerator>(),
		//				_serviceProvider.GetRequiredService<INotificationService>());
		//		});
		//}
	}
}
