using PsychoPortal;
using System.Collections.Generic;
using System.IO;

namespace PsychonautsTools;

public class DataNode_PackPack : DataNode
{
    public DataNode_PackPack(PackPack packPack, string filePath)
    {
        PackPack = packPack;
        FilePath = filePath;
        DisplayName = Path.GetFileName(filePath);
    }

    public PackPack PackPack { get; }
    public string FilePath { get; }

    public override string TypeDisplayName => "Pack Pack";
    public override string DisplayName { get; }
    public override bool HasChildren => true;
    public override IBinarySerializable SerializableObject => PackPack;

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        string name = Path.GetFileNameWithoutExtension(FilePath);

        yield return new DataNode_TexturePack(PackPack.TexturePack, $"{name}.tpf");
        yield return new DataNode_MeshPack(PackPack.MeshPack, $"{name}.mpf");
        yield return new DataNode_ScriptPack(PackPack.ScriptPack, $"{name}.lpf");

        if (PackPack.Scene != null)
            yield return new DataNode_Scene(PackPack.Scene, $"{name}.plb");
    }
}