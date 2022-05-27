using System;
using System.ComponentModel;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Input;

namespace PsychonautsTools;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : MetroWindow
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = ViewModel = viewModel;
        Title += $" ({App.Version})";
    }

    private GridLength _prevFileGridFooterRowHeight = new(200);

    public MainWindowViewModel ViewModel { get; }

    private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadAppUserDataAsync();
    }

    private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        ViewModel.SaveAppUserData();
    }

    private void MainWindow_OnClosed(object? sender, EventArgs e)
    {
        ViewModel.Dispose();
    }

    private void DataNode_OnExpanded(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { DataContext: DataNodeViewModel node })
            node.Expand();
    }

    private void DataNodes_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        // Set the selected node in the view model
        ViewModel.SelectedNode = (DataNodeViewModel)e.NewValue;

        // Check if the serializer log and raw data should be visible
        bool isBinary = ViewModel.SelectedNode?.IsBinary == true;

        if (FileGridFooterRow.Height.Value != 0)
            _prevFileGridFooterRowHeight = FileGridFooterRow.Height;

        FileGridFooterRow.Height = isBinary ? _prevFileGridFooterRowHeight : new GridLength(0);
    }

    private void DataNodes_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (ViewModel.SelectedNode != null)
            ViewModel.SelectedNode.IsSelected = false;
    }

    private async void DataNodes_OnDrop(object sender, DragEventArgs e)
    {
        // Make sure there is file drop data
        if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            return;

        // Get the files
        if (e.Data.GetData(DataFormats.FileDrop) is not string[] files || files.Length == 0)
            return;

        // Hack to avoid file explorer freezing
        await Task.Yield();

        // Load the first file
        await ViewModel.OpenFileAsync(files[0]);
    }

    private void ExitMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}