using UWPBrowser.ViewModels;

namespace UWPBrowser.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DialogPage
    {
        public DialogViewodel ViewModel { get; } = new DialogViewodel();

        public DialogPage()
        {
            InitializeComponent();
        }
    }
}
