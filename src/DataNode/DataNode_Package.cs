using PsychoPortal;
using System.Collections.Generic;

namespace PsychonautsTools;

public class DataNode_Package : DataNode
{
    public DataNode_Package(Package package, string displayName)
    {
        Package = package;
        DisplayName = displayName;
    }

    public Package Package { get; }

    public override string TypeDisplayName => "Package";
    public override string DisplayName { get; }
    public override bool HasChildren => Package.Files.AnyAndNotNull();
    public override IBinarySerializable SerializableObject => Package;

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        yield return DataNode_Folder.FromUntypedFiles(
            files: Package.GetFiles(), 
            fileContext: fileContext, 
            getFilePathFunc: x => x.FilePath, 
            getFileStreamFunc: (file, _) => file.FileEntry.GetFileStream(fileContext.FileStream));
    }
}