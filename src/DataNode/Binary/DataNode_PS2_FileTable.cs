using System.Collections.Generic;
using System.IO;
using System.Linq;
using PsychoPortal;

namespace PsychonautsStudio;

public class DataNode_PS2_FileTable : BinaryDataNode<PS2_FileTable>
{
    public DataNode_PS2_FileTable(PS2_FileTable fileTable, string displayName) : base(fileTable)
    {
        DisplayName = displayName;
    }

    public override string TypeDisplayName => "PS2 Pack";
    public override string DisplayName { get; }
    public override bool HasChildren => SerializableObject.Files.AnyAndNotNull();

    public override IEnumerable<InfoItem> GetInfoItems()
    {
        foreach (InfoItem item in base.GetInfoItems())
            yield return item;

        yield return new InfoItem("File Table Entries", $"{SerializableObject.Files?.Length ?? 0}");
    }

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        Dictionary<uint, string> fileTable = PS2_FileNames.FileTable.ToDictionary(PS2_FileEntry.GetFilePathHash);

        // The PAL version has a second pack
        long masterPackLength = fileContext.FileStream.Length;
        bool hasSecondPack = SerializableObject.Files.Any(x => x.AbsoluteOffset >= masterPackLength);

        Stream[] pakStreams;
        
        if (hasSecondPack)
        {
            const string key = "PS2_PAK2";

            if (!fileContext.HasDependency(key))
                fileContext.AddDependency(key, Path.Combine(Path.GetDirectoryName(fileContext.FilePath), "RESOURC2.PAK")); // Hard-code for now

            pakStreams = new[] { fileContext.FileStream, fileContext.GetDependency(key).FileStream };
        }
        else
        {
            pakStreams = new[] { fileContext.FileStream };
        }

        yield return DataNode_Folder.FromUntypedFiles(
            files: SerializableObject.Files.Where(x => x.FileSize >= 0),
            fileContext: fileContext,
            getFilePathFunc: x => fileTable.TryGetValue(x.FilePathHash, out string? v) ? v : $"{x.FilePathHash:X8}",
            getFileStreamFunc: (file, _) => new MemoryStream(file.ReadFile(pakStreams)));
    }
}