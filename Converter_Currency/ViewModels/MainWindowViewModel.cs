using Converter_Currency.Models;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter_Currency.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private CurrencyExchangeService _currencyService = new CurrencyExchangeService();

        public ObservableCollection<string> Currencies { get; set; }
        public decimal ResultCurrency { get; set; }
        public decimal Original { get; set; }
        public string SelectedCurrency { get; set; }
        public string SelectedTwoCurrency { get; set; }


        public MainWindowViewModel()
        {
            LoadCurrencyList();

        }


        private async void LoadCurrencyList()
        {
            List<string> currencyList = await _currencyService.GetCurrencyListAsync();

            if (currencyList != null)
            {
                Currencies = new ObservableCollection<string>(currencyList);
                SelectedCurrency = currencyList[0];
            }
            else
            {
            }
        }
    }
}
