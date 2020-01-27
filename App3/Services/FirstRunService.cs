using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using App3.Core.Service;
using Microsoft.Toolkit.Uwp.Helpers;
using UWPBrowser.Views;

namespace UWPBrowser.Services
{
    public class FirstRunService : IFirstRunService
    {
        private static bool shown;

        public async Task RunIfAppropriateAsync()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
                {
                    if (SystemInformation.IsFirstRun && !shown)
                    {
                        shown = true;
                        var dialog = new FirstRunDialog();
                        BookmarkDataService.AddBookmarks();
                        await dialog.ShowAsync();
                    }
                });
        }
    }
}
