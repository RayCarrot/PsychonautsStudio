using PsychoPortal;

namespace PsychonautsTools;

public class DataNode_ScriptDoFile : DataNode
{
    public DataNode_ScriptDoFile(ScriptDoFile scriptDoFile)
    {
        ScriptDoFile = scriptDoFile;
    }

    public ScriptDoFile ScriptDoFile { get; }

    public override string TypeDisplayName => "Script Do File";
    public override string DisplayName => ScriptDoFile.Name;
    public override GenericIconKind IconKind => GenericIconKind.DataNode_ScriptDoFile;
    public override IBinarySerializable SerializableObject => ScriptDoFile;
}