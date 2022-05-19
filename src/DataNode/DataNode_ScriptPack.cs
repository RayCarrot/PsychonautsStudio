using System.Collections.Generic;
using PsychoPortal;

namespace PsychonautsTools;

public class DataNode_ScriptPack : DataNode
{
    public DataNode_ScriptPack(ScriptPack scriptPack, string fileName)
    {
        ScriptPack = scriptPack;
        FileName = fileName;
    }

    public ScriptPack ScriptPack { get; }
    public string FileName { get; }

    public override string TypeDisplayName => "Script Pack";
    public override string DisplayName => FileName;
    public override bool HasChildren => ScriptPack.Classes.AnyAndNotNull() ||
                                        ScriptPack.Classes.AnyAndNotNull();
    public override IBinarySerializable SerializableObject => ScriptPack;

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        if (ScriptPack.Classes != null)
            foreach (ScriptClass scriptClass in ScriptPack.Classes)
                yield return new DataNode_ScriptClass(scriptClass);

        if (ScriptPack.DoFiles != null)
            foreach (ScriptDoFile scriptDoFile in ScriptPack.DoFiles)
                yield return new DataNode_ScriptDoFile(scriptDoFile);
    }
}