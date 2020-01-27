using System.Collections.ObjectModel;
using App3.Core.Models;
using App3.Core.Service;
using Prism.Windows.Mvvm;

namespace UWPBrowser.ViewModels
{
    public class DialogViewodel : ViewModelBase
    {
        #region Constructor

        public DialogViewodel()
        {
            HistoryCollection = new ObservableCollection<History>();
            LoadData();
        }

        #endregion

        #region Fields

        private ObservableCollection<History> _historyCollection;
        public ObservableCollection<History> HistoryCollection
        {
            get => _historyCollection;
            set
            {
                SetProperty(ref _historyCollection, value);
            }
        }

        #endregion

        #region Methods

        private void LoadData()
        {
            var data = HistoryDataService.GetHistoryData();
            foreach (var item in data)
            {
                HistoryCollection.Add(item);
            }
        }

        #endregion
    }
}
