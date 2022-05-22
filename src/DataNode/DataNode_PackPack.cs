using PsychoPortal;
using System.Collections.Generic;
using System.IO;

namespace PsychonautsTools;

public class DataNode_PackPack : BinaryDataNode<PackPack>
{
    public DataNode_PackPack(PackPack packPack, string filePath) : base(packPack)
    {
        FilePath = filePath;
        DisplayName = Path.GetFileName(filePath);
    }

    public string FilePath { get; }

    public override string TypeDisplayName => "Pack Pack";
    public override string DisplayName { get; }
    public override bool HasChildren => true;

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        string name = Path.GetFileNameWithoutExtension(FilePath);

        yield return new DataNode_TexturePack(SerializableObject.TexturePack, $"{name}.tpf");
        yield return new DataNode_MeshPack(SerializableObject.MeshPack, $"{name}.mpf");
        yield return new DataNode_ScriptPack(SerializableObject.ScriptPack, $"{name}.lpf");

        if (SerializableObject.Scene != null)
            yield return new DataNode_Scene(SerializableObject.Scene, $"{name}.plb");
    }
}