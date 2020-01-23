using System.Threading.Tasks;
using App3.Helpers;
using App3.Services;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace App3.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { SetProperty(ref _elementTheme, value); }
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { SetProperty(ref _versionDescription, value); }
        }

        private DelegateCommand<object> _switchThemeCommand;

        public DelegateCommand<object> SwitchThemeCommand
        {
            get
            {
                return _switchThemeCommand ?? (_switchThemeCommand = new DelegateCommand<object>(
                           async (param) =>
                           {
                               ElementTheme = (ElementTheme) param;
                               await ThemeSelectorService.SetThemeAsync((ElementTheme) param);
                           }));
            }
        }

        public async Task InitializeAsync()
        {
            VersionDescription = GetVersionDescription();
            await Task.CompletedTask;
        }

        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
