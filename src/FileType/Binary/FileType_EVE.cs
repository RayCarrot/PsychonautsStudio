using System.IO;
using PsychoPortal;

namespace PsychonautsTools;

public class FileType_EVE : BinaryFileType<EventAnim>
{
    public override string[] FileExtensions => new[] { ".eve" };
    public override string ID => "EVE";
    public override string DisplayName => "Event Animation (.eve)";

    protected override DataNode CreateDataNode(EventAnim obj, FileContext fileContext) => new DataNode_EventAnim(obj, Path.GetFileName(fileContext.FilePath));
}