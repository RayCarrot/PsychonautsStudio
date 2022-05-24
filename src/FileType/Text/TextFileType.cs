using System.IO;
using System.Text;

namespace PsychonautsTools;

public abstract class TextFileType : IFileType
{
    protected virtual Encoding Encoding => Encoding.ASCII;

    public abstract string[] FileExtensions { get; }
    public virtual bool LeaveFileStreamOpen => true;
    public abstract string ID { get; }
    public abstract string DisplayName { get; }

    public DataNode CreateDataNode(FileContext fileContext)
    {
        using StreamReader reader = new(fileContext.FileStream, Encoding);
        return new DataNode_Text(Path.GetFileName(fileContext.FilePath), reader.ReadToEnd());
    }
}