using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using App3.Views;

namespace App3.Services
{
    public class DialogService : IFirstRunDisplayService
    {
        public async Task ShowIfAppropriateAsync()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
                {
                    var dialog = new FirstRunDialog();
                    await dialog.ShowAsync();
                });
        }
    }
}
