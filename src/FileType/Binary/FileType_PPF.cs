using System;
using System.IO;
using PsychoPortal;

namespace PsychonautsTools;

public class FileType_PPF : BinaryFileType<PackPack>
{
    public override string[] FileExtensions => new[] { ".ppf" };
    public override string ID => "PPF";
    public override string DisplayName => "Pack Pack (.ppf)";

    protected override DataNode CreateDataNode(PackPack obj, FileContext fileContext) => new DataNode_PackPack(obj, fileContext.FilePath);
    protected override void OnPreSerialize(FileContext fileContext, PackPack obj)
    {
        if (Path.GetFileNameWithoutExtension(fileContext.FilePath).Equals("common", StringComparison.InvariantCultureIgnoreCase))
            obj.Pre_IsCommon = true;
    }
}