namespace PsychonautsStudio;

public interface IFileType
{
    public string[] FileExtensions { get; }
    public bool LeaveFileStreamOpen { get; }
    public string ID { get; }
    public string DisplayName { get; }

    public DataNode CreateDataNode(FileContext fileContext);
}