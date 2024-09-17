using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyConverterDashboard.Models;

namespace CurrencyConverterDashboard.Services.ApiClients
{
    public class Api2Client : IApiClient
    {
        private readonly List<Currency> _availableCurrencies =
        [
            new Currency("USD", "US Dollar", "$"),
            new Currency("EUR", "Euro", "€"),
            new Currency("GBP", "British Pound", "£"),
            new Currency("JPY", "Japanese Yen", "¥"),
            new Currency("CAD", "Canadian Dollar", "C$")
        ];

        private readonly Dictionary<string, decimal> _exchangeRates = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
        {
            {"USDEUR", 0.85m},
            {"USDGBP", 0.73m},
            {"USDJPY", 110.0m},
            {"USDCAD", 1.25m},
            {"EURUSD", 1.18m},
            {"EURGBP", 0.86m},
            {"EURJPY", 129.41m},
            {"EURCAD", 1.47m},
            {"GBPUSD", 1.37m},
            {"GBPEUR", 1.16m},
            {"GBPJPY", 150.68m},
            {"GBPCAD", 1.71m},
            {"JPYUSD", 0.0091m},
            {"JPYEUR", 0.0077m},
            {"JPYGBP", 0.0066m},
            {"JPYCAD", 0.011m},
            {"CADUSD", 0.80m},
            {"CADEUR", 0.68m},
            {"CADGBP", 0.58m},
            {"CADJPY", 88.0m}
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
                    rate * 0.995m, // Simulating a previous rate
                    (rate - (rate * 0.995m)) / (rate * 0.995m), // Simulating one day change
                    (rate - (rate * 0.985m)) / (rate * 0.985m), // Simulating one week change
                    (rate - (rate * 0.975m)) / (rate * 0.975m), // Simulating one month change
                    DateTime.UtcNow,
                    "Api2"
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
                        rate * 0.995m, // Simulating a previous rate
                        (rate - (rate * 0.995m)) / (rate * 0.995m), // Simulating one day change
                        (rate - (rate * 0.985m)) / (rate * 0.985m), // Simulating one week change
                        (rate - (rate * 0.975m)) / (rate * 0.975m), // Simulating one month change
                        DateTime.UtcNow,
                        "Api2"
                    );
                }
            }
            return Task.FromResult(result);
        }
    }
}