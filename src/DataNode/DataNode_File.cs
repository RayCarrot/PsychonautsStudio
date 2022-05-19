namespace PsychonautsTools;

public class DataNode_File : DataNode
{
    public DataNode_File(string fileName)
    {
        FileName = fileName;
    }

    public string FileName { get; }

    public override string TypeDisplayName => "File";
    public override string DisplayName => FileName;
}