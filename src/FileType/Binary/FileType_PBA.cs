using System.IO;
using PsychoPortal;

namespace PsychonautsStudio;

public class FileType_PBA : BinaryFileType<SharedBlendAnim>
{
    public override string[] FileExtensions => new[] { ".pba" };
    public override string ID => "PBA";
    public override string DisplayName => "Blend Animation (.pba)";

    protected override DataNode CreateDataNode(SharedBlendAnim obj, FileContext fileContext) => new DataNode_SharedBlendAnim(obj, Path.GetFileName(fileContext.FilePath));
}