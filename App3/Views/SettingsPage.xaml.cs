using Windows.UI.Xaml.Navigation;
using UWPBrowser.ViewModels;

namespace UWPBrowser.Views
{
    public sealed partial class SettingsPage
    {
        private SettingsViewModel ViewModel => DataContext as SettingsViewModel;

        public SettingsPage()
        {
            InitializeComponent();
        }

        // If this SettingsPage is opened in the right pane of the MenuBar project, we use
        // these methods instead of OnNavigatedTo and OnNavigatingFrom in the ViewModel.
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.InitializeAsync();
        }
    }
}
