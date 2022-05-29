using System;
using PsychoPortal;
using System.Collections.Generic;
using System.Linq;

namespace PsychonautsStudio;

public class DataNode_Package : BinaryDataNode<Package>
{
    public DataNode_Package(Package package, string displayName) : base(package)
    {
        DisplayName = displayName;
    }

    public override string TypeDisplayName => "Package";
    public override string DisplayName { get; }
    public override bool HasChildren => SerializableObject.Files.AnyAndNotNull();

    public override IEnumerable<InfoItem> GetInfoItems()
    {
        foreach (InfoItem item in base.GetInfoItems())
            yield return item;

        yield return new InfoItem("Version", $"{SerializableObject.Version}");
    }

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        yield return DataNode_Folder.FromUntypedFiles(
            files: SerializableObject.GetFiles(),
            fileContext: fileContext,
            getFilePathFunc: x => x.FilePath,
            getFileStreamFunc: (file, _) => file.FileEntry.GetFileStream(fileContext.FileStream));
    }
}