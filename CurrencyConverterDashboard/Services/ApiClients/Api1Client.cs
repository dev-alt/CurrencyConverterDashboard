using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyConverterDashboard.Models;

namespace CurrencyConverterDashboard.Services.ApiClients
{
    public class Api1Client : IApiClient
    {
        private readonly List<Currency> _availableCurrencies =
        [
            new Currency("USD", "US Dollar", "$"),
            new Currency("EUR", "Euro", "€"),
            new Currency("GBP", "British Pound", "£"),
            new Currency("JPY", "Japanese Yen", "¥")
        ];

        private readonly Dictionary<string, decimal> _exchangeRates = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
        {
            {"USDEUR", 0.84m},
            {"USDGBP", 0.72m},
            {"USDJPY", 109.48m},
            {"EURUSD", 1.19m},
            {"EURGBP", 0.86m},
            {"EURJPY", 130.64m},
            {"GBPUSD", 1.39m},
            {"GBPEUR", 1.16m},
            {"GBPJPY", 151.42m},
            {"JPYUSD", 0.0091m},
            {"JPYEUR", 0.0077m},
            {"JPYGBP", 0.0066m}
        };

        public Task<List<Currency>> GetAvailableCurrenciesAsync()
        {
            return Task.FromResult(_availableCurrencies);
        }

        public Task<ExchangeRate> GetExchangeRateAsync(Currency fromCurrency, Currency toCurrency)
        {
            string key = $"{fromCurrency.Code}{toCurrency.Code}";
            if (_exchangeRates.TryGetValue(key, out decimal rate))
            {
                var exchangeRate = new ExchangeRate(
                    fromCurrency,
                    toCurrency,
                    rate,
                    rate * 0.99m, // Simulating a previous rate
                    (rate - (rate * 0.99m)) / (rate * 0.99m), // Simulating one day change
                    (rate - (rate * 0.98m)) / (rate * 0.98m), // Simulating one week change
                    (rate - (rate * 0.97m)) / (rate * 0.97m), // Simulating one month change
                    DateTime.UtcNow,
                    "Api1"
                );
                return Task.FromResult(exchangeRate);
            }
            throw new KeyNotFoundException($"Exchange rate not found for {key}");
        }

        public Task<Dictionary<string, ExchangeRate>> GetAllExchangeRatesAsync(Currency baseCurrency)
        {
            var result = new Dictionary<string, ExchangeRate>();
            foreach (var currency in _availableCurrencies.Where(c => c.Code != baseCurrency.Code))
            {
                string key = $"{baseCurrency.Code}{currency.Code}";
                if (_exchangeRates.TryGetValue(key, out decimal rate))
                {
                    result[currency.Code] = new ExchangeRate(
                        baseCurrency,
                        currency,
                        rate,
                        rate * 0.99m, // Simulating a previous rate
                        (rate - (rate * 0.99m)) / (rate * 0.99m), // Simulating one day change
                        (rate - (rate * 0.98m)) / (rate * 0.98m), // Simulating one week change
                        (rate - (rate * 0.97m)) / (rate * 0.97m), // Simulating one month change
                        DateTime.UtcNow,
                        "Api1"
                    );
                }
            }
            return Task.FromResult(result);
        }
    }
}