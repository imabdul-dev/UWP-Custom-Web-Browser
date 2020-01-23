using System;
using System.Threading.Tasks;
using App3.Views;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using App3.Core.Service;

namespace App3.Services
{
    public class FirstRunDisplayService : IFirstRunDisplayService
    {
        private static bool shown;

        public async Task ShowIfAppropriateAsync()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
                {
                    if (SystemInformation.IsFirstRun && !shown)
                    {
                        shown = true;
                        BookmarkDataService.AddBookmarks();
                        var dialog = new FirstRunDialog();
                        await dialog.ShowAsync();
                    }
                });
        }
    }
}
