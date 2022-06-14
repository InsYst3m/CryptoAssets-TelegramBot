namespace NotifiicationBot.Domain.Entities
{
    public class Portfolio
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public long UserId { get; set; }
        public User User { get; set; } = null!;
        public IList<CryptoTransaction> CryptoTransactions { get; set; } = new List<CryptoTransaction>();
    }
}
