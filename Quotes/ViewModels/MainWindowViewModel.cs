using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quotes.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
        }

        private string _title = "Котировки";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
