using System.Threading.Tasks;

namespace App3.Services
{
    public interface IDialogService
    {
        Task ShowSettingsAsync();
        Task ShowMessageAsync();
    }
}
