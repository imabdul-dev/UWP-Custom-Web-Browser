using System;
using Windows.UI.Core;
using App3.Services;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using App3.Constants;
using App3.Core.Events;
using App3.Core.Models;
using App3.Core.Service;
using App3.Helpers;
using Prism.Events;

namespace App3.ViewModels
{
    public class WebViewViewModel : ViewModelBase
    {
        #region Constructor

        public WebViewViewModel(IMenuNavigationService menuNavigationService, IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _menuNavigationService = menuNavigationService;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            IsLoading = true;
            Source = new Uri(WebHelper.Url);
            BrowserBackCommand = new DelegateCommand(() => _webViewService?.GoBack(), () => _webViewService?.CanGoBack ?? false);
            BrowserForwardCommand = new DelegateCommand(() => _webViewService?.GoForward(), () => _webViewService?.CanGoForward ?? false);
            RefreshCommand = new DelegateCommand(() => _webViewService?.Refresh());
            RetryCommand = new DelegateCommand(Retry);
            MenuViewsBookmarksCommand = new DelegateCommand(MenuViewsBookmarksCommand_);
            SearchTextBoxCommand = new DelegateCommand(SearchTextBoxCommand_);
            AddToBookmarkCommand = new DelegateCommand(AddToBookmarkCommand_);
            MenuFileSettingsCommand = new DelegateCommand(OnMenuFileSettings);
            OpenHistoryCommand = new DelegateCommand(OpenHistoryCommand_);
            eventAggregator.GetEvent<OpenBookmarkEvent>().Subscribe(OpenBookmarkEvent_);

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

        private Uri _source;
        public Uri Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value); }
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
                //_webViewService.Navigate(Source);
            }
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
        public DelegateCommand OpenHistoryCommand { get; set; }

        #endregion

        #region Event Aggregator

        private void OpenBookmarkEvent_(Bookmark bookmark)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            currentView.BackRequested -= BackToWebView;
            WebHelper.Url = bookmark.Url;
        }

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
                _webViewService?.Navigate(WebHelper.ValidURL(Url));
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
            _menuNavigationService.UpdateView(PageTokens.BookmarksPage);
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            currentView.BackRequested += BackToWebView;
        }

        private void BackToWebView(object sender, BackRequestedEventArgs e)
        {
            _menuNavigationService.UpdateView(PageTokens.WebViewPage);
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            currentView.BackRequested -= BackToWebView;
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

        #endregion
    }
}
