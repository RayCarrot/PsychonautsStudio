using System;
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

    private string GetFileExtension(byte[] buffer)
    {
        uint magic = BitConverter.ToUInt32(buffer, 0);

        if (magic == 0x4150414B) // APAK
            return ".apf";
        else if (magic == 0x45504241) // EPBA
            return ".pba";
        else if (magic == 0x50455645) // PEVE
            return ".eve";
        else if (magic is 0x504A4158 or 0x504A414E) // PJAX or PJAN
            return ".ja2";
        else if (magic == 0x50535943) // PSYC
            return ".pl2";
        else if (magic == 0x4B415050) // PPAK
            return ".ppf";
        else if (magic == 0x61754C1B) // .LUA
            return ".lub";
        else if (magic == 0x69746341) // Acti(onFile)
            return ".asd";
        else if (magic == 0x4543414D) // MACE
            return ".cam";
        else if (magic == 0xBA010000)
            return ".pss";
        else if ((magic & 0xFFFF) == 0x4257) // WB
            return ".pwb";
        else if ((magic & 0xFFFF) == 0x4253) // SB
            return ".psb";
        else if ((magic & 0xFFFFFF) == 0x325350) // PS2
            return ".ps2";
        else if (magic == 0x73504C43) // CLPs(ychoMunger)
            return ".txt";
        else
            return String.Empty;
    }

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
            getFilePathFunc: x =>
            {
                if (fileTable.TryGetValue(x.FilePathHash, out string? v))
                    return v;

                string ext = String.Empty;

                byte[] buffer = x.ReadFile(pakStreams, 4);

                if (buffer != null)
                    ext = GetFileExtension(buffer);

                return $"{x.FilePathHash:X8}{ext}";
            },
            getFileStreamFunc: (file, _) => new MemoryStream(file.ReadFile(pakStreams)));
    }
}