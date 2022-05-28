using PsychoPortal;
using System.Collections.Generic;
using System.Linq;

namespace PsychonautsTools;

public class DataNode_Domain : BinaryDataNode<Domain>
{
    public DataNode_Domain(Domain domain) : base(domain) { }

    public override string TypeDisplayName => "Domain";
    public override string DisplayName => SerializableObject.Name;
    public override bool HasChildren => SerializableObject.Children.AnyAndNotNull() ||
                                        SerializableObject.Meshes.AnyAndNotNull();

    public override IEnumerable<InfoItem> GetInfoItems()
    {
        foreach (InfoItem item in base.GetInfoItems())
            yield return item;

        yield return new InfoItem("Bounds", $"{SerializableObject.Bounds}");
        yield return new InfoItem("Domain Entities", $"{SerializableObject.DomainEntityInfos?.Length ?? 0}"); // TODO: Show these
        yield return new InfoItem("Entity Init Data", $"{SerializableObject.EntityInitDatas?.Length ?? 0}");
        yield return new InfoItem("Runtime References", $"{SerializableObject.RuntimeReferences?.Length ?? 0}");
    }

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