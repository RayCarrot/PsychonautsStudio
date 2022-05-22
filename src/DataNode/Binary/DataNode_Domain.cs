using PsychoPortal;
using System.Collections.Generic;

namespace PsychonautsTools;

public class DataNode_Domain : BinaryDataNode<Domain>
{
    public DataNode_Domain(Domain domain) : base(domain) { }

    public override string TypeDisplayName => "Domain";
    public override string DisplayName => SerializableObject.Name;
    public override bool HasChildren => SerializableObject.Children.AnyAndNotNull() ||
                                        SerializableObject.Meshes.AnyAndNotNull();

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        if (SerializableObject.Children != null)
            foreach (Domain domain in SerializableObject.Children)
                yield return new DataNode_Domain(domain);

        if (SerializableObject.Meshes != null)
            foreach (Mesh mesh in SerializableObject.Meshes)
                yield return new DataNode_Mesh(mesh);
    }
}