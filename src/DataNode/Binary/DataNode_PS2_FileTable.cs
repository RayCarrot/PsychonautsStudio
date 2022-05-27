using System.Collections.Generic;
using System.IO;
using System.Linq;
using PsychoPortal;

namespace PsychonautsTools;

public class DataNode_PS2_FileTable : BinaryDataNode<PS2_FileTable>
{
    public DataNode_PS2_FileTable(PS2_FileTable fileTable, string displayName) : base(fileTable)
    {
        DisplayName = displayName;
    }

    public override string TypeDisplayName => "PS2 Pack";
    public override string DisplayName { get; }
    public override bool HasChildren => SerializableObject.Files.AnyAndNotNull();

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        Dictionary<uint, string> fileTable = PS2_FileNames.FileTable.ToDictionary(x => PS2_FileEntry.GetFilePathHash(x));

        yield return DataNode_Folder.FromUntypedFiles(
            files: SerializableObject.Files.Where(x => x.FileSize >= 0),
            fileContext: fileContext,
            getFilePathFunc: x => fileTable.TryGetValue(x.FilePathHash, out string? v) ? v : $"{x.FilePathHash:X8}",
            getFileStreamFunc: (file, _) => new MemoryStream(file.ReadFile(fileContext.FileStream)));
    }
}