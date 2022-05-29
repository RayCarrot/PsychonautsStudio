using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PsychonautsStudio;

public class MainWindowViewModel : BaseViewModel, IDisposable
{
    #region Constructor

    public MainWindowViewModel(AppUIManager appUI, AppUserData appUserData)
    {
        AppUI = appUI;
        AppUserData = appUserData;
        Nodes = new ObservableCollection<RootDataNodeViewModel>();
        IsEmpty = true;
        
        Nodes.CollectionChanged += (_, _) => IsEmpty = Nodes.Count == 0;
        
        OpenFileCommand = new AsyncRelayCommand(OpenFileAsync);
        UnloadSelectedNodeCommand = new RelayCommand(UnloadSelectedNode);
    }

    #endregion

    #region Private Fields

    private const string FilePath_AppUserData = "AppUserData.json";
    private readonly JsonConverter[] _jsonConverters = { new StringEnumConverter() };

    #endregion

    #region Commands

    public ICommand OpenFileCommand { get; }
    public ICommand UnloadSelectedNodeCommand { get; }

    #endregion

    #region Public Properties

    public AppUIManager AppUI { get; }
    public AppUserData AppUserData { get; }

    public ObservableCollection<RootDataNodeViewModel> Nodes { get; }
    public DataNodeViewModel? SelectedNode { get; set; }
    public bool IsEmpty { get; set; }
    public bool IsLoading { get; set; }

    #endregion

    #region Private Methods

    private Stream OpenFile(string filePath) => File.OpenRead(filePath); // TODO: Move to file manager, allow writing as well?

    #endregion

    #region Public Methods

    public async Task LoadAppUserDataAsync()
    {
        if (!File.Exists(FilePath_AppUserData))
            return;

        try
        {
            string json = File.ReadAllText(FilePath_AppUserData);
            JsonConvert.PopulateObject(json, AppUserData, new JsonSerializerSettings { Converters = _jsonConverters });
        }
        catch (Exception ex)
        {
            AppUI.ShowErrorMessage("An error occurred when reading the app user data", ex);
        }

        if (AppUserData.LoadedFiles != null)
        {
            foreach (AppUserData.LoadedFile loadedFile in AppUserData.LoadedFiles)
            {
                IFileType? type = FileTypes.GetFromID(loadedFile.FileTypeID);

                // Can't load the file if the type wasn't found
                if (type == null)
                    continue;

                FileContext fileContext = new(loadedFile.FilePath, OpenFile(loadedFile.FilePath), loadedFile.Settings, null);
                await LoadFileAsync(fileContext, type);
            }
        }
    }

    public void SaveAppUserData()
    {
        try
        {
            AppUserData.LoadedFiles = Nodes.
                Select(x => new AppUserData.LoadedFile(x.FileContext.FilePath, x.FileType.ID, x.FileContext.Settings)).
                ToArray();

            string json = JsonConvert.SerializeObject(AppUserData, Formatting.Indented, _jsonConverters);
            File.WriteAllText(FilePath_AppUserData, json);
        }
        catch (Exception ex)
        {
            AppUI.ShowErrorMessage("An error occurred when saving the app user data", ex);
        }
    }

    public Task OpenFileAsync() => OpenFileAsync(null);

    public Task OpenFileAsync(string? defaultFilePath)
    {
        AppUIManager.OpenFileResult? result = AppUI.OpenFile(defaultFilePath);

        if (result == null)
            return Task.CompletedTask;

        return LoadFileAsync(result.FileContext, result.Type);
    }

    public async Task LoadFileAsync(FileContext fileContext, IFileType type)
    {
        // Avoid loading the same file multiple times
        RootDataNodeViewModel? existingNode = Nodes.FirstOrDefault(x => 
            x.FileContext.FilePath.Equals(fileContext.FilePath, StringComparison.InvariantCultureIgnoreCase));

        if (existingNode != null)
        {
            existingNode.IsSelected = true;
            return;
        }

        if (IsLoading)
            return;

        IsLoading = true;

        try
        {
            DataNode node = await Task.Run(() => type.CreateDataNode(fileContext));
            Nodes.Add(new RootDataNodeViewModel(node, fileContext, type));

            if (!type.LeaveFileStreamOpen)
                fileContext.Dispose();
        }
        catch (Exception ex)
        {
            fileContext.Dispose();
            AppUI.ShowErrorMessage($"An error occurred when loading the file {fileContext.FilePath}", ex);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void UnloadSelectedNode()
    {
        if (SelectedNode is not RootDataNodeViewModel root)
            return;

        UnloadNode(root);
    }

    public void UnloadNode(RootDataNodeViewModel node)
    {
        int index = Nodes.IndexOf(node);

        if (index == -1)
            return;

        node.Dispose();
        Nodes.Remove(node);

        if (index > 0 && index == Nodes.Count)
            index--;

        if (Nodes.Count > index)
            Nodes[index].IsSelected = true;
    }

    public void Dispose()
    {
        foreach (RootDataNodeViewModel node in Nodes)
            node.Dispose();
    }

    #endregion
}