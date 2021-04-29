using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using Prism.Mvvm;

namespace Quotes.Models
{
    public class Daily:BindableBase
    {
        private Dictionary<string, Currency> _valute;
        private Dictionary<string, CurrencyWrapper> _valuteBinding;
        public DateTime Date { get; set; }
        public DateTime PreviousDate { get; set; }
        public string PreviousURL { get; set; }
        public DateTime Timestamp { get; set; }

        public Dictionary<string, Currency> Valute
        {
            get => _valute;
            set
            {
                _valute = value;
                RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        public Dictionary<string, CurrencyWrapper> ValuteBinding
        {
            get => _valuteBinding;
            set
            {
                _valuteBinding = value;
                RaisePropertyChanged();
            }
        }

        /// <summary> Инициализация объекта, который будет использоваться для байндинга </summary>
        public void InitBindingData()
        {
            ValuteBinding = new Dictionary<string, CurrencyWrapper>();
            foreach (var item in Valute)
            {
                ValuteBinding.Add(item.Key, new CurrencyWrapper(item.Value));
            }
        }
    }

    public class CurrencyWrapper
    {
        public CurrencyWrapper(Currency currency)
        {
            Currencies = new ObservableCollection<Currency> {currency};
        }

        public IList<Currency> Currencies { get; set; }
    }


    public class Currency
    {
        public Currency()
        {
            //Temp = new ObservableCollection<string>(){"355"};
        }

        public string ID { get; set; }
        public string NumCode { get; set; }
        public string CharCode { get; set; }
        public int Nominal { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public double Previous { get; set; }
        //public ObservableCollection<string> Temp { get; set; }
    }
}
