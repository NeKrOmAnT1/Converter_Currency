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
            

    }

    public class CurrencyData
    {
        public Dictionary <string, CurrencyInfo> Valute { get; set; }
    }
    public class CurrencyInfo
    {
        public string Name { get; set;}
        public decimal Value { get; set;}

    }
}
