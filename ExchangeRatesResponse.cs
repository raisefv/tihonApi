namespace ConverterApi;

public class ExchangeRatesResponse
{
    public DateTime Date { get; set; }
    public DateTime PreviousDate { get; set; }
    public string PreviousURL { get; set; }
    public DateTime Timestamp { get; set; }
    public Dictionary<string, CurrencyRates> Valute { get; set; }
}