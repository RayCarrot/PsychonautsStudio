using System.IO;
using PsychoPortal;

namespace PsychonautsTools;

public class FileType_ScratchDrive : BinaryFileType<ScratchInstallation>
{
    public override string[] FileExtensions => new[] { ".bin" };
    public override bool LeaveFileStreamOpen => true;
    public override string ID => "ScratchDrive";
    public override string DisplayName => "Scratch Drive (.bin)";

    protected override DataNode CreateDataNode(ScratchInstallation obj, FileContext fileContext) => new DataNode_ScratchInstallation(obj, Path.GetFileName(fileContext.FilePath));
}