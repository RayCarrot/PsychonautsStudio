using System.IO;
using PsychoPortal;

namespace PsychonautsTools;

public class FileType_TPF : BinaryFileType<TexturePack>
{
    public override string[] FileExtensions => new[] { ".tpf" };
    public override string ID => "TPF";
    public override string DisplayName => "Texture Pack (.tpf)";

    protected override DataNode CreateDataNode(TexturePack obj, FileContext fileContext) => new DataNode_TexturePack(obj, Path.GetFileName(fileContext.FilePath));
}