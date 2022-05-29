using System.Collections.Generic;
using System.IO;
using System.Linq;
using PsychoPortal;

namespace PsychonautsStudio;

public class DataNode_ScratchInstallation : BinaryDataNode<ScratchInstallation>
{
    public DataNode_ScratchInstallation(ScratchInstallation package, string displayName) : base(package)
    {
        DisplayName = displayName;
    }

    public override string TypeDisplayName => "Scratch Installation";
    public override string DisplayName { get; }
    public override bool HasChildren => SerializableObject.Directories.AnyAndNotNull();

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        yield return DataNode_Folder.FromUntypedFiles(
            files: SerializableObject.Directories.SelectMany(x => x.Files.Select(f => new
            {
                FileEntry = f,
                FilePath = Path.Combine(x.Name, f.Name),
            })),
            fileContext: fileContext,
            getFilePathFunc: x => x.FilePath,
            getFileStreamFunc: (file, _) => file.FileEntry.GetFileStream(fileContext.FileStream));
    }
}