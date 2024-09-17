using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Reactive.Linq;
using CurrencyConverterDashboard.Models;
using CurrencyConverterDashboard.Services;
using ReactiveUI;

namespace CurrencyConverterDashboard.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly ICurrencyService _currencyService;
        private Currency? _fromCurrency;
        private Currency? _toCurrency;
        private decimal _fromAmount;
        private decimal _toAmount;
        private bool _isLoading;
        private string? _errorMessage;
        private Currency? _selectedBaseCurrency;
        private ObservableCollection<CurrencyRateCardViewModel> _topCurrencies;

        public MainWindowViewModel(ICurrencyService currencyService)
        {
            _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
            Currencies = new ObservableCollection<Currency>();
            _topCurrencies = new ObservableCollection<CurrencyRateCardViewModel>();
            
            ConvertCommand = ReactiveCommand.CreateFromTask(ConvertAsync, this.WhenAnyValue(x => x.IsLoading, isLoading => !isLoading));
            LoadCurrenciesCommand = ReactiveCommand.CreateFromTask(LoadCurrenciesAsync);
            UpdateDashboardCommand = ReactiveCommand.CreateFromTask(UpdateDashboardAsync);

            _currencyService.ErrorOccurred += OnErrorOccurred!;

            // Load currencies when the ViewModel is created
            LoadCurrenciesCommand.Execute().Subscribe(_ => { }, ex => ErrorMessage = $"Failed to load currencies: {ex.Message}");

            this.WhenAnyValue(x => x.SelectedBaseCurrency)
                .Where(x => x != null)
                .SelectMany(_ => UpdateDashboardCommand.Execute())
                .Subscribe();
        }
        
        public ObservableCollection<Currency> Currencies { get; }

        public Currency? FromCurrency
        {
            get => _fromCurrency;
            set => this.RaiseAndSetIfChanged(ref _fromCurrency, value);
        }

        public Currency? ToCurrency
        {
            get => _toCurrency;
            set => this.RaiseAndSetIfChanged(ref _toCurrency, value);
        }

        public decimal FromAmount
        {
            get => _fromAmount;
            set => this.RaiseAndSetIfChanged(ref _fromAmount, value);
        }

        public decimal ToAmount
        {
            get => _toAmount;
            set => this.RaiseAndSetIfChanged(ref _toAmount, value);
        }

        private bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }

        public string? ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public Currency? SelectedBaseCurrency
        {
            get => _selectedBaseCurrency;
            set => this.RaiseAndSetIfChanged(ref _selectedBaseCurrency, value);
        }

        public ObservableCollection<CurrencyRateCardViewModel> TopCurrencies
        {
            get => _topCurrencies;
            set => this.RaiseAndSetIfChanged(ref _topCurrencies, value);
        }

        public ReactiveCommand<Unit, Unit> ConvertCommand { get; }
        private ReactiveCommand<Unit, Unit> LoadCurrenciesCommand { get; }
        private ReactiveCommand<Unit, Unit> UpdateDashboardCommand { get; }

        private async Task LoadCurrenciesAsync()
        {
            IsLoading = true;
            ErrorMessage = null;
            try
            {
                var currencies = await _currencyService.GetAvailableCurrenciesAsync();
                Currencies.Clear();
                foreach (var currency in currencies)
                {
                    Currencies.Add(currency);
                }
                FromCurrency = Currencies.FirstOrDefault(c => c.Code == "USD");
                ToCurrency = Currencies.FirstOrDefault(c => c.Code == "EUR");
                SelectedBaseCurrency = FromCurrency;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to load currencies: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ConvertAsync()
        {
            if (FromCurrency == null || ToCurrency == null)
            {
                ErrorMessage = "Please select both currencies";
                return;
            }

            IsLoading = true;
            ErrorMessage = null;
            try
            {
                var exchangeRate = await _currencyService.GetExchangeRateAsync(FromCurrency, ToCurrency);
                ToAmount = FromAmount * exchangeRate.CurrentRate;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Conversion failed: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task UpdateDashboardAsync()
        {
            if (SelectedBaseCurrency == null)
                return;

            IsLoading = true;
            ErrorMessage = null;
            try
            {
                var allRates = await _currencyService.GetAllExchangeRatesAsync(SelectedBaseCurrency);
                var topCurrencies = allRates.Values
                    .OrderBy(r => r.ToCurrency.Code)
                    .Take(5)
                    .Select(r => new CurrencyRateCardViewModel(
                        r.ToCurrency,
                        r.CurrentRate,
                        r.PreviousRate,
                        r.OneDayChange,
                        r.OneWeekChange,
                        r.OneMonthChange))
                    .ToList();

                TopCurrencies.Clear();
                foreach (var rate in topCurrencies)
                {
                    TopCurrencies.Add(rate);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to update dashboard: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    
        private void OnErrorOccurred(object sender, Exception ex)
        {
            ErrorMessage = $"An error occurred: {ex.Message}";
        }
    }
}