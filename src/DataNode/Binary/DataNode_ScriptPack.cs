using System.Collections.Generic;
using PsychoPortal;

namespace PsychonautsStudio;

public class DataNode_ScriptPack : BinaryDataNode<ScriptPack>
{
    public DataNode_ScriptPack(ScriptPack scriptPack, string displayName) : base(scriptPack)
    {
        DisplayName = displayName;
    }

    public override string TypeDisplayName => "Script Pack";
    public override string DisplayName { get; }
    public override bool HasChildren => SerializableObject.Classes.AnyAndNotNull() ||
                                        SerializableObject.Classes.AnyAndNotNull();

    public override IEnumerable<InfoItem> GetInfoItems()
    {
        foreach (InfoItem item in base.GetInfoItems())
            yield return item;

        yield return new InfoItem("Classes", $"{SerializableObject.Classes?.Length ?? 0}");
        yield return new InfoItem("Do Files", $"{SerializableObject.DoFiles?.Length ?? 0}");
    }

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        if (SerializableObject.Classes != null)
            foreach (ScriptClass scriptClass in SerializableObject.Classes)
                yield return new DataNode_ScriptClass(scriptClass);

        if (SerializableObject.DoFiles != null)
            foreach (ScriptDoFile scriptDoFile in SerializableObject.DoFiles)
                yield return new DataNode_ScriptDoFile(scriptDoFile);
    }
}