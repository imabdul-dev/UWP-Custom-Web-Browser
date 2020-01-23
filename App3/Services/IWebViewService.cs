using System;
using Windows.UI.Xaml.Controls;

namespace App3.Services
{
    public interface IWebViewService
    {
        bool CanGoBack { get; }

        bool CanGoForward { get; }

        void Navigate(Uri url);

        void GoBack();

        void GoForward();

        void Refresh();

        string GetWebTitle();

        event EventHandler<WebViewNavigationCompletedEventArgs> NavigationComplete;

        event EventHandler<WebViewNavigationFailedEventArgs> NavigationFailed;
    }
}
