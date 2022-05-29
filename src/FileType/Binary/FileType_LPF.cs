using System.IO;
using PsychoPortal;

namespace PsychonautsStudio;

public class FileType_LPF : BinaryFileType<ScriptPack>
{
    public override string[] FileExtensions => new[] { ".lpf" };
    public override string ID => "LPF";
    public override string DisplayName => "Script Pack (.lpf)";

    protected override DataNode CreateDataNode(ScriptPack obj, FileContext fileContext) => new DataNode_ScriptPack(obj, Path.GetFileName(fileContext.FilePath));
}