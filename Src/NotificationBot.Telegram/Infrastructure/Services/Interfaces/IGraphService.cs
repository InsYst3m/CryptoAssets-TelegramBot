using NotificationBot.Telegram.Infrastructure.ViewModels;

namespace NotificationBot.Telegram.Infrastructure.Services.Interfaces
{
    public interface IGraphService
    {
        /// <summary>
        /// Gets the crypto asset from GraphQL service asynchronous.
        /// </summary>
        /// <param name="abbreviation">The abbreviation.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns <see cref="CryptoAssetViewModel"/> if exists.</returns>
        Task<CryptoAssetViewModel?> GetCryptoAssetAsync(string abbreviation, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the crypto assets asynchronous.
        /// </summary>
        /// <param name="abbreviations">The abbreviations.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an empty list if no crypto assets were found.</returns>
        Task<List<CryptoAssetViewModel>> GetCryptoAssetsAsync(string[] abbreviations, CancellationToken cancellationToken = default);
    }
}
