using PsychoPortal;

namespace PsychonautsStudio;

public class DataNode_ScriptDoFile : BinaryDataNode<ScriptDoFile>
{
    public DataNode_ScriptDoFile(ScriptDoFile scriptDoFile) : base(scriptDoFile) { }

    public override string TypeDisplayName => "Script Do File";
    public override string DisplayName => SerializableObject.Name; // TODO: Name might be null
}