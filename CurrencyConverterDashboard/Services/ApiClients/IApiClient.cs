using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyConverterDashboard.Models;

namespace CurrencyConverterDashboard.Services.ApiClients
{
    public interface IApiClient
    {
        Task<List<Currency>> GetAvailableCurrenciesAsync();
        Task<ExchangeRate> GetExchangeRateAsync(Currency fromCurrency, Currency toCurrency);
        Task<Dictionary<string, ExchangeRate>> GetAllExchangeRatesAsync(Currency baseCurrency);
    }
}