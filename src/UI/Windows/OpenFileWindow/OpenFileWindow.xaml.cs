using System.Windows;
using MahApps.Metro.Controls;

namespace PsychonautsTools;

/// <summary>
/// Interaction logic for OpenFileWindow.xaml
/// </summary>
public partial class OpenFileWindow : MetroWindow
{
    public OpenFileWindow(OpenFileViewModel viewModel)
    {
        InitializeComponent();
        DataContext = ViewModel = viewModel;
    }

    public OpenFileViewModel ViewModel { get; }

    private void OpenButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (ViewModel.IsValid)
            DialogResult = true;
    }

    private void Window_OnDrop(object sender, DragEventArgs e)
    {
        // Make sure there is file drop data
        if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            return;

        // Get the files
        if (e.Data.GetData(DataFormats.FileDrop) is not string[] files || files.Length == 0)
            return;

        // Use the first file
        ViewModel.FilePath = files[0];
    }
}