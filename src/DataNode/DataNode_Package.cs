using PsychoPortal;
using System.Collections.Generic;

namespace PsychonautsTools;

public class DataNode_Package : BinaryDataNode<Package>
{
    public DataNode_Package(Package package, string displayName) : base(package)
    {
        DisplayName = displayName;
    }

    public override string TypeDisplayName => "Package";
    public override string DisplayName { get; }
    public override bool HasChildren => SerializableObject.Files.AnyAndNotNull();

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        yield return DataNode_Folder.FromUntypedFiles(
            files: SerializableObject.GetFiles(),
            fileContext: fileContext,
            getFilePathFunc: x => x.FilePath,
            getFileStreamFunc: (file, _) => file.FileEntry.GetFileStream(fileContext.FileStream));
    }
}