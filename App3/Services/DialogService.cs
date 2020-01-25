
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using App3.Views;

namespace App3.Services
{
    public class DialogService : IDialogService
    {
        public async Task ShowSettingsAsync()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
                {
                    var dialog = new DialogPage();
                    await dialog.ShowAsync();
                });
        }

        public async Task ShowMessageAsync()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
                {
                    var dialog = new MessageBox();
                    await dialog.ShowAsync();
                });
        }
    }
}
