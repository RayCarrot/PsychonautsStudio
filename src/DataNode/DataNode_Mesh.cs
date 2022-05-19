using PsychoPortal;
using System.Collections.Generic;

namespace PsychonautsTools;

public class DataNode_Mesh : DataNode
{
    public DataNode_Mesh(Mesh mesh)
    {
        Mesh = mesh;
    }

    public Mesh Mesh { get; }

    public override string TypeDisplayName => "Mesh";
    public override string DisplayName => Mesh.Name;
    public override bool HasChildren => Mesh.Children.AnyAndNotNull();
    public override IBinarySerializable SerializableObject => Mesh;

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        if (Mesh.Children != null)
            foreach (Mesh mesh in Mesh.Children)
                yield return new DataNode_Mesh(mesh);
    }
}