using NotificationBot.Telegram.Helpers;
using NotificationBot.Telegram.Infrastructure.Commands;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;

using Telegram.Bot.Types;

namespace NotificationBot.Telegram.Infrastructure.Services
{
	/// <summary>
	/// Parses received telegram bot <see cref="Message"/>.
	/// </summary>
	public class MessageParser : IMessageParser
	{
		private readonly IGraphService _graphService;

		public MessageParser(IGraphService graphService)
		{
			_graphService = graphService ?? throw new ArgumentNullException(nameof(graphService));
		}

		public async Task<Command> ParseAsync(Message message)
		{
			string? messageText = message.Text;

			if (string.IsNullOrWhiteSpace(messageText) ||
				!messageText.StartsWith('/'))
			{
				return new UnknownCommand(message.Chat.Id);
			}

			string[] parsedCommand = messageText.Split(" ");

			string command = parsedCommand[0];
			string? argument = parsedCommand.Length > 1
				? parsedCommand[1]
				: null;

			switch (command)
			{
				case ConstantsHelper.Commands.START:
					return new StartCommand(message.Chat.Id);
				case ConstantsHelper.Commands.STOP:
					return new StopCommand(message.Chat.Id);

				case ConstantsHelper.Commands.GET_ASSET:

					string[] supportedAssets = await _graphService.GetSupportedCryptoAssetsAsync();

					if (!string.IsNullOrWhiteSpace(argument) &&
						supportedAssets.Contains(argument))
					{
						return new GetAssetCommand(message.Chat.Id, argument);
					}
					else
					{
						return new UnknownCommand(message.Chat.Id);
					}

				case ConstantsHelper.Commands.GET_ASSETS:
					return new GetAssetsCommand(message.Chat.Id);

				case ConstantsHelper.Commands.GET_FAVORITE_ASSETS:
					return new GetFavoriteAssetsCommand(message.Chat.Id);

				default:
					return new UnknownCommand(message.Chat.Id);
			};
		}
	}
}
