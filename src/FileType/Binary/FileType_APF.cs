using System.IO;
using PsychoPortal;

namespace PsychonautsStudio;

public class FileType_APF : BinaryFileType<AnimPack>
{
    public override string[] FileExtensions => new[] { ".apf" };
    public override string ID => "APF";
    public override string DisplayName => "Animation Pack (.apf)";

    protected override DataNode CreateDataNode(AnimPack obj, FileContext fileContext) => new DataNode_AnimPack(obj, Path.GetFileName(fileContext.FilePath));
}