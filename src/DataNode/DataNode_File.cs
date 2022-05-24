namespace PsychonautsTools;

public class DataNode_File : BinaryDataNode<ByteArray>
{
    public DataNode_File(string fileName, ByteArray fileData) : base(fileData)
    {
        FileName = fileName;
    }

    public string FileName { get; }

    public override string TypeDisplayName => "File";
    public override string DisplayName => FileName;
}