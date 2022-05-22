using PsychoPortal;
using System.Collections.Generic;

namespace PsychonautsTools;

public class DataNode_Mesh : BinaryDataNode<Mesh>
{
    public DataNode_Mesh(Mesh mesh) : base(mesh) { }

    public override string TypeDisplayName => "Mesh";
    public override string DisplayName => SerializableObject.Name;
    public override bool HasChildren => SerializableObject.Children.AnyAndNotNull();

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        if (SerializableObject.Children != null)
            foreach (Mesh mesh in SerializableObject.Children)
                yield return new DataNode_Mesh(mesh);
    }
}