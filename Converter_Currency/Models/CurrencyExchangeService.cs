using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Converter_Currency.Models
{
    class CurrencyExchangeService
    {
        private const string ApiUrl = "https://www.cbr-xml-daily.ru/daily_json.js";

        public async Task<List<string>> GetCurrencyListAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(ApiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                        var currencyData = JsonConvert.DeserializeObject<CurrencyData>(data);

                        List<string> currencies = new List<string>(currencyData.Valute.Keys);
                        return currencies;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public async Task<decimal> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(ApiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                        var currencyData = JsonConvert.DeserializeObject<CurrencyData>(data);

                        if (currencyData.Valute.ContainsKey(fromCurrency) && currencyData.Valute.ContainsKey(toCurrency))
                        {
                            decimal fromRate = currencyData.Valute[fromCurrency].ExchangeRate;
                            decimal toRate = currencyData.Valute[toCurrency].ExchangeRate;

                            decimal result = (amount / fromRate) * toRate;

                            return result;
                        }
                        else
                        {
                            throw new Exception("Выбранные валюты не найдены в данных.");
                        }
                    }
                    else
                    {
                        throw new Exception("Не удалось получить данные с API.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла ошибка при конвертации валют.", ex);
            }
        }
    }


    public class CurrencyData
    {
        public Dictionary <string, CurrencyInfo> Valute { get; set; }
    }
    public class CurrencyInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal ExchangeRate { get; set; }

        public override string ToString()
        {
            return $"{Code} - {ExchangeRate}";
        }
    }
}
