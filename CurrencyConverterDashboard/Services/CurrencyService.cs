using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyConverterDashboard.Models;
using CurrencyConverterDashboard.Services.ApiClients;

namespace CurrencyConverterDashboard.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IEnumerable<IApiClient> _apiClients;

        public CurrencyService(IEnumerable<IApiClient> apiClients)
        {
            _apiClients = apiClients ?? throw new ArgumentNullException(nameof(apiClients));
        }

        public event EventHandler<Exception> ErrorOccurred = null!;

        public async Task<List<Currency>> GetAvailableCurrenciesAsync()
        {
            var allCurrencies = new HashSet<Currency>();

            foreach (var client in _apiClients)
            {
                try
                {
                    var currencies = await client.GetAvailableCurrenciesAsync();
                    foreach (var currency in currencies)
                    {
                        allCurrencies.Add(currency);
                    }
                }
                catch (Exception ex)
                {
                    ErrorOccurred?.Invoke(this, ex);
                }
            }

            if (!allCurrencies.Any())
                throw new InvalidOperationException("Unable to fetch currencies from any API.");

            return allCurrencies.ToList();
        }

        public async Task<ExchangeRate> GetExchangeRateAsync(Currency fromCurrency, Currency toCurrency)
        {
            var rates = new List<ExchangeRate>();

            foreach (var client in _apiClients)
            {
                try
                {
                    var rate = await client.GetExchangeRateAsync(fromCurrency, toCurrency);
                    rates.Add(rate);
                }
                catch (Exception ex)
                {
                    ErrorOccurred?.Invoke(this, ex);
                }
            }

            if (!rates.Any())
                throw new InvalidOperationException("Unable to fetch exchange rate from any API.");

            var averageCurrentRate = rates.Average(r => r.CurrentRate);
            var averagePreviousRate = rates.Average(r => r.PreviousRate);
            var averageOneDayChange = rates.Average(r => r.OneDayChange);
            var averageOneWeekChange = rates.Average(r => r.OneWeekChange);
            var averageOneMonthChange = rates.Average(r => r.OneMonthChange);

            return new ExchangeRate(
                fromCurrency,
                toCurrency,
                averageCurrentRate,
                averagePreviousRate,
                averageOneDayChange,
                averageOneWeekChange,
                averageOneMonthChange,
                DateTime.UtcNow,
                "Aggregated"
            );
        }

        public async Task<Dictionary<string, ExchangeRate>> GetAllExchangeRatesAsync(Currency baseCurrency)
        {
            var allRates = new Dictionary<string, List<ExchangeRate>>();

            foreach (var client in _apiClients)
            {
                try
                {
                    var rates = await client.GetAllExchangeRatesAsync(baseCurrency);
                    foreach (var (currencyCode, rate) in rates)
                    {
                        if (!allRates.ContainsKey(currencyCode))
                        {
                            allRates[currencyCode] = new List<ExchangeRate>();
                        }
                        allRates[currencyCode].Add(rate);
                    }
                }
                catch (Exception ex)
                {
                    ErrorOccurred?.Invoke(this, ex);
                }
            }

            return allRates.ToDictionary(
                kvp => kvp.Key,
                kvp => new ExchangeRate(
                    baseCurrency,
                    new Currency(kvp.Key, kvp.Key, kvp.Key), // Simplified, you might want to use a proper currency lookup
                    kvp.Value.Average(r => r.CurrentRate),
                    kvp.Value.Average(r => r.PreviousRate),
                    kvp.Value.Average(r => r.OneDayChange),
                    kvp.Value.Average(r => r.OneWeekChange),
                    kvp.Value.Average(r => r.OneMonthChange),
                    DateTime.UtcNow,
                    "Aggregated"
                )
            );
        }
    }
}