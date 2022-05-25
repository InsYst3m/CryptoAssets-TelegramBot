namespace NotificationBot.Telegram.Infrastructure.ViewModels
{
    public class CryptoAssetViewModel
    {
        public string Name { get; set; }
        public string Abbreviation { get; }
        public long Rank { get; }
        public decimal CapitalizationUsd { get; }
        public decimal CurrentPriceUsd { get; set; }
        public decimal AllTimeHighPriceUsd { get; }
        public decimal AllTimeLowPriceUsd { get; }
        public decimal HighTwentyFourHoursUsd { get; }
        public decimal LowTwentyFourHoursUsd { get; }
        public string AllTimeHighChangePercentage { get; }
        public string AllTimeLowChangePercentage { get; }

        public CryptoAssetViewModel(
            string name,
            string abbreviation,
            long rank,
            decimal capitalization,
            decimal currentPriceUsd,
            decimal allTimeHighPriceUsd,
            decimal allTimeLowPriceUsd,
            decimal highTwentyFourHoursUsd,
            decimal lowTwentyFourHoursUsd,
            string allTimeHighChangePercentage,
            string allTimeLowChangePercentage)
        {
            Name = name;
            Abbreviation = abbreviation;
            Rank = rank;
            CapitalizationUsd = capitalization;
            CurrentPriceUsd = currentPriceUsd;
            AllTimeHighPriceUsd = allTimeHighPriceUsd;
            AllTimeLowPriceUsd = allTimeLowPriceUsd;
            HighTwentyFourHoursUsd = highTwentyFourHoursUsd;
            LowTwentyFourHoursUsd = lowTwentyFourHoursUsd;
            AllTimeHighChangePercentage = allTimeHighChangePercentage;
            AllTimeLowChangePercentage = allTimeLowChangePercentage;
        }
    }
}
