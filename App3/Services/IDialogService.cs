using System.Threading.Tasks;

namespace UWPBrowser.Services
{
    public interface IDialogService
    {
        Task ShowSettingsAsync();
        Task ShowMessageAsync();
    }
}
