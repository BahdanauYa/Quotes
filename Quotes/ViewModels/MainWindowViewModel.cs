using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Quotes.Models;

namespace Quotes.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            DownloadCmdExecute();
        }

        private string _title = "Котировки";
        private Daily _dailyData;
        private DelegateCommand _downloadCmd;
        private DelegateCommand _updateCmd;
        private bool _isBusy;
        private KeyValuePair<string, Currency> _convert1SelectedItem;
        private KeyValuePair<string, Currency> _convert2SelectedItem;
        private double _convert1Value = 1;
        private double _convert2Value = 1;
        private DelegateCommand _searchCmd;
        private string _searchField = string.Empty;
        private string _searchResult;
        private DelegateCommand _convert1ChangeCmd;
        private DelegateCommand _convert2ChangeCmd;

        #region Properties

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public Daily DailyData
        {
            get => _dailyData;
            set
            {
                _dailyData = value;
                RaisePropertyChanged();
            }
        }

        public KeyValuePair<string, Currency> Convert1SelectedItem
        {
            get => _convert1SelectedItem;
            set
            {
                _convert1SelectedItem = value;
                Convert1ChangeCmdExecute();
                RaisePropertyChanged();
            }
        }

        public KeyValuePair<string, Currency> Convert2SelectedItem
        {
            get => _convert2SelectedItem;
            set
            {
                _convert2SelectedItem = value;
                Convert2ChangeCmdExecute();
                RaisePropertyChanged();
            }
        }

        public double Convert1Value
        {
            get => _convert1Value;
            set => SetProperty(ref _convert1Value, value);
        }

        public double Convert2Value
        {
            get => _convert2Value;
            set => SetProperty(ref _convert2Value, value);
        }

        public string SearchField
        {
            get => _searchField;
            set => SetProperty(ref _searchField, value);
        }

        public string SearchResult
        {
            get => _searchResult;
            set => SetProperty(ref _searchResult, value);
        }

        #endregion

        public DelegateCommand DownloadCmd
        {
            get { return _downloadCmd ??= new DelegateCommand(DownloadCmdExecute); }
        }

        public DelegateCommand UpdateCmd
        {
            get { return _updateCmd ??= new DelegateCommand(DownloadCmdExecute); }
        }

        public DelegateCommand SearchCmd
        {
            get { return _searchCmd ??= new DelegateCommand(SearchCmdExecute); }
        }

        public DelegateCommand Convert1ChangeCmd
        {
            get { return _convert1ChangeCmd ??= new DelegateCommand(Convert1ChangeCmdExecute); }
        }

        public DelegateCommand Convert2ChangeCmd
        {
            get { return _convert2ChangeCmd ??= new DelegateCommand(Convert2ChangeCmdExecute); }
        }

        private async void DownloadCmdExecute()
        {
            IsBusy = true;

            await Task.Delay(1000); //намеренно для демонстрации
            await Task.Run(() =>
            {
                DailyData = JsonLoader.Download();

                if (DailyData.Valute != null && DailyData.Valute.Any())
                {
                    Convert1SelectedItem = DailyData.Valute.First();
                    Convert2SelectedItem = DailyData.Valute.Last();
                }
            });
            
            IsBusy = false;
        }

        private async void SearchCmdExecute()
        {
            IsBusy = true;

            await Task.Run(() => { SearchResult = DailyData.Search(SearchField); });

            IsBusy = false;
        }

        private void Convert1ChangeCmdExecute()
        {
            if (Convert1SelectedItem.Value == null || Convert2SelectedItem.Value == null) return;
            var v1Abs = Convert1SelectedItem.Value.Value / Convert1SelectedItem.Value.Nominal;
            var v2Abs = Convert2SelectedItem.Value.Value / Convert2SelectedItem.Value.Nominal;
            Convert2Value = Convert1Value * v1Abs / v2Abs;
        }

        private void Convert2ChangeCmdExecute()
        {
            if (Convert1SelectedItem.Value == null || Convert2SelectedItem.Value == null) return;
            var v1Abs = Convert1SelectedItem.Value.Value / Convert1SelectedItem.Value.Nominal;
            var v2Abs = Convert2SelectedItem.Value.Value / Convert2SelectedItem.Value.Nominal;
            Convert1Value = Convert2Value * v2Abs / v1Abs;
        }
    }
}
