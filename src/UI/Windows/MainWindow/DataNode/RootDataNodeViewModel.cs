using System;

namespace PsychonautsTools;

public class RootDataNodeViewModel : DataNodeViewModel, IDisposable
{
    public RootDataNodeViewModel(DataNode node, FileContext fileContext, IFileType fileType) : base(node, null, null, fileContext)
    {
        FileContext = fileContext;
        FileType = fileType;

        InfoItems.Insert(0, new InfoItem("File Path", FileContext.FilePath));
        InfoItems.Insert(1, new InfoItem("Version", $"{FileContext.Settings.Version}"));
        InfoItems.Insert(2, new InfoItem("File Type", $"{FileType.DisplayName}"));
    }

    public FileContext FileContext { get; }
    public IFileType FileType { get; }

    public override void Dispose()
    {
        base.Dispose();
        FileContext.Dispose();
    }
}