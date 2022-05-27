using System.IO;
using PsychoPortal;

namespace PsychonautsTools;

public class FileType_PAK : BinaryFileType<PS2_FileTable>
{
    public override string[] FileExtensions => new[] { ".pak" };
    public override bool LeaveFileStreamOpen => true;
    public override string ID => "PAK";
    public override string DisplayName => "PS2 Pack (.pak)";

    protected override DataNode CreateDataNode(PS2_FileTable obj, FileContext fileContext) => new DataNode_PS2_FileTable(obj, Path.GetFileName(fileContext.FilePath));
}