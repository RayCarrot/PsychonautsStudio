using PsychoPortal;

namespace PsychonautsStudio;

public class DataNode_ScriptClass : BinaryDataNode<ScriptClass>
{
    public DataNode_ScriptClass(ScriptClass scriptClass) : base(scriptClass) { }

    public override string TypeDisplayName => "Script Class";
    public override string DisplayName => SerializableObject.ClassName;
}