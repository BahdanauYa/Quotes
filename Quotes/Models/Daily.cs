using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;
using Prism.Mvvm;

namespace Quotes.Models
{
    /// <summary> Десериализуемый класс соответствующий файлу .js </summary>
    public class Daily : BindableBase
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

        /// <summary>
        /// Поиск валюты по символьному коду или стране
        /// </summary>
        /// <param name="searchField">Ввод от пользователя</param>
        /// <returns>Строка-результат с курсом к рублю и доллару</returns>
        public string Search(string searchField)
        {
            if ((Valute == null) || (string.IsNullOrEmpty(searchField))) return "Поиск не дал результатов";

            var usd = Valute.FirstOrDefault(i => i.Value.CharCode.ToUpper() == "USD").Value;

            foreach (var pair in Valute)
            {
                if (pair.Value.Name.ToUpper().Contains(searchField.ToUpper()) ||
                    pair.Value.CharCode.ToUpper().Contains(searchField.ToUpper()))
                {
                    return "Валюта: " + pair.Value.Nominal + " " + pair.Value.Name + "\nКурс к рублю: " +
                           pair.Value.Value + "\nКурс к доллару: " + (pair.Value.Value / usd.Value).ToString("F4");
                }
            }

            return "Поиск не дал результатов";
        }

        /// <summary> Инициализация объекта, который будет использоваться для байндинга TreeView </summary>
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
}
