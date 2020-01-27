using System.Threading.Tasks;

namespace UWPBrowser.Services
{
    public interface IFirstRunService
    {
        Task RunIfAppropriateAsync();
    }
}
