using PsychoPortal;

namespace PsychonautsTools;

public class FileType_PKG : BinaryFileType<Package>
{
    public override string[] FileExtensions => new[] { ".pkg" };
    public override bool LeaveFileStreamOpen => true;
    public override string ID => "PKG";
    public override string DisplayName => "Package (.pkg)";

    protected override DataNode CreateDataNode(Package obj, FileContext fileContext) => new DataNode_Package(obj, fileContext);
}