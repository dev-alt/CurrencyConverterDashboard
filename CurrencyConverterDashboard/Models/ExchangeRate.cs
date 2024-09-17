using System;

namespace CurrencyConverterDashboard.Models
{
    public class ExchangeRate(
        Currency fromCurrency,
        Currency toCurrency,
        decimal currentRate,
        decimal previousRate,
        decimal oneDayChange,
        decimal oneWeekChange,
        decimal oneMonthChange,
        DateTime timestamp,
        string source)
    {
        private Currency FromCurrency { get; } = fromCurrency ?? throw new ArgumentNullException(nameof(fromCurrency));
        public Currency ToCurrency { get; } = toCurrency ?? throw new ArgumentNullException(nameof(toCurrency));
        public decimal CurrentRate { get; } = currentRate > 0 ? currentRate : throw new ArgumentException("Rate must be greater than zero", nameof(currentRate));
        public decimal PreviousRate { get; } = previousRate > 0 ? previousRate : throw new ArgumentException("Previous rate must be greater than zero", nameof(previousRate));
        public decimal OneDayChange { get; } = oneDayChange;
        public decimal OneWeekChange { get; } = oneWeekChange;
        public decimal OneMonthChange { get; } = oneMonthChange;
        private DateTime Timestamp { get; } = timestamp;
        private string Source { get; } = source ?? throw new ArgumentNullException(nameof(source));

        public override string ToString()
        {
            return $"1 {FromCurrency.Code} = {CurrentRate} {ToCurrency.Code} ({Source} at {Timestamp})";
        }
    }
}