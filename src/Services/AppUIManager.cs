using System;
using System.IO;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace PsychonautsTools;

public class AppUIManager
{
    #region Constructor

    public AppUIManager(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    #endregion

    #region Private Properties

    private IServiceProvider ServiceProvider { get; }

    #endregion

    #region Open File

    public OpenFileResult? OpenFile(string? defaultFilePath)
    {
        OpenFileWindow win = ServiceProvider.GetRequiredService<OpenFileWindow>();
        OpenFileViewModel vm = win.ViewModel;

        if (defaultFilePath != null)
            vm.FilePath = defaultFilePath;

        win.ShowDialog();

        if (win.DialogResult != true)
            return null;

        return new OpenFileResult(vm.CreateFileContext(), vm.SelectedFileType);
    }

    public record OpenFileResult(FileContext FileContext, IFileType Type);

    #endregion

    #region Browse File

    public string? BrowseFile(string title, string? defaultFilePath)
    {
        OpenFileDialog dialog = new()
        {
            CheckFileExists = true,
            FileName = defaultFilePath,
            InitialDirectory = defaultFilePath != null ? Path.GetDirectoryName(defaultFilePath) : null,
            Title = title,
        };

        bool? result = dialog.ShowDialog();

        if (result != true)
            return null;

        return dialog.FileName;
    }

    #endregion

    #region Error Message

    public void ShowErrorMessage(string message, Exception? exception)
    {
        if (exception != null)
            message += $"{Environment.NewLine}{Environment.NewLine}Error: {exception.Message}";

        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    #endregion
}