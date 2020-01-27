using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using App3.Core.Events;
using App3.Core.Models;
using App3.Core.Service;
using Prism.Commands;
using Prism.Events;
using Prism.Windows.Mvvm;
using UWPBrowser.Helpers;
using UWPBrowser.Services;

namespace UWPBrowser.ViewModels
{
    public class WebViewViewModel : ViewModelBase
    {
        #region Constructor

        public WebViewViewModel(IMenuNavigationService menuNavigationService, IEventAggregator eventAggregator, IDialogService dialogService)
        {
            BookmarksVisibility = Visibility.Collapsed;
            WebViewVisibility = Visibility.Visible;
            _menuNavigationService = menuNavigationService;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            IsLoading = true;
            Bookmarks = new ObservableCollection<Bookmark>();
            WebViewSource = new Uri(WebHelper.Url);
            BrowserBackCommand = new DelegateCommand(() => _webViewService?.GoBack(), () => _webViewService?.CanGoBack ?? false);
            BrowserForwardCommand = new DelegateCommand(() => _webViewService?.GoForward(), () => _webViewService?.CanGoForward ?? false);
            RefreshCommand = new DelegateCommand(() => _webViewService?.Refresh());
            RetryCommand = new DelegateCommand(Retry);
            MenuViewsBookmarksCommand = new DelegateCommand(MenuViewsBookmarksCommand_);
            SearchTextBoxCommand = new DelegateCommand(SearchTextBoxCommand_);
            AddToBookmarkCommand = new DelegateCommand(AddToBookmarkCommand_);
            MenuFileSettingsCommand = new DelegateCommand(OnMenuFileSettings);
            OpenHistoryCommand = new DelegateCommand(OpenHistoryCommand_);
            ItemClickCommand = new DelegateCommand<Bookmark>(ItemClickCommand_);
            LoadBookmarks();

            // Note that the WebViewService is set from within the view because it needs a reference to the WebView control
        }

        #endregion

        #region Properties

        private readonly IEventAggregator _eventAggregator;
        private readonly IMenuNavigationService _menuNavigationService;
        private readonly IDialogService _dialogService;

        #endregion

        #region Fields

        private string _url;
        public string Url
        {
            get => _url;
            set { SetProperty(ref _url, value); }
        }

