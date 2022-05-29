using System.IO;
using PsychoPortal;

namespace PsychonautsStudio;

public class FileType_PS2 : BinaryFileType<PS2_Texture>
{
    public override string[] FileExtensions => new[] { ".ps2" };
    public override string ID => "PS2";
    public override string DisplayName => "PS2 Texture (.ps2)";

    protected override DataNode CreateDataNode(PS2_Texture obj, FileContext fileContext) => new DataNode_PS2_Texture(obj, Path.GetFileName(fileContext.FilePath));
}