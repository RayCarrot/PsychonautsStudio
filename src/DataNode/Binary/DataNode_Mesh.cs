using System;
using PsychoPortal;
using System.Collections.Generic;
using System.Linq;

namespace PsychonautsTools;

public class DataNode_Mesh : BinaryDataNode<Mesh>
{
    public DataNode_Mesh(Mesh mesh) : base(mesh) { }

    public override string TypeDisplayName => "Mesh";
    public override string DisplayName => SerializableObject.Name;
    public override bool HasChildren => SerializableObject.Children.AnyAndNotNull() ||
                                        SerializableObject.MeshFrags.AnyAndNotNull();

    public override IEnumerable<InfoItem> GetInfoItems()
    {
        foreach (InfoItem item in base.GetInfoItems())
            yield return item;

        yield return new InfoItem("Bounds", $"{SerializableObject.Bounds}");
        yield return new InfoItem("Position", $"{SerializableObject.Position}");
        yield return new InfoItem("Rotation", $"{SerializableObject.Rotation}"); // TODO Normalize degrees/radians as game uses both
        yield return new InfoItem("Scale", $"{SerializableObject.Scale}");
        yield return new InfoItem("Animation Affectors", $"{SerializableObject.AnimAffectors?.Length ?? 0}");
        yield return new InfoItem("Has Collision", $"{SerializableObject.CollisionTree != null}");

        if (SerializableObject.EntityMeshInfo != null)
        {
            yield return new InfoItem("Mesh Entity Script Class", $"{SerializableObject.EntityMeshInfo.ScriptClass}");
            yield return new InfoItem("Mesh Entity Edit Vars", $"{SerializableObject.EntityMeshInfo.EditVars}");
        }

        if (SerializableObject.LODs.AnyAndNotNull())
            yield return new InfoItem("Levels of Detail", $"{String.Join(", ", SerializableObject.LODs)}");

        if (SerializableObject.Lights.AnyAndNotNull())
            yield return new InfoItem("Lights", $"{String.Join(", ", SerializableObject.Lights.Select(x => x.Type))}");

        if (SerializableObject.Triggers.AnyAndNotNull())
            yield return new InfoItem("Triggers", $"{String.Join(", ", SerializableObject.Triggers.Select(x => x.TriggerName))}");

        if (SerializableObject.Skeletons.AnyAndNotNull())
            yield return new InfoItem("Skeletons", $"{String.Join(", ", SerializableObject.Skeletons.Select(x => x.Name))}");
    }

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        if (SerializableObject.Children != null)
            foreach (Mesh mesh in SerializableObject.Children)
                yield return new DataNode_Mesh(mesh);

        for (var i = 0; i < SerializableObject.MeshFrags.Length; i++)
        {
            MeshFrag frag = SerializableObject.MeshFrags[i];
            yield return new DataNode_MeshFrag(frag, i);
        }
    }
}