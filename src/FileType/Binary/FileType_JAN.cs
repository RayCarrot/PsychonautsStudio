using System.IO;
using PsychoPortal;

namespace PsychonautsTools;

public class FileType_JAN : BinaryFileType<SharedSkelAnim>
{
    public override string[] FileExtensions => new[] { ".jan", ".ja2" };
    public override string ID => "JAN";
    public override string DisplayName => "Skeleton Animation (.jan, .ja2)";

    protected override DataNode CreateDataNode(SharedSkelAnim obj, FileContext fileContext) => new DataNode_SharedSkelAnim(obj, Path.GetFileName(fileContext.FilePath));
}