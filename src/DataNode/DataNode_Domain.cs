using PsychoPortal;
using System.Collections.Generic;

namespace PsychonautsTools;

public class DataNode_Domain : DataNode
{
    public DataNode_Domain(Domain domain)
    {
        Domain = domain;
    }

    public Domain Domain { get; }

    public override string TypeDisplayName => "Domain";
    public override string DisplayName => Domain.Name;
    public override bool HasChildren => Domain.Children.AnyAndNotNull() ||
                                        Domain.Meshes.AnyAndNotNull();
    public override IBinarySerializable SerializableObject => Domain;

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        if (Domain.Children != null)
            foreach (Domain domain in Domain.Children)
                yield return new DataNode_Domain(domain);

        if (Domain.Meshes != null)
            foreach (Mesh mesh in Domain.Meshes)
                yield return new DataNode_Mesh(mesh);
    }
}