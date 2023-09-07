using Converter_Currency.Models;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
        public ICommand ConvertCurrencyCommand { get; }



        public MainWindowViewModel()
        {
            LoadCurrencyList();
            ConvertCurrencyCommand = new DelegateCommand(ConvertCurrency);
        }


        private async void LoadCurrencyList()
        {
            List<string> currencyList = await _currencyService.GetCurrencyListAsync();

            if (currencyList != null)
            {
                Currencies = new ObservableCollection<string>(currencyList);
                // валюта по умолчанию
                SelectedCurrency = currencyList[0];
            }
            else
            {
                
            }
        }
        private async void ConvertCurrency()
        {
            Console.WriteLine("ConvertCurrency method called."); 

            if (Original <= 0)
            {
                Console.WriteLine("Invalid input: Original is less than or equal to 0."); 
                return;
            }

            try
            {
                ResultCurrency = await _currencyService.ConvertCurrencyAsync(SelectedCurrency, SelectedTwoCurrency, Original);
                RaisePropertyChanged(nameof(ResultCurrency));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during currency conversion: {ex.Message}");

            }
        }
    }
}
