using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace ConverterApi;

/// <summary>
/// Главное окно приложения конвертера валют
/// </summary>
public partial class MainWindow : Window
{
    // Конструктор главного окна
    public MainWindow()
    {
        InitializeComponent(); // Инициализация компонентов XAML
        LoadCurrencies(); // Загрузка списка валют при запуске
    }

    // Асинхронная загрузка списка валют
    private async void LoadCurrencies()
    {
        await GetRatesMethod(); // Вызов метода получения курсов валют
    }
    
    // Словарь для хранения курсов валют (ключ - код валюты, значение - данные о валюте)
    private Dictionary<string, CurrencyRates> currencyRates = new Dictionary<string, CurrencyRates>();
    
    // HTTP-клиент для запросов к API
    HttpClient client = new HttpClient();
    
    // URL API Центробанка России для получения курсов валют
    const string url = "https://www.cbr-xml-daily.ru/daily_json.js";

    // Основной метод получения курсов валют
    private async Task GetRatesMethod()
    {
        // Получаем JSON-данные с API
        string json = await client.GetStringAsync(url);
        
        // Десериализуем JSON в объект ExchangeRatesResponse
        ExchangeRatesResponse rates = JsonConvert.DeserializeObject<ExchangeRatesResponse>(json);

        // Если данные получены и содержат курсы валют
        if (rates != null && rates.Valute != null)
        {
            // Сохраняем полученные курсы валют
            currencyRates = rates.Valute;

            // Добавляем рубли в список валют (их нет в API, так как это базовая валюта)
            currencyRates["RUB"] = new CurrencyRates
            {
                CharCode = "RUB", // Код валюты
                Name = "Российский рубль", // Название
                Nominal = 1, // Номинал (1 рубль)
                Value = 1 // Курс (1 рубль = 1 рубль)
            };

            // Очищаем комбобоксы перед заполнением
            FromCurrencyComboBox.Items.Clear();
            ToCurrencyComboBox.Items.Clear();

            // Заполняем комбобоксы кодами валют
            foreach (var currency in currencyRates)
            {
                FromCurrencyComboBox.Items.Add(currency.Key);
                ToCurrencyComboBox.Items.Add(currency.Key);
            }

            // Устанавливаем значения по умолчанию (USD -> RUB)
            FromCurrencyComboBox.SelectedItem = "USD";
            ToCurrencyComboBox.SelectedItem = "RUB";
        }
    }
    
    // Обработчик нажатия кнопки "Конвертировать"
    private void OnConvertButton_Click(object sender, RoutedEventArgs e)
    {
        // Проверка, что валюты выбраны
        if (FromCurrencyComboBox.SelectedItem == null || ToCurrencyComboBox.SelectedItem == null)
        {
            MessageBox.Show("Выберите валюты для конвертации");
            return;
        }

        // Проверка корректности введенной суммы
        if (!decimal.TryParse(AmountTextBox.Text, out decimal amount))
        {
            MessageBox.Show("Введите корректную сумму");
            return;
        }

        // Вызов метода конвертации
        Convert();
    }

    // Метод выполнения конвертации валют
    public void Convert()
    {
        // Проверка, что валюты выбраны
        if (FromCurrencyComboBox.SelectedItem == null || ToCurrencyComboBox.SelectedItem == null) return;

        // Получаем выбранные валюты
        string fromCurrency = FromCurrencyComboBox.SelectedValue.ToString();
        string toCurrency = ToCurrencyComboBox.SelectedValue.ToString();

        // Проверка на использование точки как разделителя (в России используется запятая)
        if (AmountTextBox.Text.Contains("."))
        {
            MessageBox.Show("Точка не может быть разделителем", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            AmountTextBox.Text = null;
            ResultTextBlock.Text = null;
            return;
        }

        // Парсинг суммы с проверкой корректности
        if (!double.TryParse(AmountTextBox.Text, out double amount))
        {
            MessageBox.Show("Введите корректную сумму.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            AmountTextBox.Text = null;
            ResultTextBlock.Text = null;
            return;
        }

        // Расчет курсов с учетом номинала
        // Например, если номинал 10 единиц валюты, то курс будет Value / 10
        double rateFrom = currencyRates[fromCurrency].Value / currencyRates[fromCurrency].Nominal;
        double rateTo = currencyRates[toCurrency].Value / currencyRates[toCurrency].Nominal;

        // Вычисление результата: (сумма * курс исходной валюты) / курс целевой валюты
        double result = amount * (rateFrom / rateTo);
        
        // Вывод результата с округлением до 2 знаков после запятой
        ResultTextBlock.Text = result.ToString("F2");
    }

    // Обработчик изменения выбранной валюты
    private void OnCurrencySelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Convert(); // Автоматическая конвертация при изменении выбора валюты
    }
}