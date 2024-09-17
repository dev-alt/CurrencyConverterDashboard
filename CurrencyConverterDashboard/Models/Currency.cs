using System;

namespace CurrencyConverterDashboard.Models
{
    public class Currency(string code, string name, string symbol)
    {
        public string Code { get; } = code ?? throw new ArgumentNullException(nameof(code));
        private string Name { get; set; } = name ?? throw new ArgumentNullException(nameof(name));
        public string Symbol { get; set; } = symbol ?? throw new ArgumentNullException(nameof(symbol));

        public override string ToString()
        {
            return $"{Code} - {Name}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Currency currency &&
                   Code == currency.Code;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Code);
        }
    }
    
    public class CurrencyRate(Currency currency, decimal rate)
    {
        public Currency Currency { get; } = currency;
        public decimal Rate { get; } = rate;
    }
}