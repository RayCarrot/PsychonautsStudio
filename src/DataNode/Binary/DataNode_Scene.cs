using PsychoPortal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PsychonautsStudio;

public class DataNode_Scene : BinaryDataNode<Scene>
{
    public DataNode_Scene(Scene scene, string displayName, PLBType? type = null) : base(scene)
    {
        DisplayName = displayName;
        Type = type;
        EditorViewModel = new SceneEditorViewModel(scene);
    }

    public PLBType? Type { get; }

    public override string TypeDisplayName => "Scene";
    public override string DisplayName { get; }
    public override bool HasChildren => true;
    public override SceneEditorViewModel EditorViewModel { get; }

    public override IEnumerable<InfoItem> GetInfoItems()
    {
        foreach (InfoItem item in base.GetInfoItems())
            yield return item;

        yield return new InfoItem("Type", $"{Type}");
        yield return new InfoItem("Version", $"{SerializableObject.Version}");
        yield return new InfoItem("Flags", $"{SerializableObject.Flags}");
        yield return new InfoItem("Textures", $"{SerializableObject.TextureTranslationTable.Length}");
        yield return new InfoItem("Navigation Meshes", $"{SerializableObject.NavMeshes.Length}");
    }

    public override IEnumerable<DataNode> CreateChildren(FileContext fileContext)
    {
        yield return new DataNode_Domain(SerializableObject.RootDomain);
    }
}