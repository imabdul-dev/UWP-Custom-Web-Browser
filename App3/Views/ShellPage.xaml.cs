using Windows.UI.Xaml.Controls;
using UWPBrowser.ViewModels;

namespace UWPBrowser.Views
{
    public sealed partial class ShellPage
    {
        private ShellViewModel ViewModel => DataContext as ShellViewModel;

        public Frame ShellFrame => shellFrame;

        public ShellPage()
        {
            InitializeComponent();
        }

        public void SetRootFrame(Frame frame)
        {
            shellFrame.Content = frame;
            ViewModel.Initialize(frame, splitView, rightFrame);
        }
    }
}
