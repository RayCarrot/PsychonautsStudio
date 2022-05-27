using System;

namespace PsychonautsTools;

public class RootDataNodeViewModel : DataNodeViewModel, IDisposable
{
    public RootDataNodeViewModel(DataNode node, FileContext fileContext, IFileType fileType) : base(node, null, null)
    {
        FileContext = fileContext;
        FileType = fileType;
    }

    public FileContext FileContext { get; }
    public IFileType FileType { get; }

    public override void Dispose()
    {
        base.Dispose();
        FileContext.Dispose();
    }
}