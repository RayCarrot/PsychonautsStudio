using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows.Input;
using PsychoPortal;

namespace PsychonautsStudio;

public class OpenFileViewModel : BaseViewModel
{
    #region Constructor

    public OpenFileViewModel(AppUIManager appUI)
    {
        AppUI = appUI;
        AvailableFileTypes = new ObservableCollection<IFileType>(FileTypes.GetFileTypes());
        SelectedFileType = AvailableFileTypes.First();

        AvailableVersions = new ObservableCollection<PsychonautsVersion>
        {
            new(PsychoPortal.PsychonautsVersion.Xbox_Proto_20041217, "Xbox (Prototype - 2004/12/17)"),
            new(PsychoPortal.PsychonautsVersion.Xbox_Proto_20050214, "Xbox (Prototype - 2005/02/14)"),
            new(PsychoPortal.PsychonautsVersion.Xbox, "Xbox"),
            new(PsychoPortal.PsychonautsVersion.PS2, "PS2"),
            new(PsychoPortal.PsychonautsVersion.PC_Original, "PC (Original)"),
            new(PsychoPortal.PsychonautsVersion.PC_Digital, "PC (Digital)"),
        };
        SelectedVersion = AvailableVersions.First(x => x.Version == _lastVersion);

        BrowseFilePathCommand = new RelayCommand(BrowseFilePath);
    }

    #endregion

    #region Private Fields

    private static PsychoPortal.PsychonautsVersion _lastVersion = PsychoPortal.PsychonautsVersion.PC_Digital;

    private string? _filePath;
    private PsychonautsVersion _selectedVersion;

    #endregion

    #region Commands

    public ICommand BrowseFilePathCommand { get; }

    #endregion

    #region Services

    public AppUIManager AppUI { get; }

    #endregion

    #region Public Properties

    public string? FilePath
    {
        get => _filePath;
        set
        {
            _filePath = value;

            if (value == null) 
                return;
            
            IFileType? type = FileTypes.FindFileType(value);

            if (type != null)
                SelectedFileType = type;
        }
    }
    public ObservableCollection<IFileType> AvailableFileTypes { get; }
    public IFileType SelectedFileType { get; set; }
    public ObservableCollection<PsychonautsVersion> AvailableVersions { get; }
    public PsychonautsVersion SelectedVersion
    {
        get => _selectedVersion;
        [MemberNotNull(nameof(_selectedVersion))]
        set
        {
            _selectedVersion = value;
            _lastVersion = value.Version;
        }
    }
    public bool IsValid => File.Exists(FilePath);
    // TODO: Logger

    #endregion

    #region Public Methods

    public void BrowseFilePath()
    {
        string? file = AppUI.BrowseFile("Select file path", IsValid ? FilePath : null);

        if (file != null)
            FilePath = file;
    }

    public FileContext CreateFileContext()
    {
        if (!File.Exists(FilePath))
            throw new FileNotFoundException("The file does not exist", FilePath);

        return new FileContext(FilePath, File.OpenRead(FilePath), new PsychonautsSettings(SelectedVersion.Version), null);
    }

    #endregion

    #region Records

    public record PsychonautsVersion(PsychoPortal.PsychonautsVersion Version, string DisplayName);

    #endregion
}