using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyConverterDashboard.Models;

namespace CurrencyConverterDashboard.Services
{
    public interface ICurrencyService
    {
        Task<List<Currency>> GetAvailableCurrenciesAsync();
        Task<ExchangeRate> GetExchangeRateAsync(Currency fromCurrency, Currency toCurrency);
        Task<Dictionary<string, ExchangeRate>> GetAllExchangeRatesAsync(Currency baseCurrency);
        event EventHandler<Exception> ErrorOccurred;
    }
}