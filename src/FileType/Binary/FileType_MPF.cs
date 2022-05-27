using System.IO;
using PsychoPortal;

namespace PsychonautsTools;

public class FileType_MPF : BinaryFileType<MeshPack>
{
    public override string[] FileExtensions => new[] { ".mpf" };
    public override string ID => "MPF";
    public override string DisplayName => "Mesh Pack (.mpf)";

    protected override DataNode CreateDataNode(MeshPack obj, FileContext fileContext) => new DataNode_MeshPack(obj, Path.GetFileName(fileContext.FilePath));
}