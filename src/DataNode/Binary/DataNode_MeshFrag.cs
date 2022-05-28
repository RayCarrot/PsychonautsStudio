using System;
using System.Collections.Generic;
using System.Linq;
using PsychoPortal;

namespace PsychonautsTools;

public class DataNode_MeshFrag : BinaryDataNode<MeshFrag>
{
    public DataNode_MeshFrag(MeshFrag meshFrag, int index) : base(meshFrag)
    {
        Index = index;
    }

    public int Index { get; }

    public override string TypeDisplayName => "Mesh Fragment";
    public override string DisplayName => $"{Index}";
    public override bool HasChildren => false;

    public override IEnumerable<InfoItem> GetInfoItems()
    {
        foreach (InfoItem item in base.GetInfoItems())
            yield return item;

        yield return new InfoItem("Bounds", $"{SerializableObject.Bounds}");
        yield return new InfoItem("Has Animation Info", $"{SerializableObject.AnimInfo != null}");
        yield return new InfoItem("Material Flags", $"{SerializableObject.MaterialFlags}");

        if (SerializableObject.BlendshapeData != null)
            yield return new InfoItem("Blend Shapes", $"{String.Join(", ", SerializableObject.BlendshapeData.Streams.Select(x => x.BlendMappingName))}");
        
        yield return new InfoItem("Level of Detail", $"{SerializableObject.DistantLOD}");
        yield return new InfoItem("Polygons", $"{SerializableObject.PolygonCount}");
        yield return new InfoItem("Vertices", $"{SerializableObject.Vertices?.Length ?? 0}");
    }
}