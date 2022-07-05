namespace NotifiicationBot.Domain.Entities
{
    /// <summary>
    /// Represents crypto assets portfolio.
    /// </summary>
    public class Portfolio
    {
        public Portfolio(string name, long userId)
        {
            Name = name;
            UserId = userId;
        }

        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public long UserId { get; set; }
        public User User { get; set; } = null!;
        public IList<CryptoTransaction> CryptoTransactions { get; set; } = new List<CryptoTransaction>();
    }
}
