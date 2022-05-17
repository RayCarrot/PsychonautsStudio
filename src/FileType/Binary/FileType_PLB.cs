using System.IO;
using PsychoPortal;

namespace PsychonautsTools;

public class FileType_PLB : BinaryFileType<Scene>
{
    public override string[] FileExtensions => new[] { ".plb", ".pl2" };
    public override string ID => "PLB";
    public override string DisplayName => "Scene (.plb, .pl2)";

    protected override DataNode CreateDataNode(Scene obj, FileContext fileContext) => 
        new DataNode_Scene(obj, Path.GetFileName(fileContext.FilePath));
}