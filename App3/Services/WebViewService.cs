using System;
using Windows.UI.Xaml.Controls;

namespace UWPBrowser.Services
{
    public class WebViewService : IWebViewService
    {
        public readonly WebView _webView;

        public WebViewService(WebView webViewInstance)
        {
            _webView = webViewInstance;
            _webView.NavigationCompleted += WebView_NavigationCompleted;
            _webView.NavigationFailed += WebView_NavigationFailed;
        }

        public void Detatch()
        {
            if (_webView != null)
            {
                _webView.NavigationCompleted -= WebView_NavigationCompleted;
                _webView.NavigationFailed += WebView_NavigationFailed;
            }
        }

        private void WebView_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            NavigationFailed?.Invoke(sender, e);
        }

        private void WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs e)
        {
            NavigationComplete?.Invoke(sender, e);
        }

        public void Refresh()
        {
            _webView?.Refresh();
        }

        public string GetWebTitle()
        {
            return _webView.DocumentTitle;
        }

        public void GoForward()
        {
            _webView?.GoForward();
        }

        public void Navigate(Uri url)
        {
            _webView?.Navigate(url);
        }

        public void GoBack()
        {
            _webView?.GoBack();
        }

        public bool CanGoForward => _webView?.CanGoForward == true;

        public bool CanGoBack => _webView?.CanGoBack == true;

        public event EventHandler<WebViewNavigationCompletedEventArgs> NavigationComplete;

        public event EventHandler<WebViewNavigationFailedEventArgs> NavigationFailed;
    }
}