        private Uri _webViewsource;
        public Uri WebViewSource
        {
            get { return _webViewsource; }
            set { SetProperty(ref _webViewsource, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }

            set
            {
                if (value)
                {
                    IsShowingFailedMessage = false;
                }

                SetProperty(ref _isLoading, value);
                IsLoadingVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility _isLoadingVisibility;
        public Visibility IsLoadingVisibility
        {
            get
            {
                return _isLoadingVisibility;
            }

            set
            {
                SetProperty(ref _isLoadingVisibility, value);
            }
        }

        private bool _isShowingFailedMessage;
        public bool IsShowingFailedMessage
        {
            get
            {
                return _isShowingFailedMessage;
            }

            set
            {
                if (value)
                {
                    IsLoading = false;
                }

                SetProperty(ref _isShowingFailedMessage, value);
                FailedMesageVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility _failedMessageVisibility;
        public Visibility FailedMesageVisibility
        {
            get
            {
                return _failedMessageVisibility;
            }

            set
            {
                SetProperty(ref _failedMessageVisibility, value);
            }
        }

        private Visibility _bookmarksVisibility;
        public Visibility BookmarksVisibility
        {
            get
            {
                return _bookmarksVisibility;
            }

            set
            {
                SetProperty(ref _bookmarksVisibility, value);
            }
        }

        private Visibility _webViewVisibility;
        public Visibility WebViewVisibility
        {
            get
            {
                return _webViewVisibility;
            }

            set
            {
                SetProperty(ref _webViewVisibility, value);
            }
        }

        private IWebViewService _webViewService;
        public IWebViewService WebViewService
        {
            get
            {
                return _webViewService;
            }

            // the WebViewService is set from within the view (instead of IoC) because it needs a reference to the control
            set
            {
                if (_webViewService != null)
                {
                    _webViewService.NavigationComplete -= WebViewService_NavigationComplete;
                    _webViewService.NavigationFailed -= WebViewService_NavigationFailed;
                }

                _webViewService = value;
                _webViewService.NavigationComplete += WebViewService_NavigationComplete;
                _webViewService.NavigationFailed += WebViewService_NavigationFailed;
            }
        }

        private ObservableCollection<Bookmark> _bookmarks;
        public ObservableCollection<Bookmark> Bookmarks
        {
            get => _bookmarks;
            set { SetProperty(ref _bookmarks, value); }
        }

        #endregion

        #region Commands

        public DelegateCommand BrowserBackCommand { get; }
        public DelegateCommand BrowserForwardCommand { get; }
        public DelegateCommand RefreshCommand { get; }
        public DelegateCommand RetryCommand { get; }
        public DelegateCommand MenuViewsBookmarksCommand { get; }
        public DelegateCommand SearchTextBoxCommand { get; }
        public DelegateCommand AddToBookmarkCommand { get; }
        public DelegateCommand MenuFileSettingsCommand { get; }
        public DelegateCommand OpenHistoryCommand { get; }
        public DelegateCommand<Bookmark> ItemClickCommand { get; set; }


        #endregion

        #region Methods

        private async void OpenHistoryCommand_()
        {
            await _dialogService.ShowSettingsAsync();
        }

        private void SearchTextBoxCommand_()
        {
            if (!string.IsNullOrEmpty(Url))
            {
                BookmarksVisibility = Visibility.Collapsed;
                WebViewVisibility = Visibility.Visible;
                var url = WebHelper.ValidURL(Url);
                WebHelper.Url = Url;
                WebViewSource = url;
            }
        }

        private void AddToBookmarkCommand_()
        {
            if (!string.IsNullOrEmpty(Url))
            {
                BookmarkDataService.AddBookmark(_webViewService?.GetWebTitle(), Url);
                _dialogService.ShowMessageAsync();
            }
        }

        private void MenuViewsBookmarksCommand_()
        {
            Url = string.Empty;
            BookmarksVisibility = Visibility.Visible;
            WebViewVisibility = Visibility.Collapsed;
        }

        private void WebViewService_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            NavFailed();
        }

        private void WebViewService_NavigationComplete(object sender, WebViewNavigationCompletedEventArgs e)
        {
            NavCompleted(e);
        }

        private void NavCompleted(WebViewNavigationCompletedEventArgs e)
        {
            IsLoading = false;
            Url = e.Uri.ToString();
            HistoryDataService.AddorUpdateHistoryItem(_webViewService?.GetWebTitle(), Url);
            RaisePropertyChanged(nameof(BrowserBackCommand));
            RaisePropertyChanged(nameof(BrowserForwardCommand));
        }

        private void NavFailed()
        {
            // Use `e.WebErrorStatus` to vary the displayed message based on the error reason
            IsShowingFailedMessage = true;
        }

        private void Retry()
        {
            IsShowingFailedMessage = false;
            IsLoading = true;

            _webViewService?.Refresh();
        }

        private void OnMenuFileSettings()
        {
            _eventAggregator.GetEvent<OpenRightMenuEvent>().Publish();
        }

        private void ItemClickCommand_(Bookmark bookmark)
        {
            IsLoading = true;
            BookmarksVisibility = Visibility.Collapsed;
            WebViewVisibility = Visibility.Visible;
            WebHelper.Url = bookmark.Url;
            WebViewSource = new Uri(bookmark.Url);
        }

        public void LoadBookmarks()
        {
            var data = BookmarkDataService.GetBookmarksData();
            foreach (var item in data)
            {
                Bookmarks.Add(item);
            }
        }

        #endregion
    }
}
