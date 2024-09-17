using CurrencyConverterDashboard.Models;
using ReactiveUI;
using System;

namespace CurrencyConverterDashboard.ViewModels
{
    public class CurrencyRateCardViewModel : ReactiveObject
    {
        private decimal _currentRate;
        private string _changeIndicator = string.Empty;
        private decimal _changePercentage;
        private decimal _oneDayChange;
        private decimal _oneWeekChange;
        private decimal _oneMonthChange;

        public Currency Currency { get; }

        public decimal CurrentRate
        {
            get => _currentRate;
            set => this.RaiseAndSetIfChanged(ref _currentRate, value);
        }

        public string ChangeIndicator
        {
            get => _changeIndicator;
            set => this.RaiseAndSetIfChanged(ref _changeIndicator, value);
        }

        public decimal ChangePercentage
        {
            get => _changePercentage;
            set => this.RaiseAndSetIfChanged(ref _changePercentage, value);
        }

        public decimal OneDayChange
        {
            get => _oneDayChange;
            set => this.RaiseAndSetIfChanged(ref _oneDayChange, value);
        }

        public decimal OneWeekChange
        {
            get => _oneWeekChange;
            set => this.RaiseAndSetIfChanged(ref _oneWeekChange, value);
        }

        public decimal OneMonthChange
        {
            get => _oneMonthChange;
            set => this.RaiseAndSetIfChanged(ref _oneMonthChange, value);
        }

        public CurrencyRateCardViewModel(Currency currency, decimal currentRate, decimal previousRate,
            decimal oneDayChange, decimal oneWeekChange, decimal oneMonthChange)
        {
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
            CurrentRate = currentRate;
            OneDayChange = oneDayChange;
            OneWeekChange = oneWeekChange;
            OneMonthChange = oneMonthChange;

            UpdateChangeIndicator(currentRate, previousRate);
        }

        private void UpdateChangeIndicator(decimal currentRate, decimal previousRate)
        {
            var change = currentRate - previousRate;
            ChangePercentage = previousRate != 0 ? (currentRate - previousRate) / previousRate : 0;
            ChangeIndicator = change >= 0 ? "▲" : "▼";
        }
    }
}