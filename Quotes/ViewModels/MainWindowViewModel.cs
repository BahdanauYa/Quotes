using Prism.Mvvm;
using System;
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
        private DelegateCommand _downloadCmd;
        private Daily _dailyData;
        private DelegateCommand _updateCmd;
        private bool _isBusy;

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

        public DelegateCommand DownloadCmd
        {
            get { return _downloadCmd ??= new DelegateCommand(DownloadCmdExecute); }
        }

        public DelegateCommand UpdateCmd
        {
            get { return _updateCmd ??= new DelegateCommand(UpdateCmdExecute); }
        }

        private async void DownloadCmdExecute()
        {
            IsBusy = true;
            await Task.Run(async () =>
            {
                await Task.Delay(1000);
                DailyData = JsonLoader.Download();
            });
            IsBusy = false;
        }

        private async void UpdateCmdExecute()
        {
            IsBusy = true;
            await Task.Run(() => { DailyData = JsonLoader.Download(); });
            IsBusy = false;
        }
    }
}
