using App3.ViewModels;

namespace App3.Views
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
