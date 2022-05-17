using PsychoPortal;
using System.Collections.Generic;
using System.IO;

namespace PsychonautsTools;

public class DataNode_Package : DataNode
{
    public DataNode_Package(Package package, FileContext fileContext)
    {
        Package = package;
        FileContext = fileContext;
        DisplayName = Path.GetFileName(fileContext.FilePath);
    }

    public Package Package { get; }
    public FileContext FileContext { get; }

    public override string TypeDisplayName => "Package";
    public override string DisplayName { get; }
    public override bool HasChildren => Package.Files.AnyAndNotNull();
    public override GenericIconKind IconKind => GenericIconKind.DataNode_Package;
    public override IBinarySerializable SerializableObject => Package;

    public override IEnumerable<DataNode> CreateChildren()
    {
        yield return DataNode_Folder.FromUntypedFiles(
            files: Package.GetFiles(), 
            fileContext: FileContext, 
            getFilePathFunc: x => x.FilePath, 
            getFileStreamFunc: x => x.FileEntry.GetFileStream(FileContext.FileStream));
    }
}