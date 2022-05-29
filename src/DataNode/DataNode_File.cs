using System.IO;

namespace PsychonautsStudio;

public class DataNode_File : BinaryDataNode<ByteArray>
{
    public DataNode_File(string fileName, ByteArray fileData) : base(fileData)
    {
        FileName = fileName;
    }

    public string FileName { get; }

    public override string TypeDisplayName => "File";
    public override string DisplayName => FileName;

    public static DataNode_File FromStream(string fileName, Stream fileStream)
    {
        byte[] fileData = new byte[fileStream.Length];
        fileStream.Position = 0;
        int read = fileStream.Read(fileData, 0, fileData.Length);
        // TODO: Verify amount of read bytes
        return new DataNode_File(fileName, new ByteArray(fileData));
    }
}