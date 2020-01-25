using App3.Services;
using App3.Views;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Windows.UI.Xaml.Controls;
using App3.Constants;
using App3.Core.Events;
using Prism.Events;

namespace App3.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        #region Constructor
        public ShellViewModel(IMenuNavigationService menuNavigationService, IEventAggregator eventAggregator)
        {
            _menuNavigationService = menuNavigationService;
            MenuViewsWebViewCommand = new DelegateCommand(OnMenuViewsWebView);
            eventAggregator.GetEvent<OpenRightMenuEvent>().Subscribe(OnMenuFileSettings);
        }

        #endregion

        #region Properties

        private Frame _frame;
        private readonly IMenuNavigationService _menuNavigationService;

        #endregion

        #region Commands

        public DelegateCommand MenuViewsWebViewCommand { get; }

        #endregion

        #region Event Aggregator

        private void OnMenuFileSettings()
        {
            _menuNavigationService.OpenInRightPane(typeof(SettingsPage));
        }

        #endregion

        #region Methods

        public void Initialize(Frame frame, SplitView splitView, Frame rightFrame)
        {
            _frame = frame;
            _menuNavigationService.Initialize(splitView, rightFrame);
        }

        private void OnMenuViewsWebView()
        {
            _menuNavigationService.UpdateView(PageTokens.WebViewPage);
        }

        #endregion
    }
}
