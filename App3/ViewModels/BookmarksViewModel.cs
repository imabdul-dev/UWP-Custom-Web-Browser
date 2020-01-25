using System.Collections.ObjectModel;
using App3.Constants;
using App3.Core.Events;
using App3.Core.Models;
using App3.Core.Service;
using App3.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Windows.Mvvm;

namespace App3.ViewModels
{

    public class BookmarksViewModel : ViewModelBase
    {
        #region Constructor

        public BookmarksViewModel(IEventAggregator eventAggregator, IMenuNavigationService menuNavigationService)
        {
            _eventAggregator = eventAggregator;
            _menuNavigationService = menuNavigationService;
            Source = new ObservableCollection<Bookmark>();
            LoadDataAsync();
            ItemClickCommand = new DelegateCommand<Bookmark>(OnItemClick);
        }

        #endregion

        #region Properties

        private readonly IEventAggregator _eventAggregator;
        private readonly IMenuNavigationService _menuNavigationService;

        #endregion

        #region Fields

        private ObservableCollection<Bookmark> _source;
        public ObservableCollection<Bookmark> Source
        {
            get => _source;
            set { SetProperty(ref _source, value); }
        }

        #endregion

        #region Commands

        public DelegateCommand<Bookmark> ItemClickCommand { get; set; }

        #endregion

        #region Methods

        private void OnItemClick(Bookmark clickedItem)
        {
            if (clickedItem != null)
            {
                if (clickedItem.Url == "Add" && clickedItem.Url == "Add")
                {

                }
                else
                {
                    _eventAggregator.GetEvent<OpenBookmarkEvent>().Publish(clickedItem);
                    _menuNavigationService.UpdateView(PageTokens.WebViewPage);
                }
            }
        }

        public void LoadDataAsync()
        {
            var data = BookmarkDataService.GetBookmarksData();
            foreach (var item in data)
            {
                Source.Add(item);
            }
        }

        #endregion
    }
}
