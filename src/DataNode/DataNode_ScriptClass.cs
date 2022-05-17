using PsychoPortal;

namespace PsychonautsTools;

public class DataNode_ScriptClass : DataNode
{
    public DataNode_ScriptClass(ScriptClass scriptClass)
    {
        ScriptClass = scriptClass;
    }

    public ScriptClass ScriptClass { get; }

    public override string TypeDisplayName => "Script Class";
    public override string DisplayName => ScriptClass.ClassName;
    public override GenericIconKind IconKind => GenericIconKind.DataNode_ScriptClass;
    public override IBinarySerializable SerializableObject => ScriptClass;
}