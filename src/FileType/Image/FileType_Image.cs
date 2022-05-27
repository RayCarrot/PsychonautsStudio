using System.IO;

namespace PsychonautsTools;

public class FileType_Image : IFileType
{
    public string[] FileExtensions => new[] { ".dds", ".tga" };
    public bool LeaveFileStreamOpen => false;
    public string ID => "IMG";
    public string DisplayName => "Image (.dds, .tga)";

    public DataNode CreateDataNode(FileContext fileContext)
    {
        return new DataNode_Image(fileContext.FileStream, Path.GetFileName(fileContext.FilePath));
    }
}