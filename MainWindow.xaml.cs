using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace ConverterApi;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        _ = GetRatesMethod();
    }
    
    private Dictionary<string, CurrencyRates> currencyRates = new Dictionary<string, CurrencyRates>();
    HttpClient client = new HttpClient();
    const string url = "https://www.cbr-xml-daily.ru/daily_json.js";

    private async Task GetRatesMethod()
    {
        string json = await client.GetStringAsync(url);
        ExchangeRatesResponse rates = JsonConvert.DeserializeObject<ExchangeRatesResponse>(json);

        if (rates != null && rates.Valute != null)
        {
            currencyRates = rates.Valute;

            currencyRates["RUB"] = new CurrencyRates
            {
                CharCode = "RUB",
                Name = "Российский рубль",
                Nominal = 1,
                Value = 1
            };

            foreach (var currency in currencyRates)
            {
                FromCurrencyComboBox.Items.Add(currency.Key);
                ToCurrencyComboBox.Items.Add(currency.Key);
            }

            FromCurrencyComboBox.SelectedItem = "USD";
            ToCurrencyComboBox.SelectedItem = "RUB";
        }
    }
    
    private void OnConvertButton_Click(object sender, RoutedEventArgs e)
    {
        if (FromCurrencyComboBox.SelectedItem == null || ToCurrencyComboBox.SelectedItem == null)
        {
            MessageBox.Show("Выберите валюты для конвертации");
            return;
        }

        if (!decimal.TryParse(AmountTextBox.Text, out decimal amount))
        {
            MessageBox.Show("Введите корректную сумму");
            return;
        }

        Convert();
    }
    public void Convert()
    {
        if (FromCurrencyComboBox.SelectedItem == null || ToCurrencyComboBox.SelectedItem == null) return;

        string fromCurrency = FromCurrencyComboBox.SelectedValue.ToString();
        string toCurrency = ToCurrencyComboBox.SelectedValue.ToString();

        if (AmountTextBox.Text.Contains("."))
        {
            MessageBox.Show("Точка не может быть разделителем", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            AmountTextBox.Text = null;
            ResultTextBlock.Text = null;
            return;
        }

        if (!double.TryParse(AmountTextBox.Text, out double amount))
        {
            MessageBox.Show("Введите корректную сумму.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            AmountTextBox.Text = null;
            ResultTextBlock.Text = null;
            return;
        }

        double rateFrom = currencyRates[fromCurrency].Value / currencyRates[fromCurrency].Nominal;
        double rateTo = currencyRates[toCurrency].Value / currencyRates[toCurrency].Nominal;

        double result = amount * (rateFrom / rateTo);
        ResultTextBlock.Text = result.ToString("F2");
    }

    private void OnCurrencySelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Convert();
    }
}